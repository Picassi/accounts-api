﻿using System;

namespace Picassi.Core.Accounts.ViewModels.Categories
{
    public class CategoriesQueryModel
    {
        public string Name { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string PageSize { get; set; }
        public string PageNumber { get; set; }
    }
}
