

namespace BF.Common.DataAccess
{
    public class DBBaseFactory
    {
        private static readonly IDBBase _dalBase = GetDALBase();

        private static IDBBase GetDALBase()
        {
            IDBBase dalBase = null;
            switch (SqlConfig.DBType)
            {
                case "MySql":
                    dalBase = MySqlDBBase.Instance;
                    break;
            }
            return dalBase;
        }
        private DBBaseFactory()
        {

        }
        public static IDBBase DALBase
        {
            get
            {
                return _dalBase;
            }
        }
    }
}
