using Rebus.Sagas;

namespace RebusSagaSample.Api.Sagas;

public class NewsletterOnboardingSagaData : ISagaData
{
    public Guid Id { get; set; }
    public int Revision { get; set; }

    public string Email { get; set; }

    public bool WelcomeEmailSent { get; set; }

    public bool FollowUpEmailSent { get; set; }
}
