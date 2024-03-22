using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.CostumeAtribute;

internal class NotNullGuIdAttribute: ValidationAttribute
{
    public NotNullGuIdAttribute(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
    public override bool IsValid(object value)
    {

        if (value != null)
        {
            if (value is Guid)
            {
                Guid guidValue = (Guid)value;
                if (guidValue == Guid.Empty)
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }
}

