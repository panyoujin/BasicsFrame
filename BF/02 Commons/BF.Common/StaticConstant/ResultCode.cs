using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.Common.StaticConstant
{
    public class ResultCode
    {
        /// <summary>
        /// 成功！200
        /// </summary>
        public static String CODE_SUCCESS = "200";
        

        /// <summary>
        /// 系统异常！500
        /// </summary>
        public static String CODE_EXCEPTION = "500";

        /// <summary>
        /// 业务逻辑错误！509
        /// </summary>
        public static String CODE_BUSINESS_ERROR = "509";

        /// <summary>
        /// 未登陆！600
        /// </summary>
        public static String CODE_ERROR_USER_NOT_LOGIN = "600";

        /// <summary>
        /// 更新失败！201
        /// </summary>
        public static String CODE_UPDATE_FAIL = "201";
    }

    public class ResultMsg
    {
        /// <summary>
        /// 成功！200
        /// </summary>
        public static String CODE_SUCCESS = "成功";

        /// <summary>
        /// 异常！500
        /// </summary>
        public static String CODE_EXCEPTION = "系统异常";

        /// <summary>
        /// 未登陆！600
        /// </summary>
        public static String CODE_ERROR_USER_NOT_LOGIN = "未登陆";

        /// <summary>
        /// 业务逻辑错误！509
        /// </summary>
        public static String CODE_BUSINESS_ERROR = "业务逻辑错误";
    }
}
