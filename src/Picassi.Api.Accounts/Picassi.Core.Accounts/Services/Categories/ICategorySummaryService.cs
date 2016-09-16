using Picassi.Core.Accounts.Enums;
using Picassi.Core.Accounts.ViewModels.Categories;

namespace Picassi.Core.Accounts.Services.Categories
{
    public interface ICategorySummaryService
    {
        CategorySummaryReportType Type { get; }

        CategorySummaryViewModel GetResults(CategorySummaryQueryModel query);
    }
}
