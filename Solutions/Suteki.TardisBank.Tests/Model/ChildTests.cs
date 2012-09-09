// ReSharper disable InconsistentNaming
using NUnit.Framework;

namespace Suteki.TardisBank.Tests.Model
{
    using System;

    using Raven.Abstractions.Commands;
    using Raven.Client;
    using Raven.Client.Document;

    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.RavenDb;

    using global::Suteki.TardisBank.Domain;

    [TestFixture]
    public class ChildTests : RepositoryTestsBase
    {
        private string childId;
        private string parentId;
        protected override void LoadTestData()
        {
            var parent = new Parent("Mike Hadlow", "mike@yahoo.com", "yyy");
            session.Store(parent);

            parentId = parent.Id;

            var child = parent.CreateChild("Leo", "leohadlow", "xxx");
            session.Store(child);
            FlushSessionAndEvict(child);
            FlushSessionAndEvict(parent);
            this.childId = child.Id;
        }

        protected override void ClearTestData()
        {
            session.Advanced.Defer(new DeleteCommandData { Key = parentId });
            session.Advanced.Defer(new DeleteCommandData { Key = childId });
            session.SaveChanges();
            documentStore.Dispose();
        }

        [Test]
        public void Should_be_able_to_create_and_retrieve_a_child()
        {
                var child = new RavenDbRepositoryWithTypedId<Child, string>(session).Get(childId);
                child.Name.ShouldEqual("Leo");
                child.UserName.ShouldEqual("leohadlow");
                child.ParentId.ShouldEqual(parentId);
                child.Password.ShouldEqual("xxx");
                child.Account.ShouldNotBeNull();
        }

        [Test]
        public void Should_be_able_to_add_schedule_to_account()
        {
            ILinqRepositoryWithTypedId<Child, string> childRepository =  new RavenDbRepositoryWithTypedId<Child, string>(session);
            var childToTestOn = childRepository.FindOne(childId);

            Assert.AreEqual(0, childToTestOn.Account.PaymentSchedules.Count);

            if (childToTestOn == null)
            {
                childToTestOn = session.Load<Child>("children/" + childId);
            }
            childToTestOn.Account.AddPaymentSchedule(DateTime.UtcNow, Interval.Week, 10, "Weekly pocket money");    
            FlushSessionAndEvict(childToTestOn);

            var child = childRepository.FindOne(childId);
            Assert.Greater(child.Account.PaymentSchedules.Count, 0);
        }

        [Test]
        public void Should_be_able_to_add_transaction_to_account()
        {
            IRepositoryWithTypedId<Child, string> childRepository = new RavenDbRepositoryWithTypedId<Child, string>(session);
            var childToTestOn = childRepository.Get(childId);
            childToTestOn.ReceivePayment(10, "Reward");
            FlushSessionAndEvict(childToTestOn);

            var child = childRepository.Get(childId);
            Assert.Greater(child.Account.Transactions.Count, 0);
        }

    }

    public abstract class RepositoryTestsBase
    {
        protected IDocumentSession session;

        protected Exception ExceptionThrown;

        protected DocumentStore documentStore;

        [SetUp]
        protected virtual void SetUp()
        {
            InitializeSession();
            this.LoadTestData();
        }

        private void InitializeSession()
        {
            this.documentStore = new DocumentStore()
                {
                    Conventions =
                        {
                            FindTypeTagName = type => typeof(User).IsAssignableFrom(type) ? "users" : null
                        }
                };

            this.documentStore.Url = "http://localhost:8080/";
            this.documentStore.DefaultDatabase = "TardisBank.Tests";

            session = this.documentStore.Initialize().OpenSession();
            session.Advanced.AllowNonAuthoritativeInformation = false;
            session.Advanced.UseOptimisticConcurrency = true;
        }

        protected void FlushSessionAndEvict(object entity)
        {
            session.SaveChanges();
            session.Advanced.Evict(entity);
        }

        protected abstract void LoadTestData();

        [TearDown]
        protected abstract void ClearTestData();
    }
}
// ReSharper restore InconsistentNaming