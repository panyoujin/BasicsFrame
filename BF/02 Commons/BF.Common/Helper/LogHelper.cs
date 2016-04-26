using log4net;
using System;

namespace BF.Common.Helper
{
    public class LogHelper
    {
        //static readonly ILog log = LogManager.GetLogger(typeof(LogHelper));
        static readonly ILog log = LogManager.GetLogger("FileLogger");


        public static void Debug(object message)
        {
            log.Debug(message);
        }

        public static void Error(object message)
        {
            log.Error(message);
        }

        public static void Error(object message, Exception ex)
        {
            string msg = message as string;
            if (!string.IsNullOrEmpty(msg))
            {
                msg = GetOutputResult(msg, ex);
                log.Error(msg, null);
            }
            else
            {
                log.Error(message, ex);
            }

        }

        public static void Error(string modelType, string op, string message, Exception exception)
        {
            string temp = string.Format("模块名称:{0}\r\n操作：{1}\r\n消息：{2}\r\n", modelType, op, message);
            InternelError(temp, exception);
        }

        private static void InternelError(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public static void Warn(object message)
        {
            log.Warn(message);
        }


        public static void Info(object message)
        {
            if (message == null)
                return;

            log.Info(message);

        }

        public static void Info(object message, Exception exception)
        {
            log.Info(message, exception);
        }


        #region 日志输出到本地文件推荐方法


        private static string GetOutputResult(string title, string messege)
        {
            return string.Format("标题：{0}       内容：{1} ", title, messege);
        }
        private static string GetOutputResult(string title, Exception ex)
        {
            return string.Format("标题：{0}       异常信息：{1} ", title, ex.ToString());
        }

        #endregion

        public static string ConcatExceptionToString(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return ex.Message + "->" + ConcatExceptionToString(ex.InnerException);
            }
            else
            {
                return ex.Message;
            }

        }

    }


    [Serializable]
    public class LogInfo
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 模块类型
        /// </summary>
        public int ModType { get; set; }

        /// <summary>
        /// 模块类型
        /// </summary>
        public string ModTypeName { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 日志来源
        /// </summary>
        public int SourcesType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public int OperatorID { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        public int SucFlag { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperaTime { get; set; }


        public LogInfo()
        {
            this.Description = string.Empty;
        }

        //以下是新加的字段

        /// <summary>
        /// 请求地址(路由)
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// 日志状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 对结果的描述(成功和失败)
        /// </summary>
        public string ResultDes { get; set; }
    }
}
