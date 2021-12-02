using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class Appointment : Laptop
    {
        public string CustomerName { get; set; }
        public int AppointmentTime { get; set; }
        public string CreditCardNo { get; set; }

        public string WorkDone { get; set; }

        public string TechnicianName { get; set; }

        Laptop laptop { get; set; }


        public override void specialServices()
        {
            throw new NotImplementedException();
        }
    }
}
