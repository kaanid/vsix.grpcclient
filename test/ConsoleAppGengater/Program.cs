using GrpcNugetTool.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppGengater
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var options = new GrpcOptions
            {
                ProtoFile = @"F:\webapi\FW.Basic.AIService\src\Fanews.Grpc.AIServiceServer\Protos\AIService.proto",
                ResourceProjectDir = @"F:\test\grpc\src\GrpcNugetTool\Resources",
                NugetPush = false
            };

            var service = new GrpcGenerateService(options);
            service.Run(true);
        }
    }
}
