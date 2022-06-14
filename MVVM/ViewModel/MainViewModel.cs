using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Новая_попытка.Core;
using Новая_попытка.MVVM.Model;

namespace Новая_попытка.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private ITransport _currentTransport;
        private TCPClient _tcPClient;
        public RelayCommand Mini1 { get; set; }
        public RelayCommand ChangeStation { get; set; }
        public RelayCommand Closing { get; set; }

        public RelayCommand StartServer { get; set; }
        public RelayCommand StopServer { get; set; }

        public RelayCommand ChooseMode1 { get; set; }
        public RelayCommand ChooseMode2 { get; set; }
        public RelayCommand SetLogin { get; set; }
        public RelayCommand SendCommand { get; set; }

        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }
        public string Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        public string Adress
        {
            get
            {
                return _adress;
            }
            set
            {
                _adress = value;
                OnPropertyChanged();
            }
        }
        int ChosenMode = 1;
        private string _port;
        private string _adress;
        private string _login;
        private string _message;
        private ContactModel _selectedContact;

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            Mini1 = new RelayCommand(o => Minimise());
            ChangeStation = new RelayCommand(o => WindowStateClick());
            Closing = new RelayCommand(o => CloseWindow());

            StartServer = new RelayCommand(o => Starting());
            StopServer = new RelayCommand(o => Stopping());
            SetLogin = new RelayCommand(o => _tcPClient.Login(Login));

            ChooseMode1 = new RelayCommand(o => Choose1());
            ChooseMode2 = new RelayCommand(o => Choose2());
            SendCommand = new RelayCommand(o => Sending());
        }
        public ContactModel SelectedContact
        {
            get
            {
                return _selectedContact;
            }
            set
            {
                _selectedContact = value;
                ChangeColor();
                OnPropertyChanged();
            }
        }
        //Меняем элемент, чей задний фон будет красным.
        //Эта функция должна применяться всякий раз,
        //как переменная SelectedContact сменит сове значение
        private void ChangeColor()
        {
            for (int i = 0; i < Contacts.Count; i++)
            {
                if (Contacts[i].UserName == SelectedContact.UserName)
                    Contacts[i].UserColor = "Red";
                else
                    Contacts[i].UserColor = "Coral";
            }

            for (int i = 0; i < Messages.Count; i++)
            {
                if (Messages[i].UserName == SelectedContact.UserName && Messages[i].Target == "You" || SelectedContact.UserName == "All" && Messages[i].Target == "All" || Messages[i].UserName == "You" && Messages[i].Target == SelectedContact.UserName)
                    Messages[i].Vis = "1";
                else
                    Messages[i].Vis = "0";
            }
            OnPropertyChanged();
        }

        public void Starting()
        {
            _tcPClient.Connect(Adress, Port);
        }
        private void Sending()
        {
            if (!string.IsNullOrEmpty(Message))
            {
                //Делаю факт отправки сообщения видимым на экране
                Messages.Add(new MessageModel
                {
                    UserName = "You",
                    Vis = "1",
                    Target = SelectedContact.UserName,
                    Time = DateTime.Now,
                    Message = Message
                });

                //Осуществляю отправку письма на сервер.
                _tcPClient.Send(Message);
            }
        }
        public void Stopping()
        {
            _tcPClient.Disconnect();
        }

        public void Minimise()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        public void CloseWindow()
        {
            Application.Current.Shutdown();
        }

        public void WindowStateClick()
        {
            //Если окно находится не в максимальном расширении - увеличить его.
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void Choose1()
        {
            ChosenMode = 1;
        }
        public void Choose2()
        {
            ChosenMode = 2;
        }
    }
}
