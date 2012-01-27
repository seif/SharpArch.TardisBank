using System;
using System.Collections.Generic;
using System.Linq;
using Suteki.TardisBank.Events;

namespace Suteki.TardisBank.Model
{
    using SharpArch.Domain.DomainModel;
    using SharpArch.Domain.Events;

    public abstract class User : Entity
    {
        public const int MaxMessages = 20;

        protected User()
        {
        }

        public virtual string Name { get; protected set; }
        public virtual string UserName { get; protected set; }
        public virtual string Password { get; protected set; }
        public virtual bool IsActive { get; protected set; }
        public virtual IList<Message> Messages { get; protected set; }

        protected User(string name, string userName, string password)
        {
            Name = name;
            UserName = userName;
            Password = password;
            Messages = new List<Message>();
            IsActive = false;
        }

        public virtual void SendMessage(string text)
        {
            Messages.Add(new Message(DateTime.Now.Date, text));
            RemoveOldMessages();

            DomainEvents.Raise(new SendMessageEvent(this, text));
        }

        void RemoveOldMessages()
        {
            if (Messages.Count <= MaxMessages) return;

            var oldestMessage = Messages.First();
            Messages.Remove(oldestMessage);
        }

        public virtual void ReadMessage(int messageId)
        {
            var message = Messages.SingleOrDefault(x => x.Id == messageId);
            if (message == null)
            {
                throw new TardisBankException("No message with Id {0} found for user '{1}'", messageId, UserName);
            }
            message.Read();
        }

        public virtual void Activate()
        {
            IsActive = true;
        }

        public virtual void ResetPassword(string newPassword)
        {
            Password = newPassword;
        }
    }
}