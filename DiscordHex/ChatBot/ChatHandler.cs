using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Xml;

namespace DiscordHex.ChatBot
{
    public class ChatHandler
    {
        private ChatServices _chatServices { get; set; }
        private List<Template> _templates { get; set; }

        public ChatHandler(ChatServices chatServices)
        {
            _chatServices = chatServices;
            LoadTemplates();
        }

        private void LoadTemplates()
        {
            XmlDocument templates = new XmlDocument();
            templates.Load("c:\rainbot\templates.ai");
            ProcessTemplate(templates);
        }

        private void ProcessTemplate(XmlDocument templates)
        {
            throw new NotImplementedException();
        }

        #region Parsing comments
        public void HandleChatMessage(SocketCommandContext context)
        {
            _chatServices.Context = context;

            var message = context.Message;

            if (message.Content.ToLower().Contains("rainbot"))
            {
                //ParseComment(RemoveSpecialCharacters(message.Content.ToLower()));
            }
        }

        private static string RemoveSpecialCharacters(string str)
        {
            return str.Replace("?", "").Replace("!", "").Replace(".", "").Replace(",", "");
        }

        #endregion
    }
}
