using ChatServer._EventArgs_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Новая_попытка
{
    public interface ITransport
    {
        #region Events

        event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        #endregion Events

        #region Methods

        void Connect(string address, string port);

        void Disconnect();

        void Login(string login);

        void Send(string message);

        #endregion Methods
    }
}
