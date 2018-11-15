using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordHex.Domain
{
    public class SettingEntity
    {
        public int Guid { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
