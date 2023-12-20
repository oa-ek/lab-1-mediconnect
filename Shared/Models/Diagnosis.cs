namespace Shared.Models
{
    public class Diagnosis
    {
        public int Id { get; set; }
        public int ResultId { get; set; }
        public Result Result { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }
}
