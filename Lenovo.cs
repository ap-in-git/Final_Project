using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Final_Project
{
   
    public class Lenovo : Laptop
    {
        public override List<string> brandModelsList()
        {
            List<string> lenovoModels = new() { "Legion 5i", "Legion 6i", "Legion 7i"};
            return lenovoModels;
            
        }


    }
}
