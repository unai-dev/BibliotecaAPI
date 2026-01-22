using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Validations
{
    public class FirstUpperLetterAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var valueString = value.ToString()!;
            var firstChar = valueString[0].ToString();

            if (firstChar != firstChar.ToUpper())
            {
                return new ValidationResult("The first letter must be capital");
            }

            return ValidationResult.Success;
        }

    }
}
