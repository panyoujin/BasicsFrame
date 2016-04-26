using Newtonsoft.Json;

namespace BF.Common.Extensions
{
    public static class JsonExtends
    {
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }
        /// <summary>
        /// 将json格式的字符串，转换成模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToModel<T>(this string json)
        {
            return (T)JsonConvert.DeserializeObject(json);
        }
    }

}
