using System.ComponentModel.DataAnnotations;

namespace Application.Common.CostumeAtribute;
#nullable disable
[AttributeUsage(AttributeTargets.Property)]
public class NotZeroAttribute: ValidationAttribute
{
    public NotZeroAttribute(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
    public override bool IsValid(object value)
    {

        if (value is int intValue)
        {
            return intValue >= 0;
        }
        return false;

    }
}
