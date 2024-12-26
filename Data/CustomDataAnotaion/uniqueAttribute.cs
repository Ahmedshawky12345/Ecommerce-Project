using Data.Data;
using System.ComponentModel.DataAnnotations;

namespace Data.CustomDataAnotaion
{
    public class uniqueAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)

        {
            var new_value = value.ToString();
            if (new_value != null)
            {
                var context = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
                var exist = context.Users.Any(x => x.Email == new_value);
                if (exist)
                {
                    return new ValidationResult("the Email must be unique");
                }



            } 
            return ValidationResult.Success;


        }
    }
}
