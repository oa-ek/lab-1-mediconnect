using System;
using System.Collections.Generic;

namespace Shared.Models
{
    public class Appointment
    {
        public int ID { get; set; } 
        public int ClientID { get; set; }
        public User Client { get; set; }
        public int DoctorID { get; set; }
        public User Doctor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Diagnosis> Diagnosises { get; set; }
        public ICollection<Discussion> Discussions { get; set; }
    }
}
