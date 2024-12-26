using Data.CustomDataAnotaion;
using System.ComponentModel.DataAnnotations;

namespace Data.DTO.User
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress]
       
        public string email { get; set; }
        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string password { get; set; }

      
       
    }
}
