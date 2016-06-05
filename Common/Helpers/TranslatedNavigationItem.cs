
namespace Common.Helpers
{   
    public class TranslatedNavigationItem
    {
        public TranslatedNavigationItem(string id, string text, string url)
        {
            Id = id;
            Text = text;
            URL = url;
        }

        public string Id { get { return _Id; } set { _Id = value; } }
        public string Text { get { return _Text; } set { _Text = value; } }
        public string URL { get { return _URL; } set { _URL = value; } }

        private string _Id;
        private string _Text;
        private string _URL;
    }
}
