[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(UnitOfWorkExample.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(UnitOfWorkExample.Web.App_Start.NinjectWebCommon), "Stop")]

namespace UnitOfWorkExample.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Extensions.Conventions;
    using Ninject.Extensions.Interception;
    using System.Configuration;
    using System.Reflection;
    using UnitOfWorkExample.Domain.Entities;
    using UnitOfWorkExample.Data.Helpers;
    using UnitOfWorkExample.Data.MappingOverrides;
    using UnitOfWorkExample.Domain.Helpers;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // unit of work per request
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope(); 

            // default binding for everything except unit of work
            kernel.Bind(x => x.FromAssembliesMatching("*").SelectAllClasses().Excluding<UnitOfWork>().BindDefaultInterface());
        }
    }
}
