using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Picassi.Api.Accounts.Client.Auth;
using Picassi.Core.Accounts.Models.Transactions;

namespace Picassi.Api.Accounts.Client
{
    public interface ITransactionsApiClient
    {

        TransactionsResultsViewModel GetTransactions(TransactionsQueryModel query);
    }

    public class TransactionsApiClient : AbstractApiClient, ITransactionsApiClient
    {
        public TransactionsApiClient(ApiClient apiClient) : base(apiClient) { }

        public TransactionsResultsViewModel GetTransactions(TransactionsQueryModel query)
        {
            ValidateApiClient();

            var uri = $"transactions?{GetQueryString(query)}";

            return ApiClient.GetJson<TransactionsResultsViewModel>(uri);
        }

        private static string GetQueryString(object obj)
        {
            var result = new List<string>();
            var props = obj.GetType().GetProperties().Where(p => p.GetValue(obj, null) != null);
            foreach (var p in props)
            {
                var value = p.GetValue(obj, null);
                if (value is ICollection enumerable)
                {

                    result.AddRange(from object v in enumerable select
                        $"{p.Name}={HttpUtility.UrlEncode(GetStringRepresentation(v))}");
                }
                else
                {
                    result.Add($"{p.Name}={HttpUtility.UrlEncode(GetStringRepresentation(value))}");
                }
            }

            return string.Join("&", result.ToArray());
        }

        private static string GetStringRepresentation(object o)
        {
            if (o is DateTime)
            {
                return ((DateTime) o).ToString("yyyy-MM-dd");
            }
            return Convert.ToString(o);
        }
    }
}