using BF.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BF.Common.FileProcess
{
    public class FileProcessHelp
    {
        public static Dictionary<string, object> Save(HttpPostedFile file, AttmntServer attServer)
        {
            if (null == file)
            {
                throw new Exception("请选择要上传的文件");
            }
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var fileExt = Path.GetExtension(file.FileName);
            var fileSize = file.ContentLength;
            var localName = Guid.NewGuid().ToString() + fileExt;

            var timePath = string.Format(@"{0}/{1}/{2}/", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var relativePath = attServer.RelativePath.TrimStart('~') + timePath;
            var remotePath = string.Format(@"\\{0}\{1}\{2}", attServer.ServerIP, attServer.RemoteFolder, timePath.Replace('/', '\\'));

            var fileFullPath = remotePath + localName;
            var relativeUrl = relativePath + localName;

            //using (var scope = new IdentityScope(attServer.ServerDomain, attServer.ServerAccount, attServer.ServerPassword, LogonType.NewCredentials, LogonProvider.Default))
            //{
                if (!Directory.Exists(remotePath))
                    Directory.CreateDirectory(remotePath);
                file.SaveAs(fileFullPath);
            //}
            #region 组装参数
                Dictionary<string, object> paramDic = new Dictionary<string, object>();

            if (!paramDic.ContainsKey("FileName"))
            {
                paramDic.Add("FileName", fileName);
            }
            else
            {
                paramDic["FileName"] = fileName;
            }
            if (!paramDic.ContainsKey("FileExtName"))
            {
                paramDic.Add("FileExtName", fileExt);
            }
            else
            {
                paramDic["FileExtName"] = fileExt;
            }
            if (!paramDic.ContainsKey("FileSize"))
            {
                paramDic.Add("FileSize", fileSize.ToString());
            }
            else
            {
                paramDic["FileSize"] = fileSize.ToString();
            }
            if (!paramDic.ContainsKey("LocalName"))
            {
                paramDic.Add("LocalName", localName);
            }
            else
            {
                paramDic["LocalName"] = localName;
            }
            if (!paramDic.ContainsKey("AttachmentUrl"))
            {
                paramDic.Add("AttachmentUrl", relativeUrl);
            }
            else
            {
                paramDic["AttachmentUrl"] = relativeUrl;
            }
            if (!paramDic.ContainsKey("ServerID"))
            {
                paramDic.Add("ServerID", attServer.ID.ToString());
            }
            else
            {
                paramDic["ServerID"] = attServer.ID.ToString();
            }
            #endregion 组装参数
            return paramDic;
        }
    }
}
