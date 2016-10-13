using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;



namespace MOFFIS
{
    class correo
    {

        //public bool SendEmail_Act_Proc(string CompName, string CompTlf, string CompAdds, string KeyLog, ArrayList body)
        public bool SendEmail_Act_Proc(string CompName, string CompTlf, string CompAdds, string KeyLog)
        {

            string subject = "INSTALACION DE COMPLEMENTOS";

            SmtpClient smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("atechnology507@gmail.com", "Business"),
                //Timeout = 3000
            };


            //Creamos la tabla
            string BodyHtml =
                              "<TABLE border=1 cellspacing=1 cellpadding=1>" +
                              "<CAPTION>Informacion de instalación - Complementos</CAPTION>" +
                              "<TR><TH><B>Compañia</TH><TH>ID Técnico</TH><TH>Modulo(s)</B></TH></TR>" +
                              "<TR><TD>" + CompName + "<BR>" + CompAdds + "<BR>" + CompTlf + "</TD><TD>" + KeyLog + "</TD><TD>";

            //if (body[0].Equals(true))
            //    BodyHtml = BodyHtml + " Informe 43 <BR>";
            //if (body[1].Equals(true))
            //    BodyHtml = BodyHtml + " Anexo 94 <BR>";
            //if (body[2].Equals(true))
            //    BodyHtml = BodyHtml + " ACH Empleados <BR>";
            //if (body[3].Equals(true))
            //    BodyHtml = BodyHtml + " ACH Proveedores <BR>";
            //if (body[4].Equals(true))
            //    BodyHtml = BodyHtml + " Sysmeca <BR>";
            //if (body[5].Equals(true))
            //    BodyHtml = BodyHtml + " Anexo 72 <BR>";
            BodyHtml = BodyHtml + "MOffis Estandar para HASAR";
            BodyHtml = BodyHtml + "</TD></TR></TABLE>";
            //Fin de la tabla

            // --string Bodymessage = CreateBodyMessage(row["id"].ToString());

            MailMessage message = new MailMessage();
            message.From = new MailAddress("atechnology507@gmail.com");
            message.To.Add("lbolanos@bsc.com.pa");//roxyfiore23@gmail.com lbolanos@bsc.com.pa amaylin29@gmail.com
            message.Subject = subject;
            //message.Attachments.Add(new Attachment(@"C:\\AP_14_DGI_Compra_Bien_Serv.txt"));
            message.Body = BodyHtml;
            message.IsBodyHtml = true;
            smtp.Send(message);
            message = null;

            return true;
        }

    }
}
