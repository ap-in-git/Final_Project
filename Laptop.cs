using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Final_Project
{
    [XmlInclude(typeof(GamingLaptop))]
    [XmlInclude(typeof(UltraBook))]
    [XmlInclude(typeof(NoteBook))]
    [XmlInclude(typeof(NetBook))]
    public abstract class Laptop : ILaptop
    {
        public string LaptopType { get; set; }
        public string Model { get; set; }

        public string Brand { get; set; }

        public abstract string SpecialWorkDone();
    }
}
