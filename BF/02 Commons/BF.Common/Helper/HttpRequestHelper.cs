using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BF.Common.Helper
{
    public class HttpRequestHelper
    {
        public static string Request(string _address, string method = "GET", int timeOut = 5, Dictionary<string, string> headers = null)
        {
            string resultJson = string.Empty;
            if (string.IsNullOrEmpty(_address))
                return resultJson;
            Uri address = new Uri(_address);

            // 创建网络请求  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            //添加请求头信息
            if (headers != null && headers.Count > 0)
            {
                request.Accept = "application/vnd.huantengsmart-v1+json";
                foreach (var key in headers.Keys)
                {
                    request.Headers.Add(key, headers[key]);
                }
            }
            // 构建Head
            request.Method = method;

            Encoding myEncoding = Encoding.GetEncoding("utf-8");
            request.Timeout = timeOut * 1000;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseStr = reader.ReadToEnd();
                if (responseStr != null && responseStr.Length > 0)
                {
                    resultJson = responseStr;
                }
            }
            return resultJson;
        }
    }
}
