using System;

namespace Picassi.Api.Accounts.Contract
{
    public class SpendingByCategoryRequest
    {
        public int[] Accounts { get; set; }
        public int[] Categories { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}