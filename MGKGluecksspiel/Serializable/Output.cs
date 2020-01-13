using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGKGluecksspiel.Serializable
{
    [Serializable]
    class Output
    {
        public int Place { get; set; }
        public string Name { get; set; }
        public double Number { get; set; }
        public double Difference { get; set; }

        public Output(int Place, string Name, double Number, double Difference) {
            this.Place = Place;
            this.Name = Name;
            this.Number = Number;
            this.Difference = Difference;
        }
    }
}
