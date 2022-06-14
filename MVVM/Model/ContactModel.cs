using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Новая_попытка.MVVM.Model
{
    class ContactModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _color;
        public string UserName { get; set; }
        public string UID { get; set; }
        public string UserColor
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<MessageModel> Messages { get; set; }
        public string LastMessage = " ";
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
