namespace Suteki.TardisBank.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SharpArch.Domain.Events;

    using Suteki.TardisBank.Domain.Events;

    public class Parent : User
    {
        public virtual IList<string> Children { get; protected set; }
        public virtual string ActivationKey { get; protected set; }

        public Parent(string name, string userName, string password) : base(name, userName, password)
        {
            this.Children = new List<string>();
        }

        protected Parent()
        {
        }
        // should be called when parent is first created.
        public virtual Parent Initialise()
        {
            this.ActivationKey = Guid.NewGuid().ToString();
            DomainEvents.Raise(new NewParentCreatedEvent(this));
            return this;
        }

        public virtual Child CreateChild(string name, string userName, string password)
        {
            var child = new Child(name, userName, password, this.Id);
            this.Children.Add(child.Id);
            return child;
        }

        public virtual void MakePaymentTo(Child child, decimal amount)
        {
            this.MakePaymentTo(child, amount, string.Format("Payment from {0}", this.Name));
        }

        public virtual void MakePaymentTo(Child child, decimal amount, string description)
        {
            if (!this.HasChild(child))
            {
                throw new TardisBankException("{0} is not a child of {1}", child.Name, this.Name);
            }
            child.ReceivePayment(amount, description);
        }

        public virtual bool HasChild(Child child)
        {
            return this.Children.Any(x => x == child.Id);
        }

        public override void Activate()
        {
            this.ActivationKey = "";
            base.Activate();
        }

        public virtual bool HasChild(string childId)
        {
            return this.Children.Any(x => x == childId);
        }

        public virtual void RemoveChild(string childId)
        {
            var childToRemove = this.Children.SingleOrDefault(x => x == childId);
            if (childToRemove != null)
            {
                this.Children.Remove(childToRemove);
            }
        }
    }
}