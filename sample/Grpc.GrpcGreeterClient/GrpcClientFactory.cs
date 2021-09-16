#if NET461
using System;
using Grpc.Core;
using Grpc.GrpcGreeterClient;

namespace Grpc.GrpcGreeterClient
{
    public class GrpcClientFactory
    {
        private static Channel _channel;
        private static string[] targets;

        public static void Init(string target)
        {
            if (string.IsNullOrWhiteSpace(target))
                throw new ArgumentNullException(nameof(target));

            targets = target.Split(',');
        }

        public static Greeter.GreeterClient Create()
        {
            if (_channel == null || _channel.State == ChannelState.TransientFailure)
            {
                _channel = new Channel(targets[DateTime.Now.Millisecond % targets.Length], ChannelCredentials.Insecure);
            }

            return new Greeter.GreeterClient(_channel);
        }
    }
}
#endif
