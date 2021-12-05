using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class Msi : Laptop
    {
  

        public override List<string> brandModelsList()
        {
            List<string> msiModels = new() { "MSI GE", "MSI GF", "MSI GF Thin" };
            return msiModels;
        }

    }
}
