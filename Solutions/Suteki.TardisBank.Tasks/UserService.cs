namespace Suteki.TardisBank.Tasks
{
    using System;
    using System.Linq;

    using SharpArch.Domain.PersistenceSupport;

    using Suteki.TardisBank.Domain;

    public interface IUserService
    {
        User CurrentUser { get; }
        User GetUser(string userId);
        User GetUserByUserName(string userName);
        User GetUserByActivationKey(string activationKey);
        void SaveUser(User user);
        void DeleteUser(string userId);
        
        bool AreNullOrNotRelated(Parent parent, Child child);
        bool IsNotChildOfCurrentUser(Child child);
    }

    public class UserService : IUserService
    {
        readonly IHttpContextService context;

        private readonly ILinqRepositoryWithTypedId<Parent, string> parentRepository;

        readonly ILinqRepositoryWithTypedId<User, string> userRepository;

        public UserService(IHttpContextService context, ILinqRepositoryWithTypedId<Parent,string> parentRepository, ILinqRepositoryWithTypedId<User, string> userRepository)
        {
            this.context = context;
            this.parentRepository = parentRepository;
            this.userRepository = userRepository;
        }

        public User CurrentUser
        {
            get 
            {
                if (!this.context.UserIsAuthenticated) return null;

                return this.GetUserByUserName(this.context.UserName);
            }
        }

        public User GetUser(string userId)
        {
            return this.userRepository.FindOne(userId);
        }

        public User GetUserByUserName(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            return this.userRepository.FindOne(User.UserIdFromUserName(userName));
        }

        public User GetUserByActivationKey(string activationKey)
        {
            if (activationKey == null)
            {
                throw new ArgumentNullException("activationKey");
            }

            return this.parentRepository.FindAll().SingleOrDefault(x => x.ActivationKey == activationKey);            
        }

        public void SaveUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            this.userRepository.Save(user);
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
            var parent = this.CurrentUser as Parent;
            return (child == null) || (parent == null) || (!parent.HasChild(child));
        }

        public void DeleteUser(string userId)
        {
            var user = this.userRepository.FindOne(userId);

            this.userRepository.Delete(user);
        }
    }
}