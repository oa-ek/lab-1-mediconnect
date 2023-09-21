using System;
using System.Collections.Generic;

namespace MediConnect.Models
{
    public class Discussion
    {
        public int ID { get; set; } 
        public int AppointmentID { get; set; }
        public Appointment Appointment{ get; set; }
        public int DoctorID { get; set; }
        public User Doctor { get; set; }
        public int Rate { get; set; }
        public DateTime MessageDate { get; set; }
        public String Message { get; set; }
        
    }
}
