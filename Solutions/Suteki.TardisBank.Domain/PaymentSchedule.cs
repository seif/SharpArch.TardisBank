namespace Suteki.TardisBank.Domain
{
    using System;

    using SharpArch.Domain.DomainModel;

    public class PaymentSchedule : EntityWithTypedId<int>
    {
        public PaymentSchedule(int id,DateTime nextRun, Interval interval, decimal amount, string description)
        {
            this.Id = id;
            this.NextRun = nextRun;
            this.Interval = interval;
            this.Amount = amount;
            this.Description = description;
        }

        protected PaymentSchedule()
        {
        }

        public virtual DateTime NextRun { get; protected set; }
        public virtual Interval Interval { get; protected set; }
        public virtual decimal Amount { get; protected set; }
        public virtual string Description { get; protected set; }

        public virtual string AccountId { get; protected set; }

        public virtual void CalculateNextRunDate()
        {
            switch (this.Interval)
            {
                case Interval.Day:
                    this.NextRun = this.NextRun.AddDays(1);
                    break;
                case Interval.Week:
                    this.NextRun = this.NextRun.AddDays(7);
                    break;
                case Interval.Month:
                    this.NextRun = this.NextRun.AddMonths(1);
                    break;
            }
        }
    }
}