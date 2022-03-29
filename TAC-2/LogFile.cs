using Android.Util;
using MailKit.Security;
using MimeKit;
using System;
using System.IO;
using System.Text;

namespace TAC_2
{
    public class LogFile
    {
        public readonly string LOGFILE;

        public LogFile(string logFile)
        {
            LOGFILE = logFile;
        }

        public void ToLog(string str)
        {
            try
            {
                File.AppendAllText(LOGFILE, DateTime.Now.ToString("dd.MM.yy;HH:mm:ss;") + Environment.MachineName + ";" + str + ";" + "\n", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Log.Error("TAC-2", ex.Message);
            }
        }
        public bool ClearLog()
        {
            try
            {
                File.WriteAllText(LOGFILE, "");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("TAC-2", ex.Message);
            }
            return false;
        }

        public void SendMail(string Message, string mailto, string subject, string path = "")
        {
            var messageToSend = new MimeMessage();
            messageToSend.From.Add(new MailboxAddress("Лог ТАС-2", "psheni4niy@skalnyy.com"));
            messageToSend.To.Add(new MailboxAddress("vitaliy@skalnyy.com"));
            messageToSend.Subject = "Лог ТАС-2";
            var builder = new BodyBuilder { HtmlBody = "Log TAC-2" };
            builder.Attachments.Add(path);
            messageToSend.Body = builder.ToMessageBody();

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.MessageSent += (sender, args) => { };
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                try
                {
                    smtp.Connect("94.131.246.104", 25, SecureSocketOptions.StartTls);
                    smtp.Authenticate("psheni4niy@skalnyy.com", "987312Sv");
                    smtp.Send(messageToSend);
                    smtp.Disconnect(true);
                } catch (Exception ex)
                {
                    Log.Error("TAC-2", ex.Message);
                }
            };
        }
    }
}