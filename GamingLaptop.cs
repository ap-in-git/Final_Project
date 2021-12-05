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
        public override string SpecialWorkDone()
        {
            return "Check software installed and RAM usage";
            
        }


    }
}
