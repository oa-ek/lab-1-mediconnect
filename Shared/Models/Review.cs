namespace Shared.Models
{
    public class Review
    {
        public int Id { get; set; } 
        public int ClientId { get; set; }
        public User Client { get; set; }
        public int DoctorId { get; set; }
        public User Doctor { get; set; }
        public int Rate { get; set; }
        public DateTime ReviewDate { get; set; }
        public String Description { get; set; }
        
    }
}
