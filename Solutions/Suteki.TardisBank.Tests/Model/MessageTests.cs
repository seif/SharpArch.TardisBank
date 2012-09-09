// ReSharper disable InconsistentNaming

namespace Suteki.TardisBank.Tests.Model
{   
    using NUnit.Framework;

    using SharpArch.RavenDb;

    using global::Suteki.TardisBank.Domain;

    [TestFixture]
    public class MessageTests : RepositoryTestsBase
    {
        private string userId;

        protected override void LoadTestData()
        {
            User user = new Parent("Dad", "mike@mike.com", "xxx");
            session.Store(user);
            userId = user.Id;
            this.FlushSessionAndEvict(user);
        }

        protected override void ClearTestData()
        {
            session.Delete(session.Load<Parent>(userId));
            session.SaveChanges();
            documentStore.Dispose();
        }

        [Test]
        public void Should_be_able_to_add_a_message_to_a_user()
        {
            var parentRepository = new RavenDbRepositoryWithTypedId<Parent,string>(session);
            User userToTestWith = parentRepository.Get(userId);

            userToTestWith.SendMessage("some message");

            FlushSessionAndEvict(userToTestWith);


            var parent = parentRepository.Get(userId);
            parent.Messages.Count.ShouldEqual(1);
        }
    }
}
// ReSharper restore InconsistentNaming