using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO.User
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        [Compare("NewPassword",ErrorMessage ="please confirmpasswrd not valid please enter the samw NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
