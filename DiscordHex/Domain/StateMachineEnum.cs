
namespace DiscordHex.Domain
{
    public enum States
    {
        Start,
        Positive,
        Negative,
        Question
    }

    public enum Events
    {
        good,
        bad,
        morning,
        evening,
        bot,
        pat,
        @do,
        love
    }

    public enum EventInput
    {
        Morning,
        Evening,
        Positive,
        Negative,
        Love
    }
}
