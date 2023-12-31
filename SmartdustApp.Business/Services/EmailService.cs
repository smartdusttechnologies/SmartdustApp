﻿using Microsoft.Extensions.Configuration;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;
using System.Net;
using System.Net.Mail;
using TestingAndCalibrationLabs.Business.Core.Interfaces;

namespace TestingAndCalibrationLabs.Business.Services

{
    /// <summary>
    /// This service is exclusively to send the email.
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// It is to access the Appsetting.json file.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor call.
        /// </summary>
        /// <param name="configuration"></param>
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>
        /// Sends mail using the Email model.
        /// </summary>
        /// <param name="surveyModel"></param>
        /// <returns></returns>
        public RequestResult<bool> Sendemail(EmailModel emailModel)
        {
            try
            {
                //Read SMTP settings from AppSettings.json.
                string host = _configuration["Smtp:Server"];
                int port = int.Parse(_configuration["Smtp:Port"]);
                string fromAddress = _configuration["Smtp:FromAddress"];
                string userName = _configuration["Smtp:UserName"];
                string linkpre = _configuration["GoogleCloudStorage:PrefixLink"];
                string password = _configuration["Smtp:Password"];
                string emailTo = string.Join(",", emailModel.Email);

                using (MailMessage mm = new MailMessage(fromAddress, emailTo))
                {
                    mm.Subject = emailModel.Subject;
                    mm.Body = emailModel.HtmlMsg;
                    mm.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = host;
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(userName, password);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = port;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(mm);
                        return new RequestResult<bool>(true);
                    }
                }
            }
            catch(Exception)
            {
                return new RequestResult<bool>(false);
            }                           
        }
    }
}    