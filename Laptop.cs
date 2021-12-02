using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public abstract class Laptop : ILaptop
    {
        public string Brand { get; set; }
        public string Model { get; set; }

        public abstract void specialServices();
    }
}
