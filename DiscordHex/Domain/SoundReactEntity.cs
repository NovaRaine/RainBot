using DiscordHex.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordHex.Domain
{
    public class SoundReactEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public SoundReactTypeEnum Type { get; set; }
    }
}
