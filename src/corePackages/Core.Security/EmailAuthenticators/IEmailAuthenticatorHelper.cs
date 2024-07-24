namespace Core.Security.EmailAuthenticators;

public interface IEmailAuthenticatorHelper
{
    public Task<string> CreateEmailActivationKey();
    public Task<string> CreateEmailActivationCode();
}