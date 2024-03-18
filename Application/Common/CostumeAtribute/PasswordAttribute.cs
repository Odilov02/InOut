using System.ComponentModel.DataAnnotations;

namespace Application.Common.CostumeAtribute;
#nullable disable
public class PasswordAttribute : ValidationAttribute
{
    public PasswordAttribute()
    {
        ErrorMessage = "Парол камида 8 белгидан иборат болиши керак ва камида 1 харф ва 1 рақамдан иборат болиши керак!";
    }

    public override bool IsValid(object value)
    {
        string password = value as string;
        if (string.IsNullOrEmpty(password))
            return false;

        if (password.Length < 8)
            return false;

        if (!password.Any(char.IsLetter) || !password.Any(char.IsDigit))
            return false;

        return true;
    }
}