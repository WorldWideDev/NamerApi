using System.ComponentModel.DataAnnotations;
namespace NamesApi.Models.Validators
{
    public class NameEntryExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var repo = (INamesRepository)validationContext.GetService(typeof(INamesRepository));
            return repo.NameEntryExists((string)value)
                ? new ValidationResult("Name already exists.")
                : ValidationResult.Success;
        }
 
    }
}
