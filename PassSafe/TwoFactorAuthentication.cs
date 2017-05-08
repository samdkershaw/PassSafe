using System;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace PassSafe
{
    public class TwoFactorAuthentication
    {
        //Set up the variables to be used in this class.
        string emailAddress;
        string firstName;
        string _twoFactorCode;
        public string TwoFactorCode
        {
            get { return _twoFactorCode; }
            private set
            {
                _twoFactorCode = value;
            }
        }

        //Constructor:
        // This will generate the 2FA code and will also send the
        //  email containing the code to the user.
        public TwoFactorAuthentication(string _emailAddress, string _firstName)
        {
            this.emailAddress = _emailAddress;
            this.firstName = _firstName;
            this.TwoFactorCode = GenerateRandomCode(); //A random 6-digit code is generated.
            this.SendEmail(this.CreateEmailBody()); //We send the personalised email to the user.
        }

        //This method creates a personalised body for the email, with the 2FA code and
        // users first name.
        private string CreateEmailBody()
        {
            string body = @"<!doctype html>
<body>
<h1>PassSafe</h1>
<h2>Hey {firstName},</h2>
<p>The code you requested to enter your password vault is below:</p>
<span style=""font-weight:600;"">{twoFactorCode}</span>
<p>Have a nice day!</p>
<h3>PassSafe Bot</h3>
</body>
";
            //This replaces parts of the template to make the email
            // more personal.
            body = body.Replace("{firstName}", this.firstName);
            body = body.Replace("{twoFactorCode}", this.TwoFactorCode);
            return body;
        }

        //The email is sent asynchronously as to avoid blocking the UI thread.
        private async void SendEmail(string body)
        {
            try
            {
                //Set up the email message.
                MailAddress from = new MailAddress("passsafe@samdkershaw.com", "PassSafe");
                MailAddress to = new MailAddress(this.emailAddress);
                MailMessage msg = new MailMessage(from, to);

                msg.Subject = "PassSafe Auth Code";
                msg.SubjectEncoding = Encoding.UTF8;

                msg.Body = body;
                msg.BodyEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;

                //The using statement ensures that the SMTP Client is disposed of after use.
                using (var client = new SmtpClient("mail.samdkershaw.com"))
                {
                    client.UseDefaultCredentials = false;
                    //Set the email server credentials.
                    NetworkCredential authInfo = new NetworkCredential("passsafe@samdkershaw.com", "Hellomum42");
                    client.Credentials = authInfo;
                    //The email is sent asynchronously.
                    await client.SendMailAsync(msg);
                }
            }
            catch (Exception e)
            {
                //Catch any error and print to debug.
                Core.PrintDebug(e.Message);
            }
        }

        //This method generates a random 6-digit code.
        private string GenerateRandomCode()
        {
            Random rnd = new Random();
            int twoFactorCode = rnd.Next(111111, 999999);
            return twoFactorCode.ToString();
        }
    }
}