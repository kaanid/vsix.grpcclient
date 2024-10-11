using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace GrpcNugetTool.Service
{
    public class GrpcGenerateService
    {
        private readonly GrpcOptionsPlus _options;

        public GrpcGenerateService(string protoFile,bool nugetPush)
            :this(new GrpcOptions { 
                NugetPush=nugetPush,
                ProtoFile=protoFile,
                ResourceProjectDir=Util.GetVSIXResourcePathCache()
            })
        {
        }
        public GrpcGenerateService(GrpcOptions options)
        {
            _options = new GrpcOptionsPlus()
            {
                ProtoFile = options.ProtoFile,
                ResourceProjectDir = options.ResourceProjectDir,
                NugetPush=options.NugetPush
            };

            var text = File.ReadAllText(options.ProtoFile);
            var packageName = Regex.Match(text, "package\\s*\"(\\S+?)\";");
            if (packageName.Success)
            {
                _options.GrpcNamespaceName = packageName.Groups[1].Value;
            }
            else
            {
                var m = Regex.Match(text, "csharp_namespace\\s*=\\s*\"(\\S+?)\";");
                if (m.Success)
                    _options.GrpcNamespaceName = m.Groups[1].Value;
                else
                    throw new Exception("namespace is null");
            }

            var m2 = Regex.Match(text, "service\\s*(\\S+?)\\s*{");
            if (m2.Success)
            {
                _options.ServiceName = m2.Groups[1].Value;
                _options.GrpcServiceClassName = $"{_options.ServiceName}.{_options.ServiceName}Client";
            }

            var mSubdirectory= Regex.Match(text, "FWOPTION_Subdirectory\\s*=\\s*\"(\\S+?)\";");
            if(mSubdirectory.Success)
                _options.GrpcServiceSubdirectory=mSubdirectory.Groups[1].Value;
        }

        public void Run(bool isOpen = false)
        {
            Generate();
            Build();

            if (_options.NugetPush)
                NugetPush();

            if (isOpen)
                Util.OpenFolder(_options.NewProjectDir);
            
        }

        private void Generate()
        {
            if (!Directory.Exists(_options.NewProjectDir))
                Directory.CreateDirectory(_options.NewProjectDir);

            var newProtoDir = Path.Combine(_options.NewProjectDir, "Protos");
            Directory.CreateDirectory(newProtoDir);

            File.Copy(_options.ProtoFile, Path.Combine(newProtoDir, _options.ProtoFileName));

            var files=Directory.GetFiles(_options.ResourceProjectDir);
            foreach(var f in files)
            {
                var text = File.ReadAllText(f)
                    .Replace("$namespacename$", _options.GrpcNamespaceName)
                    .Replace("$clientname$", _options.GrpcServiceClassName)
                    .Replace("$extensionname$", _options.ServiceName)
                    .Replace("$protoname$", _options.ProtoFileName)
                    .Replace("$versionname$", _options.Version)
                    .Replace("$subdirectoryHandlersubpath$", _options.GrpcServiceSubdirectory);

                string newFileName = Path.GetFileName(f);
                string newFilePath = Path.Combine(_options.NewProjectDir,newFileName=="Temp.csproj"? $"{_options.GrpcNamespaceName}.csproj" : newFileName);
                File.WriteAllText(newFilePath,text);
            }
        }

        private void Build()
        {
            string message = Util.CmdRunAndReturn("dotnet restore -s http://nuget.hzfanews.fw/nuget -s https://api.nuget.org/v3/index.json", _options.NewProjectDir);
            Util.CheckCmdMessageThrewException(message);

            message = Util.CmdRunAndReturn("dotnet build -c Release", _options.NewProjectDir);
            Util.CheckCmdMessageThrewException(message);
        }

        private void NugetPush()
        {
            string message = Util.CmdRunAndReturn("dotnet pack -c Release", _options.NewProjectDir);
            Util.CheckCmdMessageThrewException(message);

            var nugetpackDir = Path.Combine(_options.NewProjectDir, "bin\\Release");
            var nugetpackName = $"{_options.GrpcNamespaceName}.{_options.Version}.nupkg";
            message = Util.CmdRunAndReturn($"dotnet nuget push {nugetpackName} -s http://nuget.hzfanews.fw/nuget -k fanews@2018nuget", nugetpackDir);
            Util.CheckCmdMessageThrewException(message);
        }
    }
}
