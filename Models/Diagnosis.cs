using System;
using System.Collections.Generic;

namespace MediConnect.Models
{
    public class Diagnosis
    {
        public int ID { get; set; }
        public int ResultID { get; set; }
        public Result Result { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int AppointmentID { get; set; }
        public Appointment Appointment { get; set; }
    }
}
