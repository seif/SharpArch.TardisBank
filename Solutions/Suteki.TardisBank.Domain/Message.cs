using System;

namespace Suteki.TardisBank.Model
{
    using SharpArch.Domain.DomainModel;

    public class Message : Entity
    {
        public Message(DateTime date, string text)
        {
            Date = date;
            Text = text;
            HasBeenRead = false;
        }

        public void Read()
        {
            HasBeenRead = true;
        }

        public DateTime Date { get; private set; }
        public string Text { get; private set; }
        public bool HasBeenRead { get; private set; }
    }
}