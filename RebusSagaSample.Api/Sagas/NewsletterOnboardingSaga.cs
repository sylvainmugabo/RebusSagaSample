﻿using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using RebusSagaSample.Api.Messages;

namespace RebusSagaSample.Api.Sagas;

public class NewsletterOnboardingSaga(IBus bus) :
    Saga<NewsletterOnboardingSagaData>,
    IAmInitiatedBy<SubscribeToNewsletter>,
    IHandleMessages<WelcomeEmailSent>,
    IHandleMessages<FollowUpEmailSent>
{
    protected override void CorrelateMessages(ICorrelationConfig<NewsletterOnboardingSagaData> config)
    {
        config.Correlate<SubscribeToNewsletter>(m => m.Email, d => d.Email);

        config.Correlate<WelcomeEmailSent>(m => m.Email, d => d.Email);

        config.Correlate<FollowUpEmailSent>(m => m.Email, d => d.Email);
    }

    public async Task Handle(SubscribeToNewsletter message)
    {
        if (!IsNew)
        {
            return;
        }

        await bus.Send(new SendWelcomeEmail(message.Email));
    }

    public async Task Handle(WelcomeEmailSent message)
    {
        Data.WelcomeEmailSent = true;

        await bus.Defer(TimeSpan.FromSeconds(300), new SendFollowUpEmail(message.Email));
    }

    public Task Handle(FollowUpEmailSent message)
    {
        Data.FollowUpEmailSent = true;

        MarkAsComplete();

        return Task.CompletedTask;
    }
}

