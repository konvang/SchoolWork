using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderEntrySystem
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        protected ViewModel(string displayName)
        {
            this.DisplayName = displayName;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual string DisplayName { get; private set; }

        public override string ToString()
        {
            return this.DisplayName;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            // If there is a listener...
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}