using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Suteki.TardisBank.Handlers;

namespace Suteki.TardisBank.IoC
{
    using SharpArch.Domain.Commands;
    using SharpArch.Domain.Events;

    public class HandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                AllTypes.FromAssemblyContaining<SendMessageEmailHandler>()
                    .BasedOn(typeof(ICommandHandler<>))
                    .WithService.FirstInterface().LifestyleTransient());

            container.Register(
                AllTypes.FromAssemblyContaining<SendMessageEmailHandler>()
                    .BasedOn(typeof(IHandles<>))
                    .WithService.FirstInterface().LifestyleTransient());
        }
    }
}