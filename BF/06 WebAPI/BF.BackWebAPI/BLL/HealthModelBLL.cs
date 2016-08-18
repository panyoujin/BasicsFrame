using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF.BackWebAPI.BLL
{
    public class HealthModelBLL
    {
        public static string GetIntroduceByParam(string param)
        {
            var introduce = new StringBuilder();
            var index = 1;
            try
            {
                if (!string.IsNullOrWhiteSpace(param))
                {
                    var byteList = param.Split('|');
                    for (var i = 0; i < byteList.Count(); i++)
                    {
                        var array = byteList[i].Split(',');
                        var p1 = 0;
                        var p2 = 0;
                        int.TryParse(array[0], out p1);
                        int.TryParse(array[1], out p2);
                        switch (i)
                        {
                            case 0:
                                if (p1 != 0 && p2 != 0)
                                {
                                    introduce.AppendFormat("{0}. 以{1}W的功率煮水到{2}度； \n", index++, p1 * 10, p2);
                                }
                                else if (p1 != 0 && p2 == 0)
                                {
                                    introduce.AppendFormat("{0}. 以{1}W的功率煮水； \n", index++, p1);
                                }
                                else if (p1 == 0 && p2 != 0)
                                {
                                    introduce.AppendFormat("{0}. 煮水到{1}度； \n", index++, p2);
                                }
                                break;
                            case 1:
                                if (p1 != 0 && p2 != 0)
                                {
                                    introduce.AppendFormat("{0}. 以{1}W的功率煮水到{2}度； \n", index++, p1 * 10, p2);
                                }
                                else if (p1 != 0 && p2 == 0)
                                {
                                    introduce.AppendFormat("{0}. 以{1}W的功率煮水； \n", index++, p1);
                                }
                                else if (p1 == 0 && p2 != 0)
                                {
                                    introduce.AppendFormat("{0}. 煮水到{1}度； \n", index++, p2);
                                }
                                break;
                            case 2:
                                if (p1 != 0 && p2 != 0)
                                {
                                    introduce.AppendFormat("{0}. {1}度后加料并浸泡{2}分钟； \n", index++, p1, p2);
                                }
                                else if (p1 != 0 && p2 == 0)
                                {
                                    introduce.AppendFormat("{0}. {1}度后加料； \n", index++, p1);
                                }
                                else if (p1 == 0 && p2 != 0)
                                {
                                    introduce.AppendFormat("{0}. 浸泡{1}分钟； \n", index++, p2);
                                }
                                break;
                            case 3:
                                if (p1 != 0 && p2 != 0)
                                {
                                    introduce.AppendFormat("{0}. 以{1}W的功率煮水{2}分钟； \n", index++, p1 * 10, p2);
                                }
                                else if (p1 != 0 && p2 == 0)
                                {
                                    introduce.AppendFormat("{0}. 以{1}W的功率煮水； \n", index++, p1);
                                }
                                else if (p1 == 0 && p2 != 0)
                                {
                                    introduce.AppendFormat("{0}. 煮水{1}分钟； \n", index++, p2);
                                }
                                break;
                            case 4:
                                if (p1 != 0 && p2 != 0)
                                {
                                    introduce.AppendFormat("{0}. 以{1}W的功率煮水{2}分钟； \n", index++, p1 * 10, p2);
                                }
                                else if (p1 != 0 && p2 == 0)
                                {
                                    introduce.AppendFormat("{0}. 以{1}W的功率煮水； \n", index++, p1);
                                }
                                else if (p1 == 0 && p2 != 0)
                                {
                                    introduce.AppendFormat("{0}. 煮水{1}分钟； \n", index++, p2);
                                }
                                break;
                            case 5:
                                if (p1 != 0 && p2 != 0)
                                {
                                    introduce.AppendFormat("{0}. 以{1}度保温{2}小时； \n", index++, p1, p2);
                                }
                                else if (p1 != 0 && p2 == 0)
                                {
                                    introduce.AppendFormat("{0}. 以{1}度保温； \n", index++, p1);
                                }
                                else if (p1 == 0 && p2 != 0)
                                {
                                    introduce.AppendFormat("{0}. 保温{1}小时； \n", index++, p2);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return introduce.ToString();
        }
    }
}
