using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiscordHex.ChatBot
{
    public class ChatHandler
    {
        private ChatServices _chatServices { get; set; }
        private List<Template> _templates = new List<Template>();
        private static readonly JsonSerializer JsonSerializer = new JsonSerializer();

        public ChatHandler(ChatServices chatServices)
        {
            _chatServices = chatServices;
            LoadTemplates();
        }

        private void LoadTemplates()
        {
            using (var sr = File.OpenText("c:\\rainbot\\templates.ai"))
            {
                using (var reader = new JsonTextReader(sr))
                {
                    _templates = JsonSerializer.Deserialize<List<Template>>(reader);
                }
            }
        }

        #region Parsing comments
        public void HandleChatMessage(SocketCommandContext context)
        {
            _chatServices.Context = context;

            var message = context.Message;

            if (message.Content.ToLower().Contains("rainbot"))
            {
                var template = _templates.FirstOrDefault(x => x.Pattern == RemoveSpecialCharacters(message.Content.ToLower()));
                if (template != null)
                    _chatServices.Execute(template);
            }
        }

        private static string RemoveSpecialCharacters(string str)
        {
            return str.Replace("?", "").Replace("!", "").Replace(".", "").Replace(",", "").Replace("rainbot", "").Trim();
        }

        #endregion
    }
}
