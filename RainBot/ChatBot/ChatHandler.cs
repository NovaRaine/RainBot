using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using RainBot.Core;

namespace RainBot.ChatBot
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
            using (var sr = File.OpenText(BotConfig.GetValue("TemplatePath")))
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
                var template = GetTemplate(message);
                if (template != null && !string.IsNullOrEmpty(template.Pattern))
                    _chatServices.Execute(template);
            }
        }

        private Template GetTemplate(SocketUserMessage message)
        {
            var msg = RemoveSpecialCharacters(message.Content.ToLower());
            // Exact match
            var template =  _templates.FirstOrDefault(x => x.Pattern == msg);

            // Partial match, including wildcards
            if (template == null || string.IsNullOrEmpty(template.Pattern))
            {

            }

            return template;
        }

        private static string RemoveSpecialCharacters(string str)
        {
            return str.Replace("?", "").Replace("!", "").Replace(".", "").Replace(",", "").Replace("rainbot", "").Trim();
        }

        internal void GetHelp(SocketCommandContext context)
        {
            var helpTemplate = _templates.FirstOrDefault(x => x.Pattern == "help me");
            _chatServices.Context = context;
            _chatServices.Execute(helpTemplate);
        }

        #endregion Parsing comments
    }
}