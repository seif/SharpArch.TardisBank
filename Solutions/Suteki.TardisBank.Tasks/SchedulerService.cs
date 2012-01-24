using System;
using System.Linq;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Services
{
    public interface ISchedulerService
    {
        void ExecuteUpdates(DateTime now);
    }

    public class SchedulerService : ISchedulerService
    {   
        public SchedulerService()
        {
        }

        /// <summary>
        /// Gets all outstanding scheduled updates and performs the update.
        /// </summary>
        public void ExecuteUpdates(DateTime now)
        {
            //TODO implement query Child/ByPendingSchedule .WhereLessThanOrEqual("NextRun", now)
        }
    }
}