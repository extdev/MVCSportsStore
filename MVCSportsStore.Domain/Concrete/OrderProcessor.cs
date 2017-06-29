using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using MVCSportsStore.Domain.Abstract;
using MVCSportsStore.Domain.Entities;

namespace MVCSportsStore.Domain.Concrete
{
    public class EmailOrderProcessor : IOrderProcessor
    {
        private readonly EmailSettings _emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            _emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = _emailSettings.UseSsl;
                smtpClient.Host = _emailSettings.ServerName;
                smtpClient.Port = _emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_emailSettings.Username,
                _emailSettings.Password);
                if (_emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                    = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = _emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder()
                .AppendLine("A new order has been submitted")
                .AppendLine("---")
                .AppendLine("Items:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity,line.Product.Name,subtotal);
                }

                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
                .AppendLine("---")
                .AppendLine("Ship to:")
                .AppendLine(shippingDetails.Name)
                .AppendLine(shippingDetails.Line1)
                .AppendLine(shippingDetails.Line2 ?? "")
                .AppendLine(shippingDetails.Line3 ?? "")
                .AppendLine(shippingDetails.City)
                .AppendLine(shippingDetails.State ?? "")
                .AppendLine(shippingDetails.Country)
                .AppendLine(shippingDetails.Zip)
                .AppendLine("---")
                .AppendFormat("Gift wrap: {0}",
                shippingDetails.GiftWrap ? "Yes" : "No");
                MailMessage mailMessage = new MailMessage(
                _emailSettings.MailFromAddress, // From
                _emailSettings.MailToAddress, // To
                "New order submitted!", // Subject
                body.ToString()); // Body

                if (_emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }

                smtpClient.Send(mailMessage);
            }
        }
    }
}