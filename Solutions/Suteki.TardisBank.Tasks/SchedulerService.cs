namespace Suteki.TardisBank.Tasks
{
    using System;
    using System.Linq;

    using Raven.Client;

    using Suteki.TardisBank.Domain;

    public interface ISchedulerService
    {
        void ExecuteUpdates(DateTime now);
    }

    public class SchedulerService : ISchedulerService
    {
        private readonly IDocumentSession session;

        public SchedulerService(IDocumentSession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Gets all outstanding scheduled updates and performs the update.
        /// </summary>
        public void ExecuteUpdates(DateTime now)
        {
            var today = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);

            var results = session.Advanced
                            .LuceneQuery<Child>("Child/ByPendingSchedule")
                            .WhereLessThanOrEqual("NextRun", now)
                            .WaitForNonStaleResults().ToList();

            foreach (var child in results.ToList())
            {
                child.Account.TriggerScheduledPayments(now);
            }
        }
    }
}