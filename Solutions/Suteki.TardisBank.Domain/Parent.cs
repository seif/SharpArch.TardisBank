using System;
using System.Collections.Generic;
using System.Linq;
using Suteki.TardisBank.Events;

namespace Suteki.TardisBank.Model
{
    using SharpArch.Domain.DomainModel;
    using SharpArch.Domain.Events;

    public class Parent : User
    {
        public virtual IList<Child> Children { get; protected set; }
        public virtual string ActivationKey { get; protected set; }

        public Parent(string name, string userName, string password) : base(name, userName, password)
        {
            Children = new List<Child>();
        }

        protected Parent()
        {
        }
        // should be called when parent is first created.
        public virtual Parent Initialise()
        {
            ActivationKey = Guid.NewGuid().ToString();
            DomainEvents.Raise(new NewParentCreatedEvent(this));
            return this;
        }

        public virtual Child CreateChild(string name, string userName, string password)
        {
            var child = new Child(name, userName, password, Id);
            
            Children.Add(child);
            return child;
        }

        public virtual void MakePaymentTo(Child child, decimal amount)
        {
            MakePaymentTo(child, amount, string.Format("Payment from {0}", Name));
        }

        public virtual void MakePaymentTo(Child child, decimal amount, string description)
        {
            if (!HasChild(child))
            {
                throw new TardisBankException("{0} is not a child of {1}", child.Name, Name);
            }
            child.ReceivePayment(amount, description);
        }

        public virtual bool HasChild(Child child)
        {
            return Children.Any(x => x == child);
        }

        public override void Activate()
        {
            ActivationKey = "";
            base.Activate();
        }

        public virtual bool HasChild(int childId)
        {
            return Children.Any(x => x.Id == childId);
        }

        public virtual void RemoveChild(int childId)
        {
            var childToRemove = Children.SingleOrDefault(x => x.Id == childId);
            if (childToRemove != null)
            {
                Children.Remove(childToRemove);
            }
        }
    }
}