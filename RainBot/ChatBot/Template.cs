namespace RainBot.ChatBot
{
    public class Template
    {
        public string Pattern { get; set; }
        public string Response { get; set; }
        public TemplateExecutionEnum Execute { get; set; }
    }
}