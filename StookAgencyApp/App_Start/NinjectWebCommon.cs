[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(StookAgencyApp.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(StookAgencyApp.App_Start.NinjectWebCommon), "Stop")]

namespace StookAgencyApp.App_Start
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using StookAgencyApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using StookAgencyApp.Services;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                RegisterMappers(kernel);

                //System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new Ninject.WebApi.DependencyResolver.NinjectDependencyResolver(kernel);

                //AppContext.DependencyResolver = System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver.GetService;

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            //kernel.Bind<ILog>().ToMethod(ctx => LogManager.GetLogger(ctx.Request.Target.Member.DeclaringType));

            kernel.Bind<IdentityDbContext>().To<IdentityDbContext>().InRequestScope();
            kernel.Bind<UserManager<User>>().To<ApplicationUserManager>().InRequestScope();
            kernel.Bind<RoleManager<Role>>().To<ApplicationRoleManager>().InRequestScope();
            kernel.Bind<SignInManager<User, string>>().To<ApplicationSignInManager>().InRequestScope();
            kernel.Bind<IAuthenticationManager>().ToMethod(ctx => HttpContext.Current.GetOwinContext().Authentication);
            kernel.Bind<IUserStore<User>>().To<ApplicationUserStore>().InRequestScope();
            kernel.Bind<IRoleStore<Role>>().To<ApplicationRoleStore>().InRequestScope();

#if DEBUG
            kernel.Bind<AppDbContext>().ToSelf().InRequestScope().OnActivation((Action<AppDbContext>)(ctx =>
            {
                ctx.Database.Log = m => System.Diagnostics.Trace.Write(m);
            }));
#else
                        kernel.Bind<AppDbContext>().ToSelf().InRequestScope();
#endif
        }

        private static void RegisterMappers(IKernel kernel)
        {
            //MapperConfiguration config = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile(new App.Core.Mappers.ContentMappingProfile());

            //    cfg.AddProfile(new App.Presentation.Mappers.OrderMappingProfile());
            //    cfg.AddProfile(new App.Presentation.Mappers.UserMappingProfile());
            //    cfg.AddProfile(new App.Presentation.Mappers.ContentMappingProfile());
            //});

            //kernel.Bind<IMapper>().ToMethod(ctx => config.CreateMapper()).InSingletonScope();
        }
    }
}