using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGKGluecksspiel.Serializable
{
    [Serializable]
    class Input
    {
        public string Name { get; set; }
        public double Number { get; set; }

        public Input(string Name, double Number) {
            this.Name = Name;
            this.Number = Number;
        }
    }
}
