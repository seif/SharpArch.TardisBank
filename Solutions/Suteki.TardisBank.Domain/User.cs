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

        public string Name { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public bool IsActive { get; protected set; }
        public IList<Message> Messages { get; protected set; }

        protected User(string name, string userName, string password)
        {
            Name = name;
            UserName = userName;
            Password = password;
            Messages = new List<Message>();
            IsActive = false;
        }

        public void SendMessage(string text)
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

        public void ReadMessage(int messageId)
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

        public void ResetPassword(string newPassword)
        {
            Password = newPassword;
        }
    }
}