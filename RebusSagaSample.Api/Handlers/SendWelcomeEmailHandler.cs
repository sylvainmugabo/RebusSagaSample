using Rebus.Bus;
using Rebus.Handlers;
using RebusSagaSample.Api.Emails;
using RebusSagaSample.Api.Messages;

namespace RebusSagaSample.Api.Handlers;

public class SendWelcomeEmailHandler(IEmailService emailService, IBus bus) : IHandleMessages<SendWelcomeEmail>
{
    public async Task Handle(SendWelcomeEmail message)
    {
        await emailService.SendWelcomeEmailAsync(message.Email);

        await bus.Reply(new WelcomeEmailSent(message.Email));
    }
}