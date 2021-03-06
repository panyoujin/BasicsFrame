﻿using System;
using System.Runtime.Serialization;

namespace BF.Common.CustomException
{
    /// <summary>
    /// 未登录异常
    /// </summary>
    [Serializable]
    public class NotLoginException : Exception
    {
        public NotLoginException()
        {
        }
        public NotLoginException(string message)
            : base(message)
        {
        }
        public NotLoginException(string message, Exception inner)
            : base(message, inner)
        {
        }
        protected NotLoginException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
