using Picassi.Api.Accounts.Contract.Enums;

namespace Picassi.Core.Accounts.Models.Categories
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        public bool HasChildren { get; set; }

        public CategoryType CategoryType { get; set; }
    }
}
