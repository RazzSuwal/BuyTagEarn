﻿namespace SMM.DataAccessLayer.Services.IServices
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
