
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
        are,
        good,
        bad,
        morning,
        evening,
        night,
        define,
        who,
        what,
        you,
        nova,
        @do,
        tell,
        yourself,
        joke,
        bot,
        pat
    }

    public enum EventInput
    {
        Morning,
        Evening,
        Night,
        Positive,
        Negative
    }
}
