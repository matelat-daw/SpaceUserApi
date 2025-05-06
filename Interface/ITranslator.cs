namespace SpaceUserAPI.Interface
{
    public interface ITranslator
    {
        public Task<string> TranslateText(string text, string language);
        public Task<string> TranslateHtml(string text, string language);
    }
}