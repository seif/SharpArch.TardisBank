namespace Suteki.TardisBank.Infrastructure
{
    using System.Linq;

    using Raven.Client.Indexes;

    using Suteki.TardisBank.Domain;

    public class All_Users : AbstractMultiMapIndexCreationTask
    {
        public All_Users()
        {
            this.AddMap<Parent>(parents => from parent in parents
                                      select new
                                          {
                                              parent.Id,
                                              parent.IsActive,
                                              parent.Messages,
                                              parent.Name,
                                              parent.UserName,
                                              parent.Password
                                          });
            this.AddMap<Child>(children => from child in children
                                      select new
                                          {
                                              child.Id,
                                              child.IsActive,
                                              child.Messages,
                                              child.Name,
                                              child.UserName,
                                              child.Password
                                          });
        }
    }
}