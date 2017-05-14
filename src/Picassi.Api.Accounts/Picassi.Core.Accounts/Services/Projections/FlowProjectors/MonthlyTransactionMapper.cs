using System.Collections.Generic;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Services.Projections.FlowProjectors
{
    public interface IMonthlyTransactionMapper
    {
        MonthlyTransactionViewModel Map(ScheduledTransaction model);
    }

    public class MonthlyTransactionMapper : IMonthlyTransactionMapper
    {
        public MonthlyTransactionViewModel Map(ScheduledTransaction model)
        {
            return new MonthlyTransactionViewModel
            {
                Amount = model.Amount,
                CategoryId = model.CategoryId,
                DayOfTheMonth = model.RecurrenceDayOfMonth ?? 1,
                Description = model.Description,
                Start = model.Start,
                End = model.End,
                AccountId = model.AccountId,
                TargetAccountId = model.ToId,
                Id = model.Id
            };
        }
    }
}
