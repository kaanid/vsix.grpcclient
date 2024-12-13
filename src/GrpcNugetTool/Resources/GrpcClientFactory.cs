﻿#if NET462
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
        public const string Name = "$clientname$";

        public static void Init(string fwapp,string target, string subPath = "$subdirectoryHandlersubpath$")
        {
            if (string.IsNullOrWhiteSpace(target))
                throw new ArgumentNullException(nameof(target));

            FW.Basic.GrpcClient.GrpcClientFactoryCore.Init(fwapp, Name, target, subPath);
        }

        public static $clientname$ Create()
        {
            var channel = FW.Basic.GrpcClient.GrpcClientFactoryCore.Create(Name) ?? throw new Exception("channel is null");

            return new $clientname$(channel);
        }
    }
}
#endif
