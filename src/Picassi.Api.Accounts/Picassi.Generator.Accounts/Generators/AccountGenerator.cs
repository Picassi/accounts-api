using System;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.DAL.Services;
using Picassi.Core.Accounts.Models.Accounts;

namespace Picassi.Generator.Accounts.Generators
{
    public class AccountGenerator : IModelDataGenerator
    {
        public Type Type => typeof(Account);

        private readonly string[] _accounts = {
            "Main", "Savings"
        };

        private readonly IAccountDataService _dataService;

        public AccountGenerator(IAccountDataService dataService)
        {
            _dataService = dataService;
        }

        public void Generate(DataGenerationContext context)
        {
            foreach (var account in _accounts)
            {
                _dataService.Create(new AccountModel
                {
                    Name = account
                });
            }
        }
    }
}
