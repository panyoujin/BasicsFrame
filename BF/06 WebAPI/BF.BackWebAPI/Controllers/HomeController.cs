using BF.Common.CommonEntities;
using BF.Common.DataAccess;
using BF.Common.Helper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BF.BackWebAPI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [Authorize(Roles ="Users")]
        public ActionResult Index()
        {
            return View();
        }


        public string GetMenuList(string name="")
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("MenuName", string.Format("%{0}%", name));
            var dt = DBBaseFactory.DALBase.QueryForDataTable("BackWeb_GetMenuList", dic);
            ApiResult<object> api = new ApiResult<object>();
            api.code = "200";
            api.msg = "请求成功";
            api.data = dt;
            return JsonHelper.SerializeObject(api);
        }

        public string ApiTest(string url, int type = 1, string fileParam = "", string CookieP = "", string HeaderP = "", string ParamP = "")
        {
            try
            {
                RestClient client = new RestClient(url);
                RestRequest request = new RestRequest((Method)type);
                request.RequestFormat = DataFormat.Json;
                if (!string.IsNullOrWhiteSpace(ParamP))
                {
                    ParamP = ParamP.Trim();
                    foreach (var str in ParamP.Split(','))
                    {
                        var strarray = str.Split('|');
                        if (strarray.Length > 1)
                        {
                            request.AddParameter(strarray[0], strarray[1], ParameterType.GetOrPost);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(CookieP))
                {
                    CookieP = CookieP.Trim();
                    foreach (var str in CookieP.Split(','))
                    {
                        var strarray = str.Split('|');
                        if (strarray.Length > 1)
                        {
                            request.AddParameter(strarray[0], strarray[1], ParameterType.Cookie);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(HeaderP))
                {
                    HeaderP = HeaderP.Trim();
                    foreach (var str in HeaderP.Split(','))
                    {
                        var strarray = str.Split('|');
                        if (strarray.Length > 1)
                        {
                            request.AddParameter(strarray[0], strarray[1], ParameterType.HttpHeader);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(fileParam))
                {
                    fileParam = fileParam.Trim();
                    foreach (var str in fileParam.Split(','))
                    {
                        var strarray = str.Split('|');
                        if (strarray.Length > 1)
                        {
                            request.AddFile(strarray[0], strarray[1]);
                        }
                    }
                }
                IRestResponse response = client.Execute(request);
                if (response.Headers != null && response.Headers.Count > 0)
                {
                    foreach (var str in response.Headers)
                    {
                        if (str.Value.ToString().IndexOf("CACHED_SESSION_ID") >= 0)
                        {
                            return response.Content + str.Value.ToString();
                        }
                    }
                }

                return response.Content;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string PostTest(string url, string ParamP = "", int type = 1, string fileParam = "", string CookieP = "", string HeaderP = "")
        {
            try
            {
                ParamP = ParamP.Replace("|@|", "=");
                ParamP = ParamP.Replace("#@#", "&");
                byte[] postData = Encoding.UTF8.GetBytes(ParamP);
                url = HttpContext.Server.UrlDecode(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = "POST";
                //refer:http://www.cnblogs.com/cxd4321/archive/2012/01/30/2331621.html
                request.ServicePoint.Expect100Continue = false;
                //int timeout = 5000;
                //if (timeout < 5000 || timeout > 15000)
                //    timeout = 5000;
                //request.Timeout = timeout;
                request.ContentType = "application/x-www-form-urlencoded";
                if (!string.IsNullOrWhiteSpace(CookieP))
                {
                    foreach (var str in CookieP.Split(','))
                    {
                        var strarray = str.Split('|');
                        if (strarray.Length > 1)
                        {
                            Cookie cookie = new Cookie(strarray[0], strarray[1]);
                            request.CookieContainer = new CookieContainer();
                            request.CookieContainer.Add(new Uri("http://127.0.0.1"), cookie);
                            //request.CookieContainer.Add(cookie);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(HeaderP))
                {
                    foreach (var str in HeaderP.Split(','))
                    {
                        var strarray = str.Split('|');
                        if (strarray.Length > 1)
                        {
                            request.Headers.Add(strarray[0], strarray[1]);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(fileParam))
                {
                    foreach (var str in fileParam.Split(','))
                    {
                        var strarray = str.Split('|');
                        if (strarray.Length > 1 && !string.IsNullOrWhiteSpace(strarray[1]) && (strarray[1].LastIndexOf('.') + 1) < strarray[1].Length)
                        {
                            using (FileStream fileStream = new FileStream(strarray[1], FileMode.Open, FileAccess.Read))
                            {
                                byte[] buffer = new byte[0];

                                BinaryReader r = new BinaryReader(fileStream);

                                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开  

                                buffer = r.ReadBytes((int)r.BaseStream.Length);

                                postData = postData.Concat(buffer).ToArray();
                                request.ContentType = strarray[1].Substring(strarray[1].LastIndexOf('.') + 1);
                                request.Headers.Add("FileType", HttpContext.Server.UrlEncode(request.ContentType));
                                request.Headers.Add("FileSize", buffer.Length.ToString());
                                request.Headers.Add("FileName", Guid.NewGuid() + strarray[1].Substring(strarray[1].LastIndexOf('.')));
                            }
                        }
                    }
                }
                request.ContentLength = postData.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postData, 0, postData.Length);
                }
                using (MemoryStream content = new MemoryStream())
                {

                    using (WebResponse response = request.GetResponse())
                    {
                        string retString = string.Empty;
                        if (response.Headers != null && response.Headers.Count > 0)
                        {
                            foreach (string str in response.Headers)
                            {
                                if (response.Headers[str] != null && response.Headers[str].IndexOf("CACHED_SESSION_ID") >= 0)
                                {
                                    retString = response.Headers[str];
                                }
                            }
                        }
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                            retString = myStreamReader.ReadToEnd() + retString;
                            return retString;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}