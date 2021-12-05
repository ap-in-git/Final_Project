using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class NoteBook : Laptop
    {


        public override List<string> BrandModelList()
        {
            List<string> noteBookModels = new() { "Dell", "Apple", "Acer" };
            return noteBookModels;

        }
    }
}
