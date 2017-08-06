using Picassi.Core.Accounts.Models.Budgets;
using Picassi.Core.Accounts.Models.Categories;

namespace Picassi.Core.Accounts.Models
{
    public class CategoryWithBudget
    {
        public CategoryModel Category { get; set; }

        public BudgetModel Budget { get; set; }
    }
}
