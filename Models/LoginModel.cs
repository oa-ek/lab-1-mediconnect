using System.ComponentModel.DataAnnotations;

namespace MediConnect.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не вказаний Sign In")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не вказаний Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
