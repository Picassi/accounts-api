using System.Collections.Generic;
using System.Linq;
using Picassi.Core.Accounts.Enums;

namespace Picassi.Core.Accounts.Services.Categories
{
    public interface ICategorySummaryServiceProvider
    {
        ICategorySummaryService GetService(CategorySummaryReportType type);
    }

    public class CategorySummaryServiceProvider : ICategorySummaryServiceProvider
    {
        private readonly IEnumerable<ICategorySummaryService> _services;

        public CategorySummaryServiceProvider(IEnumerable<ICategorySummaryService> services)
        {
            _services = services;
        }

        public ICategorySummaryService GetService(CategorySummaryReportType type)
        {
            return _services.First(x => x.Type == type);
        }
    }
}
