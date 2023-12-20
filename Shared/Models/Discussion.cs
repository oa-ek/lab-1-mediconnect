namespace Shared.Models
{
    public class Discussion
    {
        public int Id { get; set; } 
        public int AppointmentId { get; set; }
        public Appointment Appointment{ get; set; }
        public int DoctorId { get; set; }
        public User Doctor { get; set; }
        public int Rate { get; set; }
        public DateTime MessageDate { get; set; }
        public String Message { get; set; }
        
    }
}
