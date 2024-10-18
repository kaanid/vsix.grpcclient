#if dotnet
using $namespacename$;
using FW.Basic.GrpcClient;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        public static IHttpClientBuilder AddGrpc$extensionname$(this IServiceCollection services,
            Uri target,
            Interceptor? interceptor = null)
        {
            return AddGrpc$extensionname$Core(services, target, null, "$subdirectoryHandlersubpath$", null, interceptor);
        }

        /// <summary>
        /// Add Grpc $extensionname$ name
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="target"></param>
        /// <param name="interceptor"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddGrpc$extensionname$(this IServiceCollection services, string name, Uri target, Interceptor? interceptor = null)
        {
            return AddGrpc$extensionname$Core(services, target, name, "$subdirectoryHandlersubpath$", null, interceptor);
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
        public static IHttpClientBuilder AddGrpc$extensionname$Core(this IServiceCollection services,
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
                    });
            else
                builder = services.AddGrpcClient<$clientname$>(name!, o =>
                {
                    o.Address = target;
                });


            if (!string.IsNullOrWhiteSpace(subPath))
                builder.ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new SubdirectoryHandler(new HttpClientHandler(), subPath!);
                });

            services.TryAddSingleton<ClientHeaderInterceptor>();
            builder.AddInterceptor<ClientHeaderInterceptor>();
            if (interceptor != null)
                builder.AddInterceptor(() => interceptor);

            builder.ConfigureChannel(op =>
            {
                op.ServiceConfig = new Grpc.Net.Client.Configuration.ServiceConfig
                {
                    MethodConfigs = {
                        new MethodConfig {
                            Names={ MethodName.Default},
                            RetryPolicy=new RetryPolicy{
                                MaxAttempts=3,
                                InitialBackoff=TimeSpan.FromSeconds(1),
                                MaxBackoff=TimeSpan.FromSeconds(4),
                                BackoffMultiplier=2,
                                RetryableStatusCodes = { StatusCode.Unavailable }
                            }
                        }
                    }
                };
            });

            if (configOptions != null)
                builder.ConfigureChannel(configOptions);

            return builder;
        }
    }
}
#endif