using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Models;
using Picassi.Core.Accounts.Models.ModelledTransactions;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.DAL.Services
{
    public interface IModelledTransactionsDataService : IGenericDataService<ModelledTransactionModel>
    {
        ResultsViewModel<ModelledTransactionModel> Query(int accountId, ModelledTransactionQueryModel query);
    }

    public class ModelledTransactionsDataService : GenericDataService<ModelledTransactionModel, ModelledTransaction>, IModelledTransactionsDataService
    {
        private readonly IModelMapper<ModelledTransactionModel, ModelledTransaction> _modelMapper;
        public ModelledTransactionsDataService(IModelMapper<ModelledTransactionModel, ModelledTransaction> modelMapper, IAccountsDataContext dbContext) 
            : base(modelMapper, dbContext)
        {
            _modelMapper = modelMapper;
        }

        public ResultsViewModel<ModelledTransactionModel> Query(int accountId, ModelledTransactionQueryModel query)
        {
            var pageSize = query.PageSize ?? 20;
            var skip = ((query.PageNumber ?? 1) - 1) * pageSize;
            var queryResults = DbContext.ModelledTransactions.Where(t => t.AccountId == accountId).Include("Category").AsQueryable();
            var allResults = queryResults.Count();
            var actualResults = queryResults.Skip(skip).Take(pageSize);

            return new ResultsViewModel<ModelledTransactionModel>
            {
                TotalLines = allResults,
                Lines = actualResults.Select(_modelMapper.Map).ToList()
            };
        }

    }
}
