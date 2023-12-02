using System.ComponentModel.DataAnnotations;

namespace claim_based_auth_sample.Application;

public class AuthRequestDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Content { get; set; }
}
