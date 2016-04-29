using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace BF.Common.Helper
{
    public static class JsonHelper
    {
        public static string DataTable2Json(System.Data.DataTable dt)
        {
            System.Text.StringBuilder jsonBuilder = new System.Text.StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.AppendFormat("\"total\":{0}, ", dt.Rows.Count);
            jsonBuilder.Append("\"rows\":[ ");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        /// <summary>
        /// 将集合转换为另一个对象集合.
        /// </summary>
        public static IEnumerable<TOut> Convert<TOut, TIn>(this IEnumerable<TIn> items, Func<TIn, TOut> handler)
        {
            if (items == null || handler == null)
                return null;
            IList<TOut> result = new List<TOut>();
            foreach (var item in items)
            {
                TOut value = handler.Invoke(item);
                if (value == null)
                    continue;
                result.Add(value);
            }
            return result;
        }

        public static string WriteFromObject<T>(T obj) where T : class, new()
        {
            //Create a stream to serialize the object to.
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(ms, obj);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);

        }

        public static string Serialize<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public static string Serialize<T>(IEnumerable<T> items)
        {
            if (items == null)
                return string.Empty;
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(IEnumerable<T>));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, items);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public static object Deserialize(Type type, string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                return null;
            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            object result = ser.ReadObject(ms);
            return result;
        }

        public static T Deserialize<T>(string jsonString)
        {
            var value = Deserialize(typeof(T), jsonString);
            if (value == null)
                return default(T);
            T obj = (T)value;
            return obj;
        }


        public static string SerializeObject(object t)
        {
            return JsonConvert.SerializeObject(t);
        }

        public static T DeserializeObject<T>(string json)
        {
            return (T)JsonConvert.DeserializeObject(json);
        }
        /// <summary>
        /// WebApi 返回josn使用，解决返回值带转义字符
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static HttpResponseMessage SerializeObjectToWebApi(object t)
        {
            //ObjectAttributeNullProcessingHelper.ObjectDefault(t);
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(JsonConvert.SerializeObject(t), Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

        
    }
}
