using System;
using System.Collections.Generic;
using Picassi.Core.Accounts.DAL.Entities;

namespace Picassi.Core.Accounts.Services.Projections.FlowProjectors
{
    public interface IWeeklyTransactionMapper
    {
        WeeklyTransactionViewModel Map(ScheduledTransaction model);
    }

    public class WeeklyTransactionMapper : IWeeklyTransactionMapper
    {
        public WeeklyTransactionViewModel Map(ScheduledTransaction model)
        {
            return new WeeklyTransactionViewModel
            {
                Amount = model.Amount,
                CategoryId = model.CategoryId,
                DayOfTheWeek = model.RecurrenceDayOfWeek ?? DayOfWeek.Monday,
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
