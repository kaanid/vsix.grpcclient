#if NET8_0_OR_GREATER
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
        /// <returns></returns>
        public static IHttpClientBuilder AddGrpc$extensionname$(this IServiceCollection services,Uri target)
        {
            return services.AddGrpc$extensionname$(target, null, true, true);
        }

        /// <summary>
        /// Add Grpc $extensionname$ name
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddGrpc$extensionname$(this IServiceCollection services, string name, Uri target)
        {
            return services.AddGrpc$extensionname$(target, name, true, true);
        }

        /// <summary>
        /// Add Grpc $extensionname$ name
        /// </summary>
        /// <param name="services"></param>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <param name="enableDefaultClientHeaderInterceptor"></param>
        /// <param name="enableDefaultServiceConfig"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddGrpc$extensionname$(this IServiceCollection services, 
            Uri target,
            string? name, 
            bool enableDefaultClientHeaderInterceptor = true, 
            bool enableDefaultServiceConfig = true
            )
        {
            return services.AddGrpcClientServiceCore<$clientname$>(target, name, "$subdirectoryHandlersubpath$", enableDefaultClientHeaderInterceptor, enableDefaultServiceConfig);
        }
    }
}
#endif