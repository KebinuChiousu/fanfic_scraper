using System;
using System.Xml;
using System.Xml.Schema;

namespace Web.Utility.Xml.XmlWrapping
{
  public class Reader : XmlReader
  {
    protected XmlReader _reader;
    protected IXmlLineInfo _readerAsIXmlLineInfo;

    public Reader(XmlReader baseReader) => this.XmlReader = baseReader;

    public override void Close() => this.XmlReader.Close();

    protected override void Dispose(bool disposing) => ((IDisposable) this.XmlReader).Dispose();

    public override string GetAttribute(int i) => this.XmlReader.GetAttribute(i);

    public override string GetAttribute(string name) => this.XmlReader.GetAttribute(name);

    public override string GetAttribute(string name, string namespaceURI) => this.XmlReader.GetAttribute(name, namespaceURI);

    public virtual bool HasLineInfo() => this._readerAsIXmlLineInfo != null && this._readerAsIXmlLineInfo.HasLineInfo();

    public override string LookupNamespace(string prefix) => this.XmlReader.LookupNamespace(prefix);

    public override void MoveToAttribute(int i) => this.XmlReader.MoveToAttribute(i);

    public override bool MoveToAttribute(string name) => this.XmlReader.MoveToAttribute(name);

    public override bool MoveToAttribute(string name, string ns) => this.XmlReader.MoveToAttribute(name, ns);

    public override bool MoveToElement() => this.XmlReader.MoveToElement();

    public override bool MoveToFirstAttribute() => this.XmlReader.MoveToFirstAttribute();

    public override bool MoveToNextAttribute() => this.XmlReader.MoveToNextAttribute();

    public override bool Read() => this.XmlReader.Read();

    public override bool ReadAttributeValue() => this.XmlReader.ReadAttributeValue();

    public override void ResolveEntity() => this.XmlReader.ResolveEntity();

    public override void Skip() => this.XmlReader.Skip();

    public override int AttributeCount => this.XmlReader.AttributeCount;

    public override string BaseURI => this.XmlReader.BaseURI;

    public override bool CanResolveEntity => this.XmlReader.CanResolveEntity;

    public override int Depth => this.XmlReader.Depth;

    public override bool EOF => this.XmlReader.EOF;

    public override bool HasAttributes => this.XmlReader.HasAttributes;

    public override bool HasValue => this.XmlReader.HasValue;

    public override bool IsDefault => this.XmlReader.IsDefault;

    public override bool IsEmptyElement => this.XmlReader.IsEmptyElement;

    public virtual int LineNumber()
    {
        var ret = 0;

        if (this._readerAsIXmlLineInfo != null)
        {
            ret = this._readerAsIXmlLineInfo.LineNumber;
        }

        return ret;
    }

    public virtual int LinePosition()
    {
        var ret = 0;
        
        if (this._readerAsIXmlLineInfo != null)
        {
            ret = this._readerAsIXmlLineInfo.LinePosition;
        }

        return ret;
    }

    public override string LocalName => this.XmlReader.LocalName;

    public override string Name => this.XmlReader.Name;

    public override string NamespaceURI => this.XmlReader.NamespaceURI;

    public override XmlNameTable NameTable => this.XmlReader.NameTable;

    public override XmlNodeType NodeType => this.XmlReader.NodeType;

    public override string Prefix => this.XmlReader.Prefix;

    public override char QuoteChar => this.XmlReader.QuoteChar;

    protected XmlReader XmlReader
    {
      get => this._reader;
      set
      {
        this._reader = value;
        this._readerAsIXmlLineInfo = (IXmlLineInfo) value;
      }
    }

    public override ReadState ReadState => this.XmlReader.ReadState;

    public override IXmlSchemaInfo SchemaInfo => this.XmlReader.SchemaInfo;

    public override XmlReaderSettings Settings => this.XmlReader.Settings;

    public override string Value => this.XmlReader.Value;

    public override Type ValueType => this.XmlReader.ValueType;

    public override string XmlLang => this.XmlReader.XmlLang;

    public override XmlSpace XmlSpace => this.XmlReader.XmlSpace;
  }
}
