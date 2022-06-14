using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Новая_попытка.Core
{
     class ObservableObject : INotifyPropertyChanged
    {
        //Когда объект класса изменяет значение свойства,
        //то он через событие PropertyChanged извещает систему об изменении свойства.
        //А система обновляет все привязанные объекты.

        //PropertyChangedEventHandler Представляет метод,
        //который обрабатывает событие PropertyChanged,
        //возникающее при изменении свойства компонента.

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
