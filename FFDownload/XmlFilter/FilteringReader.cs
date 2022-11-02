using System.Xml;

namespace AspxToCode.Parser.Xml
{
  public class FilteringReader : WrappingReader
  {
    public bool StripPrefix;

    public FilteringReader(TextReader reader)
      : base(XmlReader.Create(reader))
    {
      this.StripPrefix = true;
    }

    public FilteringReader(TextReader reader, XmlReaderSettings settings)
      : base(XmlReader.Create(reader, settings))
    {
      this.StripPrefix = true;
    }

    public FilteringReader(string content)
      : base(XmlReader.Create((TextReader) new StringReader(content)))
    {
      this.StripPrefix = true;
    }

    public FilteringReader(string content, XmlReaderSettings settings)
      : base(XmlReader.Create((TextReader) new StringReader(content), settings))
    {
      this.StripPrefix = true;
    }

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
