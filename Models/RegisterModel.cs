using System;
using System.ComponentModel.DataAnnotations;

namespace MediConnect.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не вказане First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Не вказане First Name Second Name")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Не вказане Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Не вказан Phone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Не вказан Sign In")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не вказан Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password уведено не вірно")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Не вибрана Gender")]
        public int GenderID { get; set; }

        [Required(ErrorMessage = "Не вибрана Birth Date")]
        public DateTime BirthDate { get; set; }
    }
}
