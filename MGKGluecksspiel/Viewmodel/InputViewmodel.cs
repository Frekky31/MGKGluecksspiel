using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGKGluecksspiel.Viewmodel
{
    class InputViewmodel : INotifyPropertyChanged
    {
        public InputViewmodel()
        {
        }

        public InputViewmodel(string Name, double Number)
        {
            m_Name = Name;
            m_Number = Number;
        }

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set {
                m_Name = value;
                NotifyPropertyChanged("Name");
            }
        }
        private double m_Number;
        public double Number
        {
            get { return m_Number; }
            set
            {
                m_Number = value;
                NotifyPropertyChanged("Number");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string Obj)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(Obj));
            }
        }
    }
}
