using System.Collections;
using System.Collections.Generic;

namespace MediConnect.Models
{
    public class Result
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Appointment> Appointmens { get; set; }
    }
}
