using Application.Common.CostumeAtribute;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models;

public class UserCridential
{
    [MinLength(5)]
    public string UserName { get; set; }
    [Password]
    public string Password { get; set; }
}
