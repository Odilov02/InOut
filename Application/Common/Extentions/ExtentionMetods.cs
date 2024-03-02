using System.Security.Cryptography;
using System.Text;

namespace Application.Common.Extentions;

public static class ExtentionMetods
{
    public static string stringHash(this string rowDate)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rowDate));
            StringBuilder newDate = new StringBuilder();
            foreach (var item in bytes)
            {
                newDate.Append(item.ToString("x2"));
            }
            return newDate.ToString();
        }
    }
}
