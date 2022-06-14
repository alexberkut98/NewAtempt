using ChatServer._EventArgs_;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Новая_попытка._Enums_;
using Новая_попытка.Packets;

namespace Новая_попытка
{
    public class TCPClient
    {
        private readonly SocketAsyncEventArgs _receiveEvent;
        private readonly SocketAsyncEventArgs _sendEvent;
        private readonly SocketAsyncEventArgs _connectEvent;
        private readonly Socket _socket;
        private int _disposed;
        private string _login;
        private int _sending;
        private readonly ConcurrentQueue<byte[]> _sendQueue;

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public int BUFFER_SIZE { get; private set; }
        public int SIZE_LENGTH { get; private set; }

        public TCPClient(string address, string port)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var receiveEvent = new SocketAsyncEventArgs();
            receiveEvent.SetBuffer(new byte[BUFFER_SIZE], 0, BUFFER_SIZE);
            receiveEvent.Completed += ReceiveCompleted;

            var sendEvent = new SocketAsyncEventArgs();
            sendEvent.SetBuffer(new byte[BUFFER_SIZE], 0, BUFFER_SIZE);
            sendEvent.Completed += SendCompleted;

            var connectEvent = new SocketAsyncEventArgs();
            connectEvent.Completed += ConnectCompleted;
            connectEvent.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(address), int.Parse(port));
            socket.ConnectAsync(_connectEvent);
        }



        //Добавление нового пользователя в список контактов.
        public void Connect(string address, string port)
        {
            //Возвращает удаленную точку доступа.
            _connectEvent.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(address), int.Parse(port));

            //Выполняет асинхронное подключение к удаленному узлу.
            _socket.ConnectAsync(_connectEvent);
        }

        private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            /* Подключение завершено */
            if (e.SocketError != SocketError.Success)
            {
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, false));
                return;
            }

            ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, true));
            Receive();
        }

        //Удаление пользователя из списка контактов
        public void Disconnect()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 1)
                return;

            //Отправить в БД собщение о том, что данный логин отключился от сервера.

            Safe(() => _socket.Dispose());
            Safe(() => _sendEvent.Dispose());
            Safe(() => _receiveEvent.Dispose());

            _login = string.Empty;
        }

        private void SendImpl()
        {

            //Проверка на то, что наш клиент не отключило от сервера.
            if (_disposed == 1)
                return;

            //Повторно проверяем занятость выходного потока, а так же убеждаемся, что сообщение можно извлечь из потока.
            //Кроме того, мы размещаем сообщение в массиве packet, с которого данные и будут отправлены в сервер.
            if (!_sendQueue.TryDequeue(out var packet) && Interlocked.CompareExchange(ref _sending, 0, 1) == 1)
                return;

            // Копирование данных из packet в буффер _sendEvent
            Array.Copy(packet, 0, _sendEvent.Buffer, SIZE_LENGTH, packet.Length);
            BufferPrimitives.SetUint16(_sendEvent.Buffer, 0, (ushort)packet.Length);


            //Задает настройки буфера данных для отправки сообщения.
            _sendEvent.SetBuffer(0, packet.Length + SIZE_LENGTH);

            //Выполняет передачу данных по подключенному сокету.
            //Если if вернет true, значит отправка сообщения прошла успешно.
            if (!_socket.SendAsync(_sendEvent))
                SendCompleted(_socket, _sendEvent);
        }

        public void Login(string login)
        {
            _login = login;
            _sendQueue.Enqueue(new Common.Network.Packets.ConnectionRequest(_login).GetBytes());

            //Если вернулся 1, значит поток занят.
            //В ином случае поток свободен и можно начинать отправку сообщения.
            if (Interlocked.CompareExchange(ref _sending, 1, 0) == 0)
                SendImpl();
        }

        //Отправка сообщения
        public void Send(string message)
        {
            //Добавление сообщения в конец потока
            _sendQueue.Enqueue(new Common.Network.Packets.MessageRequest(message).GetBytes());

            //Если вернулся 1, значит поток занят.
            //В ином случае поток свободен и можно начинать отправку сообщения.
            if (Interlocked.CompareExchange(ref _sending, 1, 0) == 0)
                SendImpl();
        }

        private void SendCompleted(object sender, SocketAsyncEventArgs e)
        {
            /* Отправка данных завершена. Отправляем следующую порцию данных */
            if (e.BytesTransferred != e.Count || e.SocketError != SocketError.Success)
            {
                Disconnect();
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, false));
                return;
            }

            SendImpl();
        }

        private void Receive()
        {
            if (_disposed == 1)
                return;

            if (!_socket.ReceiveAsync(_receiveEvent))
                ReceiveCompleted(_socket, _receiveEvent);
        }

        private void ReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            /* Получение данных завершено. Ожидаем следующую порцию данных */

            //Если с подключением что то пошло не так, отсоединяемся.
            if (e.BytesTransferred == 0 || e.SocketError != SocketError.Success)
            {
                Disconnect();
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, false));
                return;
            }

            int available = e.Offset + e.BytesTransferred;
            for (; ; )
            {
                if (available < SIZE_LENGTH)
                {
                    // WE NEED MORE DATA
                    break;
                }

                var offset = 0;
                var length = BufferPrimitives.GetUint16(e.Buffer, ref offset);
                if (length + SIZE_LENGTH > available)
                {
                    // WE NEED MORE DATA
                    break;
                }

                HandlePacket(BufferPrimitives.GetBytes(e.Buffer, ref offset, length));

                available = available - length - SIZE_LENGTH;
                if (available > 0)
                    Array.Copy(e.Buffer, length + SIZE_LENGTH, e.Buffer, 0, available);
            }

            e.SetBuffer(available, BUFFER_SIZE - available);
            Receive();
        }


        //Работа над байтовым пакетом
        private void HandlePacket(byte[] packet)
        {
            var packetId = (PacketId)BufferPrimitives.GetUint8(packet, 0);
            switch (packetId)
            {

                //Если это ответ на попытку присоедениться к серверу.
                case PacketId.ConnectionResponse:
                    var connectionResponse = new ConnectionResponse(packet);
                    if (connectionResponse.Result == ResultCodes.Failure)
                    {
                        _login = string.Empty;
                        //Если попытка соединения окончилось неудачей, занести в БД причину.
                        MessageReceived?.Invoke(this, new MessageReceivedEventArgs(_login, connectionResponse.Reason));
                    }
                    ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_login, true));
                    break;
                //Если это полученное сообщение 
                case PacketId.MessageBroadcast:
                    var messageBroadcast = new Common.Network.Packets.MessageBroadcast(packet);
                    //Зафиксировать факт получения сообщения.
                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(_login, messageBroadcast.Message));
                    break;
            }
        }

        private void Safe(Action callback)
        {
            try
            {
                callback();
            }
            catch
            {
            }
        }
    }
}
