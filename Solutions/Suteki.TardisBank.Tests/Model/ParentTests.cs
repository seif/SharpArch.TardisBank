// ReSharper disable InconsistentNaming
using NUnit.Framework;

namespace Suteki.TardisBank.Tests.Model
{
    using SharpArch.RavenDb;

    using global::Suteki.TardisBank.Domain;

    [TestFixture]
    public class ParentTests : RepositoryTestsBase
    {
        private string parentId;

        protected override void LoadTestData()
        {
            var parent = new Parent(name: "Mike Hadlow", userName: string.Format("{0}@yahoo.com", "mike"), password: "yyy");
            session.Store(parent);
            parentId = parent.Id;
            this.FlushSessionAndEvict(parent);
        }

        protected override void ClearTestData()
        {
            this.session.Delete(this.session.Load<Parent>(parentId));
            this.session.SaveChanges();
            documentStore.Dispose();
        }

        [Test]
        public void Should_be_able_to_create_and_retrieve_Parent()
        {
            var parent = new RavenDbRepositoryWithTypedId<Parent, string>(session).Get(parentId);
            parent.ShouldNotBeNull();
            parent.Name.ShouldEqual("Mike Hadlow");
            parent.UserName.ShouldEqual("mike@yahoo.com");
            parent.Children.ShouldNotBeNull();
        }

        [Test]
        public void Should_be_able_to_add_a_child_to_a_parent()
        {
            var linqRepository = new RavenDbRepositoryWithTypedId<Parent, string>(session);
            var savedParent = linqRepository.Get(parentId);
            savedParent.CreateChild("jim", "jim123", "passw0rd1");
            savedParent.CreateChild("jenny", "jenny123", "passw0rd2");
            savedParent.CreateChild("jez", "jez123", "passw0rd3");
            this.FlushSessionAndEvict(savedParent);

            var parent = linqRepository.Get(parentId);
            parent.Children.Count.ShouldEqual(3);

            parent.Children[0].ShouldEqual("Users/jim123");
            parent.Children[1].ShouldEqual("Users/jenny123");
            parent.Children[2].ShouldEqual("Users/jez123");
        }
    }
}
// ReSharper restore InconsistentNaming