using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.Common.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 格林标准时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long UnixDateTicks(this DateTime date)
        {
            DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (date.Ticks - dt.Ticks) / 10000000;
        }
    }
}
