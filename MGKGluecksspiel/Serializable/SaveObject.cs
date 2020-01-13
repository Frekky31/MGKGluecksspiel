using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGKGluecksspiel.Serializable
{
    [Serializable]
    class SaveObject
    {
        public List<Input> Inputs;
        public List<Output> Outputs;
        public int Only { get; set; }
        public double GuessNumber { get; set; }

        public SaveObject() {
            Inputs = new List<Input>();
            Outputs = new List<Output>();
        }
    }
}
