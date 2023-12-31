﻿using System.Threading.Tasks;

namespace KeyboardVN.Util.EmailSender
{
    public interface ISendMailService
    {
        Task SendMail(MailContent mailContent);

        Task SendMailWithoutImage(MailContent mailContent);

        Task SendEmailAsync(string email, string subject, string htmlMessage);

        Task SendEmailAsyncWithImage(string email, string subject, string htmlMessage, string ImagePath);
    }
}
