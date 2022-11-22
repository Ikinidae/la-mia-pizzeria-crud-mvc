using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Validator
{
    public class FiveWordsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string fieldValue = (string)value;

            if (fieldValue == null)
            {
                return new ValidationResult("Il campo deve contenere almeno 5 parole");
            }

            string[] arrSpaces = fieldValue.Split(' ');
            
            if (arrSpaces.Length < 5)
            {
                return new ValidationResult("Il campo deve contenere almeno 5 parole");
            }

            return ValidationResult.Success;
        }
    }
}