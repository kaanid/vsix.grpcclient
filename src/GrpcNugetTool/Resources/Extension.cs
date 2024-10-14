#if dotnet
using $namespacename$;
using FW.Basic.GrpcClient;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using System;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Extension
    {

        /// <summary>
        /// Add Grpc $extensionname$
        /// </summary>
        /// <param name="services"></param>
        /// <param name="target"></param>
        /// <param name="interceptor"></param>
        /// <returns></returns>
        public static IServiceCollection AddGrpc$extensionname$(this IServiceCollection services,
            Uri target,
            Interceptor? interceptor = null)
        {
            return AddGrpcAIService(services, target, null, "$subdirectoryHandlersubpath$", null, interceptor);
        }

        /// <summary>
        /// Add Grpc $extensionname$ name
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="target"></param>
        /// <param name="interceptor"></param>
        /// <returns></returns>
        public static IServiceCollection AddGrpc$extensionname$(this IServiceCollection services, string name, Uri target, Interceptor? interceptor = null)
        {
            return AddGrpcAIService(services, target, name, "$subdirectoryHandlersubpath$", null, interceptor);
        }
        
        /// <summary>
        /// Add Grpc $extensionname$
        /// </summary>
        /// <param name="services"></param>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <param name="subPath"></param>
        /// <param name="configOptions"></param>
        /// <param name="interceptor"></param>
        /// <returns></returns>
        public static IServiceCollection AddGrpc$extensionname$(this IServiceCollection services,
            Uri target,
            string? name = null,
            string? subPath = null,
            Action<GrpcChannelOptions>? configOptions = null,
            Interceptor? interceptor = null)
        {
            IHttpClientBuilder builder;
            if (string.IsNullOrWhiteSpace(name))
                builder = services.AddGrpcClient<$clientname$>(o =>
                    {
                        o.Address = target;
                        if (configOptions != null)
                            o.ChannelOptionsActions.Add(configOptions);
                    });
            else
                builder = services.AddGrpcClient<$clientname$>(name!, o =>
                {
                    o.Address = target;
                    if (configOptions != null)
                        o.ChannelOptionsActions.Add(configOptions);
                });


            if (!string.IsNullOrWhiteSpace(subPath))
                builder.ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new SubdirectoryHandler(new HttpClientHandler(), subPath!);
                });

            builder.AddInterceptor<ClientHeaderInterceptor>();
            if (interceptor != null)
                builder.AddInterceptor(() => interceptor);

            return services;
        }
    }
}
#endif