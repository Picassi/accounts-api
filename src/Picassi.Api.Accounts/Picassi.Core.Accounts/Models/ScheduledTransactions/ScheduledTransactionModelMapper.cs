using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.DAL.Entities;
using Picassi.Core.Accounts.Services;

namespace Picassi.Core.Accounts.Models.ScheduledTransactions
{
    public class ScheduledTransactionModelMapper : IModelMapper<ScheduledTransactionModel, ScheduledTransaction>
    {
        public ScheduledTransaction CreateEntity(ScheduledTransactionModel model)
        {
            return new ScheduledTransaction
            {
                Description = model.Description,
                AccountId = model.AccountId,
                ToId = model.ToId,
                CategoryId = model.CategoryId,
                EventId = model.EventId,
                Amount = model.Amount,
                Date = model.Date,
                DaysBefore = model.DaysBefore,
                Recurrence = model.Recurrence,
                RecurrenceDayOfMonth = model.RecurrenceDayOfMonth,
                RecurrenceWeekOfMonth = model.RecurrenceWeekOfMonth,
                RecurrenceDayOfWeek = (DayOfWeek?)model.RecurrenceDayOfWeek
            };
        }

        public ScheduledTransactionModel Map(ScheduledTransaction model)
        {
            return new ScheduledTransactionModel
            {
                Id = model.Id,
                Description = model.Description,
                AccountId = model.Account.Id,
                AccountName = model.Account.Name,
                ToId = model.To?.Id,
                ToName = model.To?.Name,
                CategoryId = model.Category?.Id,
                CategoryName = model.Category?.Name,
                EventId = model.EventId,
                Amount = model.Amount,
                Date = model.Date,
                DaysBefore = model.DaysBefore,
                Recurrence = model.Recurrence,
                RecurrenceDayOfMonth = model.RecurrenceDayOfMonth,
                RecurrenceWeekOfMonth = model.RecurrenceWeekOfMonth,
                RecurrenceDayOfWeek = (int?)model.RecurrenceDayOfWeek
            };
        }

        public void Patch(ScheduledTransactionModel model, ScheduledTransaction entity)
        {
            entity.Description = model.Description;
            entity.AccountId = model.AccountId;
            entity.ToId = model.ToId;
            entity.CategoryId = model.CategoryId;
            entity.EventId = model.EventId;
            entity.Amount = model.Amount;
            entity.Date = model.Date;
            entity.DaysBefore = model.DaysBefore;
            entity.Recurrence = model.Recurrence;
            entity.RecurrenceDayOfMonth = model.RecurrenceDayOfMonth;
            entity.RecurrenceWeekOfMonth = model.RecurrenceWeekOfMonth;
            entity.RecurrenceDayOfWeek = (DayOfWeek?) model.RecurrenceDayOfWeek;
        }

        public IEnumerable<ScheduledTransactionModel> MapList(IEnumerable<ScheduledTransaction> results)
        {
            return results.Select(Map);
        }
    }
}