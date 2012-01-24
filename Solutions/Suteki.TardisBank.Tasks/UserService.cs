using System;
using System.Linq;
using System.Collections.Generic;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Services
{
    public interface IUserService
    {
        User CurrentUser { get; }
        User GetUser(string userId);
        User GetUserByUserName(string userName);
        User GetUserByActivationKey(string activationKey);
        void SaveUser(User user);
        void DeleteUser(string userId);
        IEnumerable<Child> GetChildrenOf(Parent parent);

        bool AreNullOrNotRelated(Parent parent, Child child);
        bool IsNotChildOfCurrentUser(Child child);
    }

    public class UserService : IUserService
    {
        readonly IHttpContextService context;

        public UserService(IHttpContextService context)
        {
            this.context = context;
        }

        public User CurrentUser
        {
            get 
            {
                if (!context.UserIsAuthenticated) return null;
                var userId = string.Format("users/{0}", context.UserName);

                // TODO return user
                throw new NotImplementedException("Just build for now.");
            }
        }

        public User GetUser(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            // TODO return user
            throw new NotImplementedException("Just build for now.");
        }

        public User GetUserByUserName(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }
            
            // TODO return user
            throw new NotImplementedException("Just build for now.");
        }

        public User GetUserByActivationKey(string activationKey)
        {
            if (activationKey == null)
            {
                throw new ArgumentNullException("activationKey");
            }

            //return session.Query<Parent>().Where(x => x.ActivationKey == activationKey).SingleOrDefault();
            // TODO return user
            throw new NotImplementedException("Just build for now.");
        }

        public void SaveUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            // TODO save user
        }

        public IEnumerable<Child> GetChildrenOf(Parent parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            // TODO
            throw new NotImplementedException("Just build for now.");
        }

        public bool AreNullOrNotRelated(Parent parent, Child child)
        {
            if (parent == null || child == null) return true;

            if (!parent.HasChild(child))
            {
                throw new TardisBankException("'{0}' is not a child of '{1}'", child.UserName, parent.UserName);
            }

            return false;
        }

        public bool IsNotChildOfCurrentUser(Child child)
        {
            var parent = CurrentUser as Parent;
            return (child == null) || (parent == null) || (!parent.HasChild(child));
        }

        public void DeleteUser(string userId)
        {
            //TODO
        }
    }
}