using System;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace web_scraper.Models.Utility.XmlFiltering
{
  public class Writer : web_scraper.Models.Utility.XmlWrapping.Writer
    {
    public bool FilterOutput;
    public bool ReduceConsecutiveSpace;
    private string[] _AllowedTags;
    private string _ReplacementTag;
    private string[] _AllowedAttributes;
    public bool RemoveNewlines;
    public bool ConvertPrefixesToTags;
    private string LastElement;
    private string LastPrefix;
    private bool HasPrefix;
    private bool PrefixSameAsRoot;
    private string root;
    private string[] xmlns;

    public string[] AllowedTags
    {
      get => this._AllowedTags;
      set => this._AllowedTags = value;
    }

    public virtual string[] AllowedAttributes
    {
      get => this._AllowedAttributes;
      set => this._AllowedAttributes = value;
    }

    public virtual string ReplacementTag
    {
      get => this._ReplacementTag;
      set => this._ReplacementTag = value;
    }

    public Writer(TextWriter writer)
      : base(XmlWriter.Create(writer))
    {
      this.FilterOutput = false;
      this.ReduceConsecutiveSpace = true;
      this._ReplacementTag = "dd";
      this.RemoveNewlines = true;
      this.ConvertPrefixesToTags = false;
    }

    public Writer(StringBuilder builder)
      : base(XmlWriter.Create((TextWriter) new StringWriter(builder)))
    {
      this.FilterOutput = false;
      this.ReduceConsecutiveSpace = true;
      this._ReplacementTag = "dd";
      this.RemoveNewlines = true;
      this.ConvertPrefixesToTags = false;
    }

    public Writer(Stream stream)
      : base(XmlWriter.Create(stream))
    {
      this.FilterOutput = false;
      this.ReduceConsecutiveSpace = true;
      this._ReplacementTag = "dd";
      this.RemoveNewlines = true;
      this.ConvertPrefixesToTags = false;
    }

    public override void WriteWhitespace(string ws)
    {
      if (this.FilterOutput)
        return;
      base.WriteWhitespace(ws);
    }

    public override void WriteComment(string text)
    {
      if (this.FilterOutput)
        return;
      base.WriteComment(text);
    }

    public override void WriteStartElement(string prefix, string localName, string ns)
    {
      if (this.ConvertPrefixesToTags)
      {
        if (!this.PrefixSameAsRoot)
        {
          if (!Information.IsNothing((object) prefix) && Operators.CompareString(prefix.ToLower(), localName.ToLower(), false) == 0)
          {
            this.PrefixSameAsRoot = true;
            this.root = prefix.ToLower();
            prefix = "";
            ns = "";
          }
        }
        else if (!Information.IsNothing((object) prefix) && Operators.CompareString(prefix.ToLower(), this.root, false) == 0)
        {
          prefix = "";
          ns = "";
        }
        if (Operators.CompareString(ns, "", false) != 0)
          ns = "";
        if (Operators.CompareString(localName, this.LastPrefix, false) != 0 && Operators.CompareString(this.LastPrefix, "", false) != 0 && (Operators.CompareString(prefix, "", false) == 0 | Operators.CompareString(prefix, this.LastPrefix, false) != 0 && Operators.CompareString(localName, this.LastElement, false) != 0))
        {
          this.WriteEndElement();
          this.LastPrefix = "";
          this.LastElement = "";
        }
        if (Operators.CompareString(prefix, "", false) != 0)
        {
          if (Operators.CompareString(prefix, this.LastPrefix, false) != 0)
          {
            this.LastPrefix = prefix;
            this.WriteStartElement(prefix);
            prefix = "";
            ns = "";
            this.LastElement = localName;
          }
          else
          {
            prefix = "";
            ns = "";
          }
        }
      }
      localName = localName.ToLower();
      if (this.FilterOutput)
      {
        bool flag = false;
        string lower = localName.ToLower();
        string[] allowedTags = this.AllowedTags;
        int index = 0;
        while (index < allowedTags.Length)
        {
          if (Operators.CompareString(allowedTags[index], lower, false) == 0)
          {
            flag = true;
            break;
          }
          checked { ++index; }
        }
        if (!flag)
          localName = this.ReplacementTag;
      }
      base.WriteStartElement(prefix, localName, ns);
    }

    public override void WriteAttributes(XmlReader reader, bool defattr)
    {
      bool flag = false;
      if (Information.IsNothing((object) reader))
        throw new ArgumentNullException(nameof (reader));
      if (reader.NodeType == XmlNodeType.Element | reader.NodeType == XmlNodeType.XmlDeclaration)
      {
        if (!reader.MoveToFirstAttribute())
          return;
        this.WriteAttributes(reader, defattr);
        reader.MoveToElement();
      }
      else
      {
        if (reader.NodeType != XmlNodeType.Attribute)
          throw new XmlException("Xml_InvalidPosition");
        do
        {
          if (defattr | !reader.IsDefault)
          {
            string str = reader.Prefix;
            string ns = reader.NamespaceURI;
            string lower = reader.LocalName.ToLower();
            if (this.FilterOutput)
            {
              string[] allowedAttributes = this.AllowedAttributes;
              int index = 0;
              while (index < allowedAttributes.Length)
              {
                if (Operators.CompareString(allowedAttributes[index], lower, false) == 0)
                {
                  flag = true;
                  break;
                }
                checked { ++index; }
              }
            }
            else
              flag = true;
            if (this.ConvertPrefixesToTags)
            {
              if (Operators.CompareString(str, "xmlns", false) == 0)
                flag = false;
              if (Operators.CompareString(lower, "xmlns", false) == 0)
                flag = false;
              if (Operators.CompareString(str, "", false) != 0)
              {
                str = "";
                ns = "";
              }
            }
            else
              flag = true;
            if (flag)
            {
              this.WriteStartAttribute(str, lower, ns);
              while (reader.ReadAttributeValue())
              {
                if (reader.NodeType == XmlNodeType.EntityReference)
                {
                  if (reader.NodeType == XmlNodeType.EntityReference && flag)
                    this.WriteEntityRef(reader.Name);
                  else if (flag)
                    this.WriteString(reader.Value);
                }
                else if (flag)
                  this.WriteString(reader.Value);
              }
            }
            if (flag)
              this.WriteEndAttribute();
          }
        }
        while (reader.MoveToNextAttribute());
      }
    }
  }
}
