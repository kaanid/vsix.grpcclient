using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcNugetTool.Service
{
    public class GrpcOptions
    {
        public string ProtoFile { set; get; }

        public string ResourceProjectDir { set; get; }
        /// <summary>
        /// 是否发布
        /// </summary>
        public bool NugetPush { set; get; }
    }

    public class GrpcOptionsPlus:GrpcOptions
    {
        public string WorkDir { set; get; }= $"{Path.GetTempPath()}grpc\\{DateTime.Now.Ticks}";
        public string ProtoFileName => Path.GetFileName(ProtoFile);
        public string GrpcNamespaceName { set; get; }
        public string GrpcServiceClassName { set; get; }
        public string GrpcServiceSubdirectory { set; get; } = string.Empty;

        private string serviceName;
        public string ServiceName
        {
            set
            {
                serviceName = value;
            }
            get
            {
                return serviceName ?? GrpcNamespaceName.Replace(".", "").Replace("Fanews.Grpc.", "");
            }
        }

        public string NewProjectDir
        {
            get
            {
                return $"{WorkDir}\\Project\\";
            }
        }

        public string Version { set; get; }= DateTime.Now.ToString("1.yyyy.M.dHH1");
    }
}
