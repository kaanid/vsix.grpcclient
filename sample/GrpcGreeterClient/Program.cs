using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using System.Diagnostics;

namespace GrpcGreeterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Stopwatch sw = null;

            sw = Stopwatch.StartNew();
            HelloReply reply2=null;
            using var channel2=GrpcChannel.ForAddress("https://localhost:5001");
            for (int i = 0; i < 1000; i++)
            {
                var client2=new Greeter.GreeterClient(channel2);
                reply2 = await client2.SayHelloAsync(new HelloRequest{Name="GreeterClient"});
            }
            Console.WriteLine("5001 Greeting: "+reply2.Message+" "+sw.ElapsedMilliseconds+"ms");

            sw = Stopwatch.StartNew();
            HelloReply reply=null;
            using var channel=GrpcChannel.ForAddress("http://localhost:5000");
            for (int i = 0; i < 1000; i++)
            {
                var client=new Greeter.GreeterClient(channel);
                reply = await client.SayHelloAsync(new HelloRequest{Name="GreeterClient"});
            }
            Console.WriteLine("5000 Greeting: "+reply.Message+" "+sw.ElapsedMilliseconds+"ms");

            sw = Stopwatch.StartNew();
            HelloReply reply4=null;
            using var channel4=GrpcChannel.ForAddress("http://localhost:5000");
            var client4=new Greeter.GreeterClient(channel4);
            for (int i = 0; i < 1000; i++)
            {
                reply4 = await client4.SayHelloAsync(new HelloRequest{Name="GreeterClient"});
            }
            Console.WriteLine("5000-4 Greeting: "+reply4.Message+" "+sw.ElapsedMilliseconds+"ms");

            sw = Stopwatch.StartNew();
            HelloReply reply6=null;
            using var channel6=GrpcChannel.ForAddress("https://localhost:5001");
            var client6=new Greeter.GreeterClient(channel6);
            for (int i = 0; i < 1000; i++)
            {
                reply6 = await client6.SayHelloAsync(new HelloRequest{Name="GreeterClient"});
            }
            Console.WriteLine("5001-6 Greeting: "+reply6.Message+" "+sw.ElapsedMilliseconds+"ms");

            sw = Stopwatch.StartNew();
            HelloReply reply8=null;
            using var channel8=GrpcChannel.ForAddress("https://localhost:5001");
            for (int i = 0; i < 1000; i++)
            {
                var client8=new Greeter.GreeterClient(channel8);
                reply8 = await client8.SayHelloAsync(new HelloRequest{Name="GreeterClient"});
            }
            Console.WriteLine("5001-8 Greeting: "+reply2.Message+" "+sw.ElapsedMilliseconds+"ms");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
