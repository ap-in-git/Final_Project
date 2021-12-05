using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class NetBook : Laptop
    {


        public override List<string> BrandModelList()
        {
            List<string> netBookModels = new() { "Sony", "Gigabyte", "Asus" };
            return netBookModels;

        }
    }
}
