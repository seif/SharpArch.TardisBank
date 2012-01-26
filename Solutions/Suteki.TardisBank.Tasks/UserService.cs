using System;
using System.Linq;
using System.Collections.Generic;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.Services
{
    using SharpArch.Domain.PersistenceSupport;

    public interface IUserService
    {
        User CurrentUser { get; }
        User GetUser(int userId);
        User GetUserByUserName(string userName);
        User GetUserByActivationKey(int activationKey);
        void SaveUser(User user);
        void DeleteUser(int userId);
        IEnumerable<Child> GetChildrenOf(Parent parent);

        bool AreNullOrNotRelated(Parent parent, Child child);
        bool IsNotChildOfCurrentUser(Child child);
    }

    public class UserService : IUserService
    {
        readonly IHttpContextService context;

        readonly ILinqRepository<User> userRepository;

        public UserService(IHttpContextService context, ILinqRepository<User> userRepository)
        {
            this.context = context;
            this.userRepository = userRepository;
        }

        public User CurrentUser
        {
            get 
            {
                if (!context.UserIsAuthenticated) return null;

                return this.GetUserByUserName(context.UserName);
            }
        }

        public User GetUser(int userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            return userRepository.FindOne(userId);
        }

        public User GetUserByUserName(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            return this.userRepository.FindAll().FirstOrDefault(u => u.UserName == userName);
        }

        public User GetUserByActivationKey(int activationKey)
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

        public void DeleteUser(int userId)
        {
            //TODO
        }
    }
}