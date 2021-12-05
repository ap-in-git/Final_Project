﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class Appointment
    {
        public string CustomerName { get; set; }
        public int AppointmentTime { get; set; }
        public decimal CreditCardNo { get; set; }

        public string WorkDone { get; set; }

        public string TechnicianName { get; set; }

        private Laptop laptop = null;

        public Laptop CustomerLaptop { get => laptop; set => laptop = value; }

    }
}
