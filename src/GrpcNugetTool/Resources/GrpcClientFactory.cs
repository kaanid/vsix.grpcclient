#if NET462
using System;
using System.Net.Http;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using FW.Basic.GrpcClient;

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

            GrpcChannel channel;
            if (!string.IsNullOrWhiteSpace(subPath))
                channel = GrpcChannel.ForAddress(target, new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new SubdirectoryHandler(new HttpClientHandler(), subPath))
                });
            else
                channel = GrpcChannel.ForAddress(target, new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });

            _channel=channel.Intercept(new ClientHeaderInterceptor());
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
