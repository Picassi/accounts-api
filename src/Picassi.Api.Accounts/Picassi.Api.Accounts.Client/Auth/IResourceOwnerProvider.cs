namespace Picassi.Api.Accounts.Client.Auth
{
    public interface IResourceOwnerProvider
    {
        string TokenEndpoint { get; }
        string Clientid { get; }
        string ClientSecret { get; }
        string RequestedScopes { get; }
        string Username { get; }
        string Password { get; }
    }
}
