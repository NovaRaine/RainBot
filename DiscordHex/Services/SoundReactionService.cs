using Discord;
using System;

namespace DiscordHex.Services
{
    public class SoundReactionService
    {
        public EmbedBuilder GetTransSounds(string type)
        {
            var e = new EmbedBuilder();
            var transSounds = GetSoundsType<TransSounds>(type);

            switch (transSounds)
            {
                case TransSounds.ANGRY:
                    e.ImageUrl = "https://i.imgur.com/Wve21VD.jpg";
                    break;

                case TransSounds.BANHAMMER:
                    e.ImageUrl = "https://i.imgur.com/0yoFZfk.jpg";
                    break;

                case TransSounds.DISSAPOINTED:
                    e.ImageUrl = "https://i.imgur.com/ZNu0gvN.jpg";
                    break;

                case TransSounds.FESTIVE:
                    e.ImageUrl = "https://i.imgur.com/vJi1uIe.gif";
                    break;

                case TransSounds.LEWD:
                    e.ImageUrl = "https://i.imgur.com/Sl24o29.gif";
                    break;

                case TransSounds.PERFECT:
                    e.ImageUrl = "https://i.imgur.com/b2m8iJ0.jpg";
                    break;

                case TransSounds.SMUG:
                    e.ImageUrl = "https://i.imgur.com/CZAWHOs.jpg";
                    break;

                case TransSounds.SURPRISED:
                    e.ImageUrl = "https://i.imgur.com/vt6wvXs.jpg";
                    break;

                case TransSounds.TERRIFIED:
                    e.ImageUrl = "https://i.imgur.com/ajGUAqi.jpg";
                    break;

                case TransSounds.UNEASY:
                    e.ImageUrl = "https://i.imgur.com/qGcEiyd.jpg";
                    break;

                default:
                    return e;
            }

            return e;
        }

        public EmbedBuilder GetGaySounds(string type)
        {
            var e = new EmbedBuilder();
            var gaySounds = GetSoundsType<GaySounds>(type);

            switch (gaySounds)
            {
                case GaySounds.ANGRY:
                    e.ImageUrl = "https://i.imgur.com/m9HmpUU.jpg";
                    break;

                case GaySounds.CONFLICTED:
                    e.ImageUrl = "https://i.imgur.com/t5zHmVJ.jpg";
                    break;

                case GaySounds.CONFUSED:
                    e.ImageUrl = "https://i.imgur.com/1tqZsa6.jpg";
                    break;

                case GaySounds.DEVIOUS:
                    e.ImageUrl = "https://i.imgur.com/nVojgXA.jpg";
                    break;

                case GaySounds.DISMISSIVE:
                    e.ImageUrl = "https://i.imgur.com/jtF9otr.jpg";
                    break;

                case GaySounds.FESTIVE:
                    e.ImageUrl = "https://i.imgur.com/YtuMuUG.png";
                    break;

                case GaySounds.FLIRTY:
                    e.ImageUrl = "https://i.imgur.com/WMCPbFE.jpg";
                    break;

                case GaySounds.HAPPY:
                    e.ImageUrl = "https://i.imgur.com/LWZ43Ah.jpg";
                    break;

                case GaySounds.IMPERIAL:
                    e.ImageUrl = "https://i.imgur.com/ybOEIzM.jpg";
                    break;

                case GaySounds.IRRITATED:
                    e.ImageUrl = "https://i.imgur.com/ynAFapI.jpg";
                    break;

                case GaySounds.LEWD:
                    e.ImageUrl = "https://i.imgur.com/ytQkyLQ.jpg";
                    break;

                case GaySounds.NIGHTMARISH:
                    e.ImageUrl = "https://i.imgur.com/PQKRYud.jpg";
                    break;

                case GaySounds.SAD:
                    e.ImageUrl = "https://i.imgur.com/yuxwthJ.png";
                    break;

                case GaySounds.SILENCE:
                    e.ImageUrl = "https://i.imgur.com/HGlWIah.jpg";
                    break;

                case GaySounds.TERRIFIED:
                    e.ImageUrl = "https://i.imgur.com/S1VO2rp.png";
                    break;

                case GaySounds.TIRED:
                    e.Description = "Night night people! <3";
                    e.ImageUrl = "https://i.imgur.com/YKLbyCD.jpg";
                    break;
                    
                default:
                    return e;
            }

            return e;
        }

        private TEnum GetSoundsType<TEnum>(string type) where TEnum : struct
        {
            Enum.TryParse<TEnum>(type.ToUpper(), out var res);
            return res;
        }
    }

    public enum TransSounds
    {
        NONE,
        FESTIVE,
        LEWD,
        BANHAMMER,
        PERFECT,
        SMUG,
        DISSAPOINTED,
        ANGRY,
        UNEASY,
        SURPRISED,
        TERRIFIED
    }

    public enum GaySounds
    {
        NONE,
        HAPPY,
        CONFLICTED,
        ANGRY,
        SILENCE,
        DEVIOUS,
        LEWD,
        FLIRTY,
        NIGHTMARISH,
        TIRED,
        IMPERIAL,
        DISMISSIVE,
        CONFUSED,
        IRRITATED,
        FESTIVE,
        TERRIFIED,
        SAD
    }
}
