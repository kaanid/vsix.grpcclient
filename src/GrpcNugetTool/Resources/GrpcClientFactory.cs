#if NET462
using System;
using System.Net.Http;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using FW.Basic.GrpcClient;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client.Configuration;

namespace $namespacename$
{
    public class GrpcClientFactory
    {
        private static CallInvoker? _channel;
        private static string[]? targets;

        public static void Init(string target, string subPath = "$subdirectoryHandlersubpath$")
        {
            if (string.IsNullOrWhiteSpace(target))
                throw new ArgumentNullException(nameof(target));
            var opt = new GrpcChannelOptions
            {
                HttpHandler = !string.IsNullOrWhiteSpace(subPath) ? new GrpcWebHandler(new SubdirectoryHandler(new HttpClientHandler(), subPath))
                                : new GrpcWebHandler(new HttpClientHandler()),
                ServiceConfig = new ServiceConfig
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
                }
            };
            GrpcChannel channel = GrpcChannel.ForAddress(target,opt);
            _channel = channel.Intercept(new ClientHeaderInterceptor());
        }

        public static $clientname$ Create()
        {
            if (_channel == null)
                throw new Exception("channel is null");

            return new $clientname$(_channel);
        }
    }
}
#endif
