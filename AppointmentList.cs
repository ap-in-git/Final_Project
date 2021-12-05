using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Final_Project
{
   
    public class AppointmentList
    {
        private List<Appointment> appointmentList = null;

       
        public List<Appointment> Appointments { get => appointmentList; set => appointmentList = value; }
        public AppointmentList()
        {
            Appointments = new List<Appointment>();
        }

        public Appointment this[int i]
        {
            get { return Appointments[i]; }
        }

        public int Count()
        {
            return Appointments.Count();
        }

        public void Add(Appointment appointment)
        {
            Appointments.Add(appointment);
        }
        public void Clear()
        {
            Appointments.Clear();
        }
    }
}
