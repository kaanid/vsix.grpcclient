using System;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.GrpcGreeterClient;

namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IServiceProvider serviceProvider = new ServiceCollection()
                .AddGrpcGreeter(new Uri("https://localhost:5001"))
                .BuildServiceProvider();

            var client=serviceProvider.GetService<Greeter.GreeterClient>();
            Stopwatch stopwatch = new();
            for (var j= 0;j < 4; j++)
            {
                stopwatch.Restart();
                HelloReply response = null;
                for (var i = 0; i < 1000; i++)
                {
                    response = await client.SayHelloAsync(new HelloRequest
                    {
                        Name = "ConsoleApp2"
                    },
                    new Grpc.Core.Metadata {
                        new Grpc.Core.Metadata.Entry("test","test")
                    });
                }
                stopwatch.Stop();
                Console.WriteLine("{1}:{0}ms,{2}", stopwatch.ElapsedMilliseconds,j,response.Message);
            }
            
        }
    }
}
