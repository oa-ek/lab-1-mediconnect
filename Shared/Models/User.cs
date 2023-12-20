using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        [NotMapped] public IFormFile AvatarFile { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int ProffesionId { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Discussion> Discussions { get; set; }

        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + LastName; }
        }
    }
}