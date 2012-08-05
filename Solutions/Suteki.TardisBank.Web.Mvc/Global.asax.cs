namespace Suteki.TardisBank.Web.Mvc
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Castle.Windsor;
    using Castle.Windsor.Installer;

    using CommonServiceLocator.WindsorAdapter;

    using SharpArch.Domain.Events;

    using Suteki.TardisBank.Web.Mvc.Utilities;

    using log4net.Config;

    using Microsoft.Practices.ServiceLocation;

    using SharpArch.Web.Mvc.Castle;
    using SharpArch.Web.Mvc.ModelBinder;

    /// <summary>
    /// Represents the MVC Application
    /// </summary>
    /// <remarks>
    /// For instructions on enabling IIS6 or IIS7 classic mode, 
    /// visit http://go.microsoft.com/?LinkId=9394801
    /// </remarks>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Due to issues on IIS7, the NHibernate initialization must occur in Init().
        /// But Init() may be invoked more than once; accordingly, we introduce a thread-safe
        /// mechanism to ensure it's only initialized once.
        /// See http://msdn.microsoft.com/en-us/magazine/cc188793.aspx for explanation details.
        /// </summary>
        public override void Init()
        {
            base.Init();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e) 
        {
            // Useful for debugging
            Exception ex = this.Server.GetLastError();
            var reflectionTypeLoadException = ex as ReflectionTypeLoadException;
        }

        protected void Application_Start()
        {
            XmlConfigurator.Configure();

            ViewEngines.Engines.Clear();

            ViewEngines.Engines.Add(new RazorViewEngine());

            ModelBinders.Binders.DefaultBinder = new SharpModelBinder();

            ModelValidatorProviders.Providers.Add(new ClientDataTypeModelValidatorProvider());

            this.InitializeServiceLocator();

            AreaRegistration.RegisterAllAreas();
            RouteRegistrar.RegisterRoutesTo(RouteTable.Routes);
        }

        /// <summary>
        /// Instantiate the container and add all Controllers that derive from
        /// WindsorController to the container.  Also associate the Controller
        /// with the WindsorContainer ControllerFactory.
        /// </summary>
        protected virtual void InitializeServiceLocator() 
        {
            IWindsorContainer container = new WindsorContainer();
            container.Install(FromAssembly.This());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            var windsorServiceLocator = new WindsorServiceLocator(container);

            DomainEvents.ServiceLocator = windsorServiceLocator;

            ServiceLocator.SetLocatorProvider(() => windsorServiceLocator);
        }
    }
}