namespace WorkingOnJWT.Options;

public class JwtOption
{
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public string signingKey { get; set; }
    public int expiresInSeconds { get; set; }
}