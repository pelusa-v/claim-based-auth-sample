using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace claim_based_auth_sample.Application;

public class UserCredentialsDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }
}
