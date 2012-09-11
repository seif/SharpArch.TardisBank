namespace Suteki.TardisBank.Infrastructure
{
    using System.Linq;

    using Raven.Client.Indexes;

    using Suteki.TardisBank.Domain;

    public class Child_ByPendingSchedule : AbstractIndexCreationTask<Child>
    {
        public Child_ByPendingSchedule()
        {
            this.Map = children => from child in children
                              from schedule in child.Account.PaymentSchedules
                              select new { schedule.NextRun };
        }
    }
}