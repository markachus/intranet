using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace IntranetCore.Api.ValidationAttributes
{
    public class MustBeHexColorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string sHexColorCandidate = value as String;

            if (!string.IsNullOrWhiteSpace(sHexColorCandidate))
            {
                if (!sHexColorCandidate.StartsWith("#"))
                {
                    return new ValidationResult("An hexadecimal color must start with #");
                }
            }

            return ValidationResult.Success;
        }
    }
}
