using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGKGluecksspiel.Viewmodel
{
    class OutputViewmodel : INotifyPropertyChanged
    {
        public OutputViewmodel()
        {
        }

        public OutputViewmodel(string Name, double Number)
        {
            m_Name = Name;
            m_Number = Number;
        }

        public OutputViewmodel(int Place, string Name, double Number, double Difference)
        {
            m_Place = Place;
            m_Name = Name;
            m_Number = Number;
            m_Difference = Difference;
        }

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set
            {
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

        private double m_Difference;
        public double Difference
        {
            get { return m_Difference; }
            set
            {
                m_Difference = value;
                NotifyPropertyChanged("Difference");
            }
        }

        private int m_Place;
        public int Place
        {
            get { return m_Place; }
            set
            {
                m_Place = value;
                NotifyPropertyChanged("Place");
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
