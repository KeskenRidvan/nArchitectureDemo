using Core.Security.Entities;

namespace Domain.Entities;

public class OtpAuthenticator : OtpAuthenticator<int>
{
    public virtual User User { get; set; } = default!;
}
