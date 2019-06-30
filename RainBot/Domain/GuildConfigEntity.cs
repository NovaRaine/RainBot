using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RainBot.Domain
{
    public class GuildConfigEntity
    {
        [Key]
        public string GuildId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
