// ReSharper disable InconsistentNaming

namespace Suteki.TardisBank.Tests.Model
{
    using System.Linq;

    using NUnit.Framework;

    using global::Suteki.TardisBank.Domain;

    [TestFixture]
    public class UserTests : RepositoryTestsBase
    {
        private Parent mike;

        private Child leo;

        private Parent john;

        private Child yuna;

        private Child jim;

        protected override void LoadTestData()
        {
            this.mike = new Parent("Mike Hadlow", "mike@yahoo.com", "yyy");
            this.leo = this.mike.CreateChild("Leo", "leohadlow", "xxx");
            this.yuna = this.mike.CreateChild("Yuna", "yunahadlow", "xxx");
            this.john = new Parent("John Robinson", "john@gmail.com", "yyy");
            this.jim = this.john.CreateChild("Jim", "jimrobinson", "xxx");

            
            session.Store(this.mike);
            session.Store(this.leo);
            session.Store(this.yuna);
            session.Store(this.john);
            session.Store(this.jim);
            session.SaveChanges();
        }

        protected override void ClearTestData()
        {
            session.Delete(mike);
            session.Delete(leo);
            session.Delete(yuna);
            session.Delete(john);
            session.Delete(jim);
            session.SaveChanges();
            documentStore.Dispose();
        }

        [Test]
        public void Should_be_able_to_treat_Parents_and_Children_Polymorphically()
        {
                var users = session.Query<User>().ToArray();

                users.Length.ShouldEqual(5);
            
                users[0].GetType().Name.ShouldEqual("Parent");
                users[1].GetType().Name.ShouldEqual("Child");
                users[2].GetType().Name.ShouldEqual("Child");
                users[3].GetType().Name.ShouldEqual("Parent");
                users[4].GetType().Name.ShouldEqual("Child");
        }
    }
}
// ReSharper restore InconsistentNaming