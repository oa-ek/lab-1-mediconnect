using System;
using System.Collections.Generic;

namespace MediConnect.Models
{
    public class Review
    {
        public int ID { get; set; } 
        public int ClientID { get; set; }
        public User Client { get; set; }
        public int DoctorID { get; set; }
        public User Doctor { get; set; }
        public int Rate { get; set; }
        public DateTime ReviewDate { get; set; }
        public String Description { get; set; }
        
    }
}
