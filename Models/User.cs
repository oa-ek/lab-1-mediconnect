using System;
using System.Collections.Generic;

namespace MediConnect.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Avatar{ get; set; }
        public string Login { get; set; }
        public string Password{ get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime BitrhDate { get; set; }
        public string Phone { get; set; }
        public int GenderID { get; set; }
        public Gender Gender { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Discussion> Discussions { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + SecondName + " " + LastName;
            }
        }
    }
}
