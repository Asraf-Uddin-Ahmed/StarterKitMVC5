[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($safeprojectname$.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof($safeprojectname$.App_Start.NinjectWebCommon), "Stop")]

namespace $safeprojectname$.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using System;
    using System.Web;
    using $safeprojectname$.Codes;

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
            log4net.Config.XmlConfigurator.Configure();
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
            return _Kernel;
        }

        static IKernel _kernel;
        private static IKernel _Kernel
        {
            get
            {
                if (_kernel == null)
                {
                    _kernel = new StandardKernel();
                    _kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                    _kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                    RegisterServices(_kernel);
                }
                return _kernel;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(new INinjectModule[]
            {
                new NinjectWebModule()
            });

            kernel.Bind(x =>
            {
                x.FromAssembliesMatching("*") // Scans all assemblies
                 .SelectAllClasses() // Retrieve all non-abstract classes
                 .BindDefaultInterface(); // Binds the default interface to them;
            });
        }

        /// <summary>
        /// Creates concrete class instace
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetConcreteInstance<T>()
        {
            object instance = _Kernel.TryGet<T>();
            if (instance != null)
                return (T)instance;
            throw new InvalidOperationException(string.Format("Unable to create an instance of {0}", typeof(T).FullName));
        }
    }
}
