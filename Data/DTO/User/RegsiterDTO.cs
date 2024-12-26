using Microsoft.EntityFrameworkCore;
using Data.CustomDataAnotaion;
using System.ComponentModel.DataAnnotations;

namespace Data.DTO.User
{
    public class RegsiterDTO
    {
        [Required(ErrorMessage ="Email Required")]
        [EmailAddress]
        [unique]
        public string email { get; set; }
        [Required(ErrorMessage = "Username Required")]
        
        public string username { get; set; }
        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required(ErrorMessage = " confirmPassword Required")]
        [DataType(DataType.Password)]
        [Compare("password",ErrorMessage ="confirmpassword not match with password")]
        public string confirmpassword { get;set; }
        [Required(ErrorMessage = "Address Required")]
        public string Address { get;set; }


    }

}
