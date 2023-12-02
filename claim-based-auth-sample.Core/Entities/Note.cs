using Microsoft.AspNetCore.Identity;

namespace claim_based_auth_sample.Core;

public class Note
{
    public int NoteId { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string OwnerId { get; set; }
    public IdentityUser Owner { get; set; }
}
