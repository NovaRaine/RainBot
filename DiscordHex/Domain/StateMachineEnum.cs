
namespace DiscordHex.Domain
{
    public enum States
    {
        Start,
        Positive,
        Negative,
        Who,
        What,
        Abilities
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
        something,
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
