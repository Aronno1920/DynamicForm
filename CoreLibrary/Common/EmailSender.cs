using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Configuration;
using System.Web;
using System.Reflection;

namespace CoreLibrary
{
    public static class EmailSender
    {
        public static Boolean Send(String EmailTo, String EmailSubject)
        {
            Boolean isSendSuccess = false;

            String SmtpOut = ConfigurationManager.AppSettings["SmtpOut"].ToString();
            Int32 SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
            String mailFromUser = ConfigurationManager.AppSettings["MailFrom"].ToString();
            String MailUserPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();

            try
            {
                StringBuilder sbEmailBody = new StringBuilder();
                sbEmailBody.Append("Dear Sir,<br/><br/>");
                sbEmailBody.Append("Document has been reverted.");
                sbEmailBody.Append("<br/><br/>");
                sbEmailBody.Append("<b>DMS Solution</b>");


                MailAddress mailfrom = new MailAddress(mailFromUser);
                MailAddress mailto = new MailAddress(EmailTo);
                MailMessage newmsg = new MailMessage(mailfrom, mailto);
                //if (txtEmailcc.Text != "")
                //{
                //    MailAddress mailcc = new MailAddress(txtEmailcc.Text);
                //    newmsg.CC.Add(mailcc);
                //}
                newmsg.IsBodyHtml = true;
                newmsg.Subject = EmailSubject;
                newmsg.Body = sbEmailBody.ToString();

                ////For File Attachment, more file can also be attached
                //Attachment att = new Attachment(Attachment);
                //newmsg.Attachments.Add(att);


                SmtpClient smtps = new SmtpClient(SmtpOut, SmtpPort);
                smtps.UseDefaultCredentials = false;
                smtps.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtps.Credentials = new System.Net.NetworkCredential(mailFromUser, MailUserPassword);
                smtps.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtps.Send(newmsg);

                isSendSuccess = true;
            }
            catch(Exception ex)
            {
                isSendSuccess = false;
                ErrorTracking.SaveError("", "EmailSender", System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }

            return isSendSuccess;
        }

        public static Boolean Send(String EmailTo, String EmailSubject, List<String> EmaillCCList)
        {
            Boolean isSendSuccess = false;

            String SmtpOut = ConfigurationManager.AppSettings["SmtpOut"].ToString();
            Int32 SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
            String mailFromUser = ConfigurationManager.AppSettings["MailFrom"].ToString();
            String MailUserPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();

            try
            {
                StringBuilder sbEmailBody = new StringBuilder();
                sbEmailBody.Append("Dear Ahmed,<br/><br/>");
                sbEmailBody.Append("Please click on the following link to reset your password");
                sbEmailBody.Append("<br/><br/>");
                sbEmailBody.Append("<b>DMS Solution</b>");

                MailAddress mailfrom = new MailAddress(mailFromUser);
                MailAddress mailto = new MailAddress(EmailTo);
                MailMessage newmsg = new MailMessage(mailfrom, mailto);

                if (EmaillCCList.Count>0)
                {
                    foreach (String cc_email in EmaillCCList)
                    {
                        MailAddress mailcc = new MailAddress(cc_email);
                        newmsg.CC.Add(mailcc);
                    }
                }
                newmsg.IsBodyHtml = true;
                newmsg.Subject = EmailSubject;
                newmsg.Body = sbEmailBody.ToString();

                SmtpClient smtps = new SmtpClient(SmtpOut, SmtpPort);
                smtps.UseDefaultCredentials = false;
                smtps.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtps.Credentials = new System.Net.NetworkCredential(mailFromUser, MailUserPassword);
                smtps.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtps.Send(newmsg);

                isSendSuccess = true;
            }
            catch(Exception ex)
            {
                isSendSuccess = false;
                ErrorTracking.SaveError("", "EmailSender", System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }

            return isSendSuccess;
        }
    }
}