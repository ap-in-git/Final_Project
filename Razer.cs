using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class Razer : Laptop
    {


        public override List<string> brandModelsList()
        {
            List<string> razerModels = new() { "Razer Blade 15", "Razer Blade 16", "Razer Blade 16i" };
            return razerModels;

        }
    }
}
