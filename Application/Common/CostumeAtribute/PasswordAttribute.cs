using System.ComponentModel.DataAnnotations;

namespace Application.Common.CostumeAtribute;
#nullable disable
public class PasswordAttribute : ValidationAttribute
{
    public PasswordAttribute()
    {
        ErrorMessage = "Parol kamida 8 belgidan iborat bo'lishi kerak va kamida bir harf va bir raqamdan iborat bo'lishi kerak.";
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