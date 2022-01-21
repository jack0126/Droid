using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Reflection;

namespace Jack.Mvvm
{
    public class MvvmSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Dictionary<string, object> MvvmProperties = new Dictionary<string, object>(16);
        
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        protected T GetValue<T>([CallerMemberName] string propertyName = null)
        {
            return MvvmProperties.ContainsKey(propertyName) ? (T)MvvmProperties[propertyName] : default(T);
        }

        protected void SetValue<T>(T value, [CallerMemberName] string propertyName = null)
        {
            if (MvvmProperties.ContainsKey(propertyName))
            {
                MvvmProperties.Remove(propertyName);
            }
            MvvmProperties[propertyName] = value;
            OnPropertyChanged(propertyName);
        }
        
    }
}
