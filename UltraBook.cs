using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class UltraBook : Laptop
    {
  

        public override List<string> BrandModelList()
        {
            List<string> ultraBookModels = new() { "LG", "Surface", "Samsung" };
            return ultraBookModels;
        }

    }
}
