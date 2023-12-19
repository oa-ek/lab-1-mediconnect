namespace Shared.Models
{
    public class Appointment
    {
        public int Id { get; set; } 
        public int ClientId { get; set; }
        public User Client { get; set; }
        public int DoctorId { get; set; }
        public User Doctor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Diagnosis> Diagnosises { get; set; }
        public ICollection<Discussion> Discussions { get; set; }
    }
}
