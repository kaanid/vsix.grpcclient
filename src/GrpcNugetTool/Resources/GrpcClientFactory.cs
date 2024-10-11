#if NET462
using System;
using System.Net.Http;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace $namespacename$
{
    public class GrpcClientFactory
    {
        private static GrpcChannel? _channel;
        private static string[]? targets;

        public static void Init(string target, string subPath = "$subdirectoryHandlersubpath$")
        {
            if (string.IsNullOrWhiteSpace(target))
                throw new ArgumentNullException(nameof(target));

            if (!string.IsNullOrWhiteSpace(subPath))
                _channel = GrpcChannel.ForAddress(target, new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new SubdirectoryHandler(new HttpClientHandler(), subPath))
                });
            else
                _channel = GrpcChannel.ForAddress(target, new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
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
