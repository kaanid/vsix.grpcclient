#if NET461
using System;
using Grpc.Core;

namespace $namespacename$
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

        public static $clientname$ Create()
        {
            if (_channel == null || _channel.State == ChannelState.TransientFailure)
            {
                _channel = new Channel(targets[DateTime.Now.Millisecond % targets.Length], ChannelCredentials.Insecure);
            }

            return new $clientname$(_channel);
        }
    }
}
#endif
