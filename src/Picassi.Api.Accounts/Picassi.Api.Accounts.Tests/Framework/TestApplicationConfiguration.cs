namespace Picassi.Api.Accounts.Tests.Framework
{
    public class TestApplicationConfiguration
    {
        public string ClientUrl { get; set; }
        public string UnencodedSecret { get; set; }
        public TestApplicationAuthConfiguration AuthConfig { get; set; }
    }
}