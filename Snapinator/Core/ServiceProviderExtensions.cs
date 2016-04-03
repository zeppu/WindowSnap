using System;

namespace Snapinator.Core
{
    public static class ServiceProviderExtensions
    {
        public static TService Get<TService>(this IServiceProvider serviceProvider)
        {
            return (TService)serviceProvider.GetService(typeof(TService));
        }
        public static object Get(this IServiceProvider serviceProvider, Type serviceType)
        {
            return serviceProvider.GetService(serviceType);
        }
    }
}