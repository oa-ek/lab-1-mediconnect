namespace Shared.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Appointment> Appointmens { get; set; }
    }
}
