using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Final_Project
{
    [XmlInclude(typeof(Lenovo))]
    [XmlInclude(typeof(Msi))]
    [XmlInclude(typeof(Razer))]
    [XmlInclude(typeof(Alienware))]
    public abstract class Laptop : ILaptop
    {
        public string Brand { get; set; }
        public string Model { get; set; }


        public abstract List<string> brandModelsList();
    }
}
