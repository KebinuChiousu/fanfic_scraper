using System.IO;
using System.Xml;

namespace web_scraper.Models.Utility.XmlFiltering
{
  public class Reader : web_scraper.Models.Utility.XmlWrapping.Reader
    {
    public bool StripPrefix;

    public Reader(TextReader reader)
      : base(XmlReader.Create(reader))
      => this.StripPrefix = true;

    public Reader(TextReader reader, XmlReaderSettings settings)
      : base(XmlReader.Create(reader, settings))
      => this.StripPrefix = true;

    public Reader(string content)
      : base(XmlReader.Create((TextReader) new StringReader(content)))
      => this.StripPrefix = true;

    public Reader(string content, XmlReaderSettings settings)
      : base(XmlReader.Create((TextReader) new StringReader(content), settings))
      => this.StripPrefix = true;

    public override bool Read()
    {
      bool flag = base.Read();
      if (this.StripPrefix)
      {
        while (flag && this.NodeType == XmlNodeType.Element && this.Name.IndexOf(":") > 0)
          this.Skip();
      }
      return flag;
    }
  }
}
