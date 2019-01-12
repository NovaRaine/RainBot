
namespace DiscordHex.Domain
{
    public enum States
    {
        Start,
        Positive,
        Negative,
        Who,
        What,
        Abilities,
        Info
    }

    public enum Events
    {
        good,
        bad,
        morning,
        evening,
        night,
        define,
        who,
        what,
        you,
        @do,
        tell,
        yourself,
        joke,
        bot
    }

    public enum EventInput
    {
        Morning,
        Evening,
        Night,
        Positive,
        Negative,
        Love
    }
}
