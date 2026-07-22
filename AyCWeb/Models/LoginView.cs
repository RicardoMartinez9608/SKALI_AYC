

using System.ComponentModel.DataAnnotations;

namespace AyCWeb.Models
{
    public class LoginView
    {
        [Required]
        public string Usuario { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
