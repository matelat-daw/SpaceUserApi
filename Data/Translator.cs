using Google.Cloud.Translation.V2;
using SpaceUserAPI.Interface;

namespace SpaceUserAPI.Data
{
    public class Translator : ITranslator
    {
        private readonly TranslationClient client;
        public Translator()
        {
            client = TranslationClient.Create();
        }
        public Task<string> TranslateHtml(string text, string language)
        {
            var response = client.TranslateHtml(text, language);
            return Task.FromResult(response.TranslatedText);
        }
        public Task<string> TranslateText(string text, string language)
        {
            var response = client.TranslateText(text, language);
            return Task.FromResult(response.TranslatedText);
        }
    }
}