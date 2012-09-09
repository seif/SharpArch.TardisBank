namespace Suteki.TardisBank.Domain
{
    using System;

    using SharpArch.Domain.DomainModel;

    public class Transaction
    {
        public Transaction(string description, decimal amount)
        {
            this.Description = description;
            this.Amount = amount;
            this.Date = DateTime.Now.Date;
        }

        protected Transaction()
        {
        }

        public virtual string Description { get; protected set; }
        public virtual decimal Amount { get; protected set; }

        public virtual DateTime Date { get; protected set; }
    }
}