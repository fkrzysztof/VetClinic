using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace VetClinic.Data.Helpers
{
    public class EMaill
    {
        public string AddressMail { get; set; } // adres mailowy z ktorego wysyłamy maila (vetclinicwsbnl2016@gmail.com)
        public string Password { get; set; } // jego hasło wsbnlu2016
        public string MessageText { get; set; } // treść maila
        public string MessageSubject { get; set; } // temat maila
        public string MessageTo { get; set; } // adres na jaki wysyłamy maila

        // adres mailowy kliniki: vetclinicwsbnl2016@gmail.com
        // haslo:wsbnlu2016
        public EMaill()
        {
            this.AddressMail = "vetclinicwsbnl2016@gmail.com";
            this.Password = "wsbnlu2016";
        }
        public EMaill(string to,string subject,string message):base ()

        {
            //this.AddressMail = "vetclinicwsbnl2016@gmail.com";
            //this.Password = "wsbnlu2016";
           
            MessageTo = to;
            MessageSubject = subject;
            MessageText = message;
        }
        public void send()
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(MessageTo));
                message.From = new MailAddress(AddressMail, "VetClinic");
                message.Subject = MessageSubject;
                message.Body = MessageText;
                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Credentials = new NetworkCredential(AddressMail, Password);
                    client.Send(message);
                }
            }
        }
    }
}
