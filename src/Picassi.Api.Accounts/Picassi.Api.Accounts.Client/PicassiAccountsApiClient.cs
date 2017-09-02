using Picassi.Api.Accounts.Client.Auth;

namespace Picassi.Api.Accounts.Client
{
    public interface IPicassiAccountsApiClient
    {
        IEventsApiClient Events { get; }
        ITransactionsApiClient Transactions { get; }
    }

    public class PicassiAccountsApiClient : IPicassiAccountsApiClient
    {        
        public IEventsApiClient Events { get; }
        public ITransactionsApiClient Transactions { get; }

        public PicassiAccountsApiClient(ApiClient apiClient)
        {
            Events = new EventsApiClient(apiClient);
            Transactions = new TransactionsApiClient(apiClient);
        }
    }
}
