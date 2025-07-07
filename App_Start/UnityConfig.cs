using CrediAPI.Domain.Services;
using CrediAPI.Domain.Services.Impl;
using CrediAPI.Infrastructure.Context;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace CrediAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
            var container = new UnityContainer();

            container.RegisterType<CrediDBContext>(new HierarchicalLifetimeManager());

            container.RegisterType<IPlazosService, PlazosServiceImpl>(new HierarchicalLifetimeManager());
            container.RegisterType<IProductoService, ProductoServiceImpl>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsuarioService, UsuarioServiceImpl>(new HierarchicalLifetimeManager());
            container.RegisterType<IAuthService, AuthServiceImpl>(new HierarchicalLifetimeManager());
            container.RegisterType<IPlanPagoService, PlanPagoServiceImpl>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}