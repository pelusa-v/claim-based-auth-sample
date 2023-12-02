namespace claim_based_auth_sample.Application;

public class AuthResponseDTO
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}
