using System;

namespace Suteki.TardisBank.Model
{
    using SharpArch.Domain.DomainModel;

    public class Message : Entity
    {
        public Message(DateTime date, string text, User user)
        {
            Date = date;
            Text = text;
            User = user;
            HasBeenRead = false;
        }

        protected Message()
        {
        }

        public virtual void Read()
        {
            HasBeenRead = true;
        }

        public virtual DateTime Date { get; protected set; }
        public virtual string Text { get; protected set; }

        public virtual User User { get; set; }

        public virtual bool HasBeenRead { get; protected set; }
    }
}