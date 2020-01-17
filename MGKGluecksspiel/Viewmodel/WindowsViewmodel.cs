using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGKGluecksspiel.Viewmodel
{
    class WindowsViewmodel
    {
        private static Random random = new Random();
        public ObservableCollection<InputViewmodel> Inputs { get; set; }
        public ObservableCollection<OutputViewmodel> Outputs { get; set; }

        public WindowsViewmodel()
        {
            Inputs = new ObservableCollection<InputViewmodel>();

            //for (int i = 0; i < 400; i++)
            //{
            //    Inputs.Add(new InputViewmodel(GetRandomString(5), GetRandomNumber()));
            //}

            Outputs = new ObservableCollection<OutputViewmodel>();
        }

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public double GetRandomNumber()
        {
            return random.Next(10000);
        }
    }
}