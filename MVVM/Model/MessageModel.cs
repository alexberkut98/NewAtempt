using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Новая_попытка.MVVM.Model
{
    class MessageModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _vis;
        public string UserName { get; set; }
        public string UserNameColor { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }
        public string Vis
        {
            get
            {
                return _vis;
            }
            set
            {
                _vis = value;
                OnPropertyChanged();
            }
        }
        public DateTime Time { get; set; }
        public bool IsNativeOrigin { get; set; }
        public bool FirstMessage { get; set; }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
