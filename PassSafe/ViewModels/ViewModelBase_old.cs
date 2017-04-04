using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Windows;
using System.ComponentModel;

namespace PassSafe.ViewModels
{
    public abstract class ViewModelBase_old : INotifyPropertyChanged
    {
        internal void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        bool? _CloseWindowFlag;
        public bool? CloseWindowFlag
        {
            get { return _CloseWindowFlag; }
            set
            {
                _CloseWindowFlag = value;
                RaisePropertyChanged("CloseWindowFlag");
            }
        }

        public virtual void CloseWindow(bool? result = true)
        {
            Application.Current.Dispatcher.BeginInvoke
                (DispatcherPriority.Background, new Action(() =>
                {
                    CloseWindowFlag = CloseWindowFlag == null
                    ? true
                    : !CloseWindowFlag;
                }));
        }
    }
}
