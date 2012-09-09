namespace Suteki.TardisBank.Tests.Model
{
    using NUnit.Framework;

    using SharpArch.RavenDb;

    public class RavenTests : RepositoryTestsBase
    {
        private int id;

        protected override void LoadTestData()
        {
            var civic = new Car("Honda", "Civic");
            var golf = new Car("VW", "Golf");

            session.Store(civic);
            session.Store(golf);
            id = civic.Id;
            session.SaveChanges();
            this.FlushSessionAndEvict(civic);
            this.FlushSessionAndEvict(golf);
        }
        
        [Test]
        public void ShouldBeAbleToLoadItemByIntId()
        {
            var repo = new RavenDbRepositoryWithTypedId<Car, int>(session);
            var item = repo.FindOne(id);
            item.ShouldNotBeNull();
            item.Model.ShouldEqual("Civic");
            item.Manufacturer.ShouldEqual("Honda");
        }

        [Test]
        public void ShouldBeAbleToDeleteById()
        {
            var repo = new RavenDbRepositoryWithTypedId<Car, int>(session);
            repo.Delete(id);
            repo.DbContext.CommitChanges();

            var deleted = session.Load<Car>(id);
            deleted.ShouldBeNull();
        }

        protected override void ClearTestData()
        {
        }
    }

    public class Car
    {
        private int id;

        private string manufacturer;

        private string model;

        public Car(string manufacturer, string model)
        {
            this.Manufacturer = manufacturer;
            this.Model = model;
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public string Manufacturer
        {
            get
            {
                return this.manufacturer;
            }
            set
            {
                this.manufacturer = value;
            }
        }

        public string Model
        {
            get
            {
                return this.model;
            }
            set
            {
                this.model = value;
            }
        }
    }
}