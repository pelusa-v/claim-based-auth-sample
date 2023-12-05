using Microsoft.AspNetCore.Identity;

namespace claim_based_auth_sample.Application;

public class NoteDTO
{
    public int NoteId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    // public IdentityUser Owner { get; set; }
}
