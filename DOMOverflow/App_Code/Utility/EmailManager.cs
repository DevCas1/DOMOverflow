using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace DOMOverflow {
    public static class EmailManager {
        private const string CREDENTIAL_ADDRESS  = "DOMOverflow@gmail.com";
        private const string CREDENTIAL_PASSWORD = "xmtIftmBlQyVsh4lewTy";

        /// <summary>
        /// Send an email to the recipient address, with a given title and body.
        /// (Body should be in HTML format)
        /// </summary>
        /// <param name="recp">The recipient address.</param>
        /// <param name="title">The email title.</param>
        /// <param name="contents">The email contents.</param>
        public static void SendEmail(string recp, string title, List<string> contents) {
            MailMessage msg = new MailMessage(CREDENTIAL_ADDRESS, recp);
            msg.Subject = title;
            msg.Body = string.Join("", contents);
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = "smtp.gmail.com";

            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(CREDENTIAL_ADDRESS, CREDENTIAL_PASSWORD);

            client.Send(msg);
        }
    }
}