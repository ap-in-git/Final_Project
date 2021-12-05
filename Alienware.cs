using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class Alienware : Laptop
    {


        public override List<string> brandModelsList()
        {
            List<string> alienModels = new() { "Alien 15", "Alien Fighter", "Legion X" };
            return alienModels;

        }
    }
}
