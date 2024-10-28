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
        /// <param name="interceptor"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddGrpc$extensionname$(this IServiceCollection services,
            Uri target)
        {
            return services.AddGrpcClientServiceCore<$clientname$>(target, null, "$subdirectoryHandlersubpath$", true, true);
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
            return services.AddGrpcClientServiceCore<$clientname$>(target, name, "$subdirectoryHandlersubpath$", true, true);
        }
    }
}
#endif