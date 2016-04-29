using System;
using System.Runtime.Serialization;

namespace BF.Common.CustomException
{
    /// <summary>
    /// 逻辑异常
    /// </summary>
    [Serializable]
    public class BusinessException : Exception
    {
        public BusinessException()
        {
        }
        public BusinessException(string message)
            : base(message)
        {
        }
        public BusinessException(string message, Exception inner)
            : base(message, inner)
        {
        }
        protected BusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
