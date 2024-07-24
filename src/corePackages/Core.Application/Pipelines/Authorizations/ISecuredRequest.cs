namespace Core.Application.Pipelines.Authorizations;
public interface ISecuredRequest
{
    public string[] Roles { get; }
}