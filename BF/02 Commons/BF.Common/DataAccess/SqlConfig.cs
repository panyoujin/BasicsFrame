using BF.Common.Helper;

namespace BF.Common.DataAccess
{
    public class SqlConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static string DBType
        {
            get
            {
                return ConfigHelper.GetConfigValue("DBType", "MySql");
                //return ConfigValue("DBType", "MySql");
            }
        }
        private static string _sqlConnStringName;
        public static string SqlConnStringName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sqlConnStringName))
                {
                    return "SqlConnString";
                }
                return _sqlConnStringName;
            }
            set
            {
                _sqlConnStringName = value;
            }
        }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings[SqlConnStringName].ConnectionString;
            }
        }
    }
}
