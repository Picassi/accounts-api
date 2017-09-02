namespace Picassi.Api.Accounts.Client.Auth
{
    public interface IClientCredentialsProvider
    {
        string TokenEndpoint { get; }
        string Clientid { get; }
        string ClientSecret { get; }
        string RequestedScopes { get; }
    }
}
