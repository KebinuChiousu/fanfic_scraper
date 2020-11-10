namespace Web
{
public struct Url
    {
        public string Scheme;
        public string Host;
        public int Port;
        public string Uri;
        public QueryString[] Query;
    }
}