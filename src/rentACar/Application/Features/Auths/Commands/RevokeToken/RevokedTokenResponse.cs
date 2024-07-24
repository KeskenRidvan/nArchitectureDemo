namespace Application.Features.Auths.Commands.RevokeToken;
public class RevokedTokenResponse
{
    public Guid Id { get; set; }
    public string Token { get; set; }

    public RevokedTokenResponse()
    {
        Token = string.Empty;
    }

    public RevokedTokenResponse(Guid id, string token)
    {
        Id = id;
        Token = token;
    }
}