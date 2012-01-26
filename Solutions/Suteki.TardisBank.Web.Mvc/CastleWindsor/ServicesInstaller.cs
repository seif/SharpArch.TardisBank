using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Suteki.TardisBank.IoC
{
    using Suteki.TardisBank.Services;

    public class TasksInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes
                    .FromAssemblyContaining<UserService>()
                    .Where(Component.IsInSameNamespaceAs<UserService>())
                    .WithService.DefaultInterfaces()
                    .Configure(c => c.LifestyleTransient())
                );
        }
    }
}