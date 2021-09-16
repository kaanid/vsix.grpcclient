using Grpc.Core;
using Grpc.GrpcGreeterClient;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcClientFactory.Init("127.0.0.1:5000");
            
            for (int i = 0; i < 1000; i++)
            {
                try
                {
                    var response2 = await GrpcClientFactory.Create().SayHelloAsync(new HelloRequest { Name = ".NET " + i });
                    Console.WriteLine("Greeting: " + response2.Message);

                    var client4 = GrpcClientFactory.Create();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            Stopwatch sw = Stopwatch.StartNew();
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var channel = GrpcChannel.ForAddress("http://localhost:5000");
            //var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
            //{
            //    HttpHandler = new WinHttpHandler()
            //});

            //var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
            //{
            //    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
            //});

            //var channel = GrpcChannel.ForAddress("http://localhost:5000", new GrpcChannelOptions
            //{
            //    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
            //});

            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //Channel channel = new Channel("127.0.0.1:5001", ChannelCredentials.SecureSsl);
            //Channel channel = new Channel("127.0.0.1",5001, new SslCredentials());
            //Channel channel = new Channel("127.0.0.1:5000", ChannelCredentials.Insecure);
            //var client = new Greeter.GreeterClient(channel);
            var client = GrpcClientFactory.Create();

            HelloReply response = null;
            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    response = await client.SayHelloAsync(new HelloRequest { Name = ".NET " +j+"-"+ i });
                }
                sw.Stop();
                Console.WriteLine("Greeting: " + response.Message + " " + sw.ElapsedMilliseconds + "ms");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
