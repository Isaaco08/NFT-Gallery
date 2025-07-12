using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoNFTs.Application.CustomValidation;

public class ValidValueNegative : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
        {
            if (int.TryParse(value.ToString(), out int numero))
            {
                if (numero < 0)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
        }
        return ValidationResult.Success;
    }
}
