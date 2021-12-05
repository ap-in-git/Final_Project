using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Final_Project
{
   
    public class GamingLaptop : Laptop
    {
        public override List<string> BrandModelList()
        {
            List<string> gamingLaptopModels = new() { "Lenovo", "AlienWare", "MSI"};
            return gamingLaptopModels;
            
        }


    }
}
