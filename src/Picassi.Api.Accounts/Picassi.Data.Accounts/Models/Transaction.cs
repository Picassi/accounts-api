﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Picassi.Data.Accounts.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public string Description { get; set; }

        [ForeignKey("From")]
        public int? FromId { get; set; }

        [ForeignKey("To")]
        public int? ToId { get; set; }

        public int? CategoryId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }        

        public virtual Account From { get; set; }

        public virtual Account To { get; set; }

        public virtual Category Category { get; set; }

        public TransactionStatus Status { get; set; }
    }

    public enum TransactionStatus
    {
        Provisional = 0,
        Confirmed = 1
    }
}