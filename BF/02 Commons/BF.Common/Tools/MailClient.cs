using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BF.Common.Tools
{
    public class MailClient
    {
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="mailServer">邮件服务器地址</param>
        /// <param name="loginName">用户名</param>
        /// <param name="loginPassword">密码</param>
        /// <param name="fromMail">发件人Email地址</param>
        /// <param name="toMail">收件人Email地址列表</param>
        /// <param name="subject">主题</param>
        /// <param name="content">内容</param>
        public static void SendMail(string mailServer, string loginName, string loginPassword, string fromMail, List<string> toMail, string subject, string content)
        {
            if (toMail.Count < 1)
                throw new ArgumentException("收件人不能为空！");

            SmtpClient client = new SmtpClient(mailServer);
            client.Credentials = new NetworkCredential(loginName, loginPassword);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Port = 25;

            MailAddress from = new MailAddress(fromMail, fromMail, System.Text.Encoding.Default);
            MailAddress to = new MailAddress(toMail[0]);
            MailMessage message = new MailMessage(from, to);

            for (int i = 1; i < toMail.Count; i++)
            {
                message.To.Add(toMail[i]);
            }
            message.Body = content;
            message.BodyEncoding = System.Text.Encoding.Default;
            message.Subject = subject;
            message.SubjectEncoding = System.Text.Encoding.Default;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                message.Dispose();
            }
        }

        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="mailServer">邮件服务器地址</param>
        /// <param name="loginName">用户名</param>
        /// <param name="loginPassword">密码</param>
        /// <param name="fromMail">发件人Email地址</param>
        /// <param name="toMail">收件人Email地址列表</param>
        /// <param name="subject">主题</param>
        /// <param name="content">内容</param>
        /// <param name="cc">抄送人Email地址列表</param>
        public static void SendMail(string mailServer, string loginName, string loginPassword, string fromMail, List<string> toMail, string subject, string content, List<string> cc)
        {
            if (toMail.Count < 1)
                throw new ArgumentException("收件人不能为空！");

            SmtpClient client = new SmtpClient(mailServer);
            client.Credentials = new NetworkCredential(loginName, loginPassword);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Port = 25;

            MailAddress from = new MailAddress(fromMail, fromMail, System.Text.Encoding.Default);
            MailAddress to = new MailAddress(toMail[0]);
            MailMessage message = new MailMessage(from, to);

            for (int i = 1; i < toMail.Count; i++)
            {
                message.To.Add(toMail[i]);
            }
            for (int i = 0; i < cc.Count; i++)
            {
                message.CC.Add(cc[i]);
            }
            message.Body = content;
            message.BodyEncoding = System.Text.Encoding.Default;
            message.Subject = subject;
            message.SubjectEncoding = System.Text.Encoding.Default;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                message.Dispose();
            }
        }

        public async static Task<bool> SendMail(MailContract contract)
        {
            bool flag = false;
            if (contract.To.Count < 1)
                throw new ArgumentException("收件人不能为空！");

            SmtpClient client = new SmtpClient(contract.Server);
            client.Credentials = new NetworkCredential(contract.Account, contract.Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Port = 25;

            MailAddress from = new MailAddress(contract.From, contract.FromName, System.Text.Encoding.Default);
            MailAddress to = new MailAddress(contract.To[0]);
            MailMessage message = new MailMessage(from, to);

            for (int i = 1; i < contract.To.Count; i++)
            {
                message.To.Add(contract.To[i]);
            }
            if (contract.CC != null && contract.CC.Count > 0)
            {
                for (int i = 0; i < contract.CC.Count; i++)
                {
                    message.CC.Add(contract.CC[i]);
                }
            }

            message.Body = contract.Content;
            message.BodyEncoding = System.Text.Encoding.Default;
            message.Subject = contract.Subject;
            message.SubjectEncoding = System.Text.Encoding.Default;
            //失败时发送回执
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            SendCompletedEventHandler handler = null;
            try
            {
                client.SendCompleted += handler = (s, e) =>
                {
                    client.SendCompleted -= handler;
                    handler = null;

                    if (e.Cancelled || null != e.Error)
                    {
                        flag = false;
                    }
                    else if (null == e.Error)
                        flag = true;
                };
                //client.Send(message);
                await client.SendMailAsync(message);
                flag = true;

            }
            catch (SmtpException ex)
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                message.Dispose();
                client.Dispose();
            }
            return flag;
        }

        public async static Task<bool> SendMail(MailContract contract, bool? isBodyHtml)
        {
            bool flag = false;
            if (contract.To.Count < 1)
                throw new ArgumentException("收件人不能为空！");

            SmtpClient client = new SmtpClient(contract.Server);
            client.Credentials = new NetworkCredential(contract.Account, contract.Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Port = 25;

            MailAddress from = new MailAddress(contract.From, contract.FromName, System.Text.Encoding.Default);
            MailAddress to = new MailAddress(contract.To[0]);
            MailMessage message = new MailMessage(from, to);

            for (int i = 1; i < contract.To.Count; i++)
            {
                message.To.Add(contract.To[i]);
            }
            if (contract.CC != null && contract.CC.Count > 0)
            {
                for (int i = 0; i < contract.CC.Count; i++)
                {
                    message.CC.Add(contract.CC[i]);
                }
            }

            message.Body = contract.Content;
            message.BodyEncoding = System.Text.Encoding.Default;
            message.Subject = contract.Subject;
            message.SubjectEncoding = System.Text.Encoding.Default;
            if (isBodyHtml != null)
                message.IsBodyHtml = isBodyHtml ?? false;

            //失败时发送回执
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            SendCompletedEventHandler handler = null;
            try
            {
                client.SendCompleted += handler = (s, e) =>
                {
                    client.SendCompleted -= handler;
                    handler = null;

                    if (e.Cancelled || null != e.Error)
                    {
                        flag = false;

                    }
                    else if (null == e.Error)
                        flag = true;
                };
                //client.Send(message);
                await client.SendMailAsync(message);
                flag = true;

            }
            catch (SmtpException ex)
            {

            }
            catch (Exception ex)
            {

            }
            finally
            {
                message.Dispose();
                client.Dispose();
            }
            return flag;
        }




    }
    public class MailContract
    {
        public string Server { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public List<string> CC { get; set; }
        public string ReplyTo { get; set; }
        public string WebSite { get; set; }
    }
}
