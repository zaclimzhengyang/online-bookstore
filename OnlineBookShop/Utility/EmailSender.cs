using Microsoft.AspNetCore.Identity.UI.Services;

namespace OnlineBookShop.Utility;

public class EmailSender : IEmailSender
{
    // Mock Implementation
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Task.CompletedTask;
    }
}