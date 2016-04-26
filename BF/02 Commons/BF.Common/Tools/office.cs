using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;

namespace BF.Common.Tools
{
    public class Office
    {
        public static byte[] ExportToExcel<dynamic>(IList<dynamic> entityList, string[] tiltes, string fileName, XlFileFormat fileFormat, string extension)
        {
            byte[] bytes = null;
            if (null == entityList || 0 == entityList.Count) return bytes;
            Application xlApp = null;
            string diskPath = HttpContext.Current.Server.MapPath("~/App_Data/");
            string savePath = null;

            try
            {
                xlApp = new Application();
                xlApp.DisplayAlerts = false;
                Workbooks workbooks = xlApp.Workbooks;
                Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                Worksheet worksheet = (Worksheet)workbook.Worksheets[1];//取得sheet1
                worksheet.Name = "Sheet1";
                Range range = null;
                long totalCount = entityList.Count;
                long rowRead = 0;
                float percent = 0;

                //写入标题
                for (int i = 0; i < tiltes.Length; i++)
                {
                    worksheet.Cells[1, i + 1] = tiltes.Length - 1 >= i ? tiltes[i] : entityList[0].GetType().GetProperties()[i].Name;
                    range = (Range)worksheet.Cells[1, i + 1];
                    range.Font.Bold = true;//粗体
                    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;//居中
                    range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, null);
                    range.EntireColumn.AutoFit();//自动调整列宽
                    range.EntireRow.AutoFit();//自动调整行高
                }

                //写入内容
                for (int r = 0; r < entityList.Count; r++)
                {
                    for (int i = 0; i < entityList[0].GetType().GetProperties().Length; i++)
                    {
                        object value = entityList[r].GetType().GetProperties()[i].GetValue(entityList[r], null);
                        value = null == value ? "" : value.ToString();
                        worksheet.Cells[r + 2, i + 1] = value;
                        range = (Range)worksheet.Cells[r + 2, i + 1];
                        range.Font.Size = 9;//字体大小
                        range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, null);
                        range.EntireColumn.AutoFit();//自动调整列宽
                    }
                    rowRead++;
                    percent = ((float)(100 * rowRead)) / totalCount;
                }
                range.Borders[XlBordersIndex.xlInsideHorizontal].Weight = XlBorderWeight.xlThin;
                if (entityList[0].GetType().GetProperties().Length > 1)
                {
                    range.Borders[XlBordersIndex.xlInsideVertical].Weight = XlBorderWeight.xlThin;
                }

                //避免多个用户同时导出文件造成文件覆盖，采用Guid来唯一区分
                savePath = string.Concat(diskPath, fileName, Guid.NewGuid(), extension);
                xlApp.ActiveWorkbook.SaveAs(savePath, fileFormat, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                workbooks.Close();
            }
            catch (System.Exception ex)
            {
                // Logger.WriteMessage(" Common.Ext.Office的ExportToExcel方法异常错误：" + ex.ToString());
                throw ex;
            }
            finally
            {
                if (null != xlApp)
                {
                    xlApp.Quit();
                }
                if (System.IO.File.Exists(savePath))
                {
                    //存储文件到内存流，避免遗留文件在服务器
                    MemoryStream memoryStream = new MemoryStream();
                    BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
                    FileStream fileStream = new FileStream(savePath, FileMode.Open);
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    binaryWriter.Write(binaryReader.ReadBytes((int)fileStream.Length));
                    binaryReader.Close();
                    fileStream.Close();
                    binaryWriter.Close();
                    bytes = memoryStream.ToArray();
                    System.IO.File.Delete(savePath);
                    //HttpContext.Current.Session["response_bytes"] = bytes;
                    //HttpContext.Current.Session["response_fileName"] = HttpUtility.UrlEncode(fileName + extension);
                    //HttpContext.Current.Response.Write("OK");   //通知浏览器文件存放完成，可以执行二次响应的请求
                    //HttpContext.Current.Response.End();
                }
                else
                {
                    //HttpContext.Current.Response.Write("文件生成错误,请检查磁盘写入权限是否足够!");
                    //HttpContext.Current.Response.End();
                }
            }
            return bytes;
        }

        /// <summary>
        /// 导出函数[公用于Action]
        /// </summary>
        /// <typeparam name="T">要导出的模型类型</typeparam>
        /// <param name="listT">导出的泛型结果集</param>
        /// <param name="headerText">表头</param>
        /// <param name="fileName">文件名</param>
        public static byte[] outPut<dynamic>(IList<dynamic> listT, string[] headerText, string fileName)
        {
            string fileType = HttpContext.Current.Request["FileType"];
            fileType = fileType == null ? "" : fileType;
            switch (fileType.ToUpper().Trim())
            {
                case "HTML文件":
                    return Office.ExportToExcel<dynamic>(listT, headerText, fileName, XlFileFormat.xlHtml, ".html");

                case "CSV文件":
                    return Office.ExportToExcel<dynamic>(listT, headerText, fileName, XlFileFormat.xlCSV, ".csv");

                case "TEXT文件":
                    return Office.ExportToExcel<dynamic>(listT, headerText, fileName, XlFileFormat.xlUnicodeText, ".txt");

                case "XML文件":
                    return Office.ExportToExcel<dynamic>(listT, headerText, fileName, XlFileFormat.xlOpenXMLWorkbook, ".xml");

                default:  //excel
                    return Office.ExportToExcel<dynamic>(listT, headerText, fileName, XlFileFormat.xlWorkbookNormal, ".xls");
            }
        }

        public static string path = System.AppDomain.CurrentDomain.BaseDirectory + "App_Data\\";
        /// <summary>
        /// 返回byte[] 流
        /// </summary>
        /// <typeparam name="dynamic"></typeparam>
        /// <param name="listT"></param>
        /// <param name="headerText"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] WriteExcel<dynamic>(IList<dynamic> listT, string[] headerText, string fileName)
        {
            StreamWriter sw = null;
            byte[] bytes = null;
            FileStream fileStream = null;
            try
            {
                string path = Office.path + DateTime.Now.ToString("yyyMMddHHmmss") + ".xls";
                long totalCount = listT.Count;
                sw = new StreamWriter(path, false, Encoding.GetEncoding("gb2312"));
                StringBuilder sb = new StringBuilder();
                foreach (var item in headerText)
                {
                    sb.Append(item + "\t");
                }
                sb.Append(Environment.NewLine);

                PropertyInfo[] parinfo = null;
                if (listT.Count > 0)
                {
                    parinfo = listT[0].GetType().GetProperties();
                }
                for (int i = 0; i < listT.Count; i++)
                {
                    for (int j = 0; j < parinfo.Length; j++)
                    {
                        object value = parinfo[j].GetValue(listT[i], null);
                        value = value == null ? "" : value.ToString();
                        sb.Append(value + "\t");
                    }
                    sb.Append(Environment.NewLine);
                }
                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
                if (System.IO.File.Exists(path))
                {
                    //存储文件到内存流，避免遗留文件在服务器
                    MemoryStream memoryStream = new MemoryStream();
                    BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
                    fileStream = new FileStream(path, FileMode.Open);
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    binaryWriter.Write(binaryReader.ReadBytes((int)fileStream.Length));
                    binaryReader.Close();
                    fileStream.Close();
                    binaryWriter.Close();
                    bytes = memoryStream.ToArray();
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return bytes;
        }
        public static string WriteExcelFile<dynamic>(IList<dynamic> listT, string[] headerText, string fileName)
        {
            StreamWriter sw = null;
            //byte[] bytes = null;
            FileStream fileStream = null;
            string path = Office.path + "OutPutthisList.xls"; //DateTime.Now.ToString("yyyMMddHHmmss") + ".xls";
            try
            {

                long totalCount = listT.Count;
                sw = new StreamWriter(path, false, Encoding.GetEncoding("gb2312"));
                StringBuilder sb = new StringBuilder();
                foreach (var item in headerText)
                {
                    sb.Append(item + "\t");
                }
                sb.Append(Environment.NewLine);

                PropertyInfo[] parinfo = null;
                if (listT.Count > 0)
                {
                    parinfo = listT[0].GetType().GetProperties();
                }
                for (int i = 0; i < listT.Count; i++)
                {
                    for (int j = 0; j < parinfo.Length; j++)
                    {
                        object value = parinfo[j].GetValue(listT[i], null);
                        value = value == null ? "" : value.ToString();
                        sb.Append(value + "\t");
                    }
                    sb.Append(Environment.NewLine);
                }
                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
                if (System.IO.File.Exists(path))
                {
                    //存储文件到内存流，避免遗留文件在服务器
                    MemoryStream memoryStream = new MemoryStream();
                    BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
                    fileStream = new FileStream(path, FileMode.Open);
                    BinaryReader binaryReader = new BinaryReader(fileStream);
                    binaryWriter.Write(binaryReader.ReadBytes((int)fileStream.Length));
                    binaryReader.Close();
                    fileStream.Close();
                    binaryWriter.Close();
                    //bytes = memoryStream.ToArray();
                    //System.IO.File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            return path;
        }
    }
}
