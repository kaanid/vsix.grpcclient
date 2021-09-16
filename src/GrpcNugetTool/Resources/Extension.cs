#if dotnet
using $namespacename$;
using Grpc.Core.Interceptors;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Extension
    {
        /// <summary>
        /// add Greeter.GreeterClient
        /// </summary>
        /// <param name="services"></param>
        /// <param name="target"></param>
        /// <param name="interceptor"></param>
        /// <returns></returns>
        public static IServiceCollection AddGrpc$extensionname$(this IServiceCollection services, Uri target, Interceptor interceptor = null)
        {
            var builder = services.AddGrpcClient<$clientname$>(o =>
                {
                    o.Address = target;
                });

            if (interceptor != null)
                builder.AddInterceptor(() => interceptor);

            return services;
        }

        /// <summary>
        /// add Greeter.GreeterClient
        /// GrpcClientFactory.CreateClient<Greeter.GreeterClient>("name");
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="target"></param>
        /// <param name="interceptor"></param>
        /// <returns></returns>
        public static IServiceCollection AddGrpc$extensionname$(this IServiceCollection services, string name, Uri target, Interceptor interceptor = null)
        {
            var builder = services.AddGrpcClient<$clientname$>(name, o =>
               {
                   o.Address = target;
               });

            if (interceptor != null)
                builder.AddInterceptor(() => interceptor);

            return services;
        }
    }
}
#endif