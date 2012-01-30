// ReSharper disable InconsistentNaming
using NUnit.Framework;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Tests.Model
{
    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.NHibernate;
    using SharpArch.Testing.NUnit.NHibernate;

    [TestFixture]
    public class ChildTests : RepositoryTestsBase
    {
        private int childId;
        
        protected override void LoadTestData()
        {
            var parent = new Parent("Mike Hadlow", "mike@yahoo.com", "yyy");
            NHibernateSession.Current.Save(parent);
            var child = parent.CreateChild("Leo", "leohadlow", "xxx");
            NHibernateSession.Current.Save(child);
            RepositoryTestsHelper.FlushSessionAndEvict(child);
            this.childId = child.Id;
        }

        [Test]
        public void Should_be_able_to_create_and_retrieve_a_child()
        {
                var child = new LinqRepository<Child>().Get(childId);
                child.Name.ShouldEqual("Leo");
                child.UserName.ShouldEqual("leohadlow");
                child.Id.ShouldEqual("users/leohadlow");
                child.ParentId.ShouldEqual("users/mike@yahoo.com");
                child.Password.ShouldEqual("xxx");
                child.Account.ShouldNotBeNull();
        }
    }
}
// ReSharper restore InconsistentNaming