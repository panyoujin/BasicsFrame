using System;
using System.Collections.Generic;

namespace BF.Common.CommonEntities
{
    /// <summary>
    /// 手机端接口返回结果(PC端不要调用)
    /// </summary>
    [Serializable]
    public class ApiResult
    {
        public string code { get; set; }
        public string msg { get; set; }
    }

    /// <summary>
    /// 手机端接口返回结果(PC端不要调用)
    /// </summary>
    [Serializable]
    public class ApiResult<T> : ApiResult
    {
        public T data { get; set; }
    }

    public class ResultData<T> : ApiResult
    {
        public Dictionary<string, T> data { get; set; }

        public void DataInit(string name, T t)
        {
            var dic = new Dictionary<string, T>();
            dic.Add(name, t);
            data = dic;
        }
    }
}
