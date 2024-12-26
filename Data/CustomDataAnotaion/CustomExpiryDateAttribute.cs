using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.CustomDataAnotaion
{
    class CustomExpiryDateAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            // Ensure the expiry date is not in the past
            if (value is DateTime expiryDate)
            {
                return expiryDate > DateTime.UtcNow;
            }
            return false;
        }
    }
}
