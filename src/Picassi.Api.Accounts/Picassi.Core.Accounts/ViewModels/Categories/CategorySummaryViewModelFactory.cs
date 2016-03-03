using System;
using System.Collections.Generic;
using System.Linq;
using Picassi.Common.Data.Enums;
using Picassi.Data.Accounts.Database;
using Picassi.Data.Accounts.Models;

namespace Picassi.Core.Accounts.ViewModels.Categories
{
    public interface ICategorySummaryViewModelFactory
    {
        CategorySummaryViewModel GetSummary(CategoryViewModel category, Frequency period, int periodsToSummarise);
    }

    public class CategorySummaryViewModelFactory : ICategorySummaryViewModelFactory
    {
        private readonly IAccountsDataContext _dataContext;

        public CategorySummaryViewModelFactory(IAccountsDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public CategorySummaryViewModel GetSummary(CategoryViewModel category, Frequency period, int periodsToSummarise)
        {
            var startDate = GetStartDate(period, periodsToSummarise);
            var transactions = category == null
                ? _dataContext.Transactions.Where(x => x.CategoryId == null && x.Date > startDate)
                : _dataContext.Transactions.Where(x => x.CategoryId == category.Id && x.Date > startDate);
                
            var report = GetReport(startDate, transactions.ToList(), period, periodsToSummarise);

            return new CategorySummaryViewModel
            {
                Id = category?.Id,
                Name = category?.Name ?? "Uncategorised",
                SpendingReport = report,
                AverageSpend = report.Where(x => x.Value != null).ToList().Any() ?
                    report.Where(x => x.Value != null).Average(x => (decimal)x.Value) : 0
            };
        }

        private Dictionary<int, decimal?> GetReport(DateTime startDate, List<Transaction> transactions, Frequency period, int periodsToSummarise)
        {
            var reportDictionary = Enumerable.Range(1, periodsToSummarise).ToDictionary(x => x, x => (decimal?)null);
            foreach (var group in transactions.GroupBy(x => GetPeriodRelatedTo(startDate, x.Date, period, periodsToSummarise)))
            {
                reportDictionary[periodsToSummarise - group.Key] = group.Sum(x => x.Amount);
            }
            return reportDictionary;
        }

        private int GetPeriodRelatedTo(DateTime startDate, DateTime date, Frequency period, int periodsToSummarise)
        {
            switch (period)
            {
                case Frequency.Daily:
                    return (int)(date - startDate).TotalDays;
                case Frequency.Weekly:
                    return (int)((date - startDate).TotalDays / 7);
                case Frequency.Monthly:
                    return date.Month;
                case Frequency.Annually:
                    return date.Year;
                default:
                    return date.Month;
            }
        }

        private DateTime GetStartDate(Frequency period, int periodsToSummarise)
        {
            var currentDate = DateTime.Now;

            switch (period)
            {
                case Frequency.Daily:
                    return currentDate.AddDays(-periodsToSummarise);
                case Frequency.Weekly:
                    return currentDate.AddDays(-periodsToSummarise * 7);
                case Frequency.Monthly:
                    return currentDate.AddMonths(-periodsToSummarise);
                default:
                    return currentDate.AddYears(-periodsToSummarise);
            }
        }
    }
}
