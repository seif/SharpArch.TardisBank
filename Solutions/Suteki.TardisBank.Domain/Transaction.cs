using System;

namespace Suteki.TardisBank.Model
{
    using SharpArch.Domain.DomainModel;

    public class Transaction : Entity
    {
        public Transaction(string description, decimal amount)
        {
            Description = description;
            Amount = amount;
            Date = DateTime.Now.Date;
        }

        protected Transaction()
        {
        }

        public virtual string Description { get; protected set; }
        public virtual decimal Amount { get; protected set; }
        public virtual DateTime Date { get; protected set; }
    }
}