using BF.Common.Helper;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace BF.Common.Tools
{
    public class Upload
    {


        /// <summary>
        /// 上传文件方法
        /// </summary>
        /// <param name="myfile">上传按钮</param>
        /// <param name="size">允许上传文件的大小</param>
        /// <param name="foldername">文件保存到的文件夹</param>
        /// <param name="fileExt">允许上传文件的格式</param>
        /// <returns></returns>
        public static string UpLoadFile(HttpPostedFile myfile, int size, string foldername, string fileExt)
        {
            int fileSize = myfile.ContentLength;
            string getFileExt = GetFileExt(myfile.FileName.ToLower()).Trim();
            if (fileSize == 0)
            {
                return "1"; //上传文件为空请重新上传。
            }
            else if (Math.Round(Convert.ToDouble(fileSize / 1024), 2) > size)
            {
                return "2"; //上传文件大于允许上传的文件大小。
            }
            else if (!(fileExt.Contains(getFileExt)))
            {
                return "3"; //上传文件格式不正确。
            }
            else
            {
                try
                {
                    //文件名称
                    string strfileName = DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + CreateNum(5) + getFileExt;

                    string path = foldername + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day;
                    string allName = path + "\\" + strfileName;

                    //判断目录是否存在
                    FolderCreate(path);

                    //上传
                    myfile.SaveAs(allName);

                    //返回路径
                    return DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + strfileName;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }


        /// <summary>
        /// 上传文件方法
        /// </summary>
        /// <param name="myfile">上传按钮</param>
        /// <param name="size">允许上传文件的大小</param>
        /// <param name="foldername">文件保存到的文件夹</param>
        /// <param name="fileExt">允许上传文件的格式</param>
        /// <returns></returns>
        public static string UpLoadFile(FileUpload myfile, int size, string foldername, string fileExt)
        {
            int fileSize = myfile.PostedFile.ContentLength;
            string getFileExt = GetFileExt(myfile.PostedFile.FileName.ToLower()).Trim();
            if (fileSize == 0)
            {
                return "1"; //上传文件为空请重新上传。
            }
            else if (Math.Round(Convert.ToDouble(fileSize / 1024), 2) > size)
            {
                return "2"; //上传文件大于允许上传的文件大小。
            }
            else if (!(fileExt.Contains(getFileExt)))
            {
                return "3"; //上传文件格式不正确。
            }
            else
            {
                try
                {
                    //文件名称
                    string strfileName = DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + CreateNum(5) + getFileExt;

                    string path = foldername + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day;
                    string allName = path + "\\" + strfileName;

                    //判断目录是否存在
                    FolderCreate(path);

                    //上传
                    myfile.PostedFile.SaveAs(allName);

                    //返回路径
                    return DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + strfileName;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }


        /// <summary>
        /// 上传文件方法
        /// </summary>
        /// <param name="myfile">上传按钮</param>
        /// <param name="size">允许上传文件的大小</param>
        /// <param name="foldername">文件保存到的文件夹</param>
        /// <param name="fileExt">允许上传文件的格式</param>
        /// <returns></returns>
        public static string UpLoadFile(HttpPostedFileBase myfile, int size, string foldername, string fileExt)
        {

            int fileSize = myfile.ContentLength;
            string getFileExt = GetFileExt(myfile.FileName.ToLower()).Trim();
            if (fileSize == 0)
            {
                return "1"; //上传文件为空请重新上传。
            }
            else if (Math.Round(Convert.ToDouble(fileSize / 1024), 2) > size)
            {
                return "2"; //上传文件大于允许上传的文件大小。
            }
            else if (!(fileExt.Contains(getFileExt)))
            {
                return "3"; //上传文件格式不正确。
            }
            else
            {
                try
                {
                    //文件名称
                    string strfileName = DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + CreateNum(5) + getFileExt;
                    string allName = foldername + "\\" + strfileName;

                    //判断目录是否存在
                    FolderCreate(foldername);

                    //上传
                    myfile.SaveAs(allName);
                    var UploadImagePath = ConfigHelper.GetConfigValue("UploadImagePath", "D://");
                    //返回路径
                    return UploadImagePath.Substring(1, UploadImagePath.Length - 1) + strfileName;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public static void FolderCreate(string Path)
        {
            // 判断目标目录是否存在如果不存在则新建之   
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        /// <summary>
        /// 得到文件的后缀,以.gif或.**的格式出现
        /// </summary>
        /// <param name="myfile">文件名称</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetFileExt(string myfile)
        {
            if (string.IsNullOrEmpty(myfile))
                return string.Empty;

            int index = myfile.LastIndexOf(".");
            return myfile.Substring(index).ToLower();
        }

        /// <summary>
        /// 生成随机码
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CreateNum(int len)
        {
            Random random = new Random();
            int number;
            StringBuilder checkCode = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                number = random.Next(0, 9);
                checkCode.Append(number);
            }
            return checkCode.ToString();
        }


    }
}
