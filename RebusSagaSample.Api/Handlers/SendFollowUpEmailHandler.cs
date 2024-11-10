using Rebus.Bus;
using Rebus.Handlers;
using RebusSagaSample.Api.Emails;
using RebusSagaSample.Api.Messages;

namespace RebusSagaSample.Api.Handlers;

public class SendFollowUpEmailHandler(IEmailService emailService, IBus bus) : IHandleMessages<SendFollowUpEmail>
{

    public async Task Handle(SendFollowUpEmail message)
    {
        await emailService.SendFollowUpEmailAsync(message.Email);

        await bus.Reply(new FollowUpEmailSent(message.Email));
    }
}