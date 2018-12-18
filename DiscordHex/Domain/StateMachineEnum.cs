
namespace DiscordHex.Domain
{
    public enum States
    {
        Start,
        Positive,
        Negative
    }

    public enum Events
    {
        good,
        bad,
        morning,
        evening,
        bot,
        pat
    }

    public enum EventInput
    {
        Morning,
        Evening,
        Positive,
        Negative
    }
}
