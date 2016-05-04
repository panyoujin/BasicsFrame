using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.Common.Models
{
    [Serializable]
    public class AttmntServer
    {
        public int ID { get; set; }
        /// <summary>
        /// 服务器主机名
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// 服务器主机IP
        /// </summary>
        public string ServerIP { get; set; }
        /// <summary>
        /// 服务器主机域名
        /// </summary>
        public string ServerDomain { get; set; }
        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativePath { get; set; }
        /// <summary>
        /// 远程文件夹路径
        /// </summary>
        public string RemoteFolder { get; set; }
        /// <summary>
        /// 远程文件夹访问账号
        /// </summary>
        public string ServerAccount { get; set; }
        /// <summary>
        /// 远程文件夹访问密码
        /// </summary>
        public string ServerPassword { get; set; }
    }
}
