using System.Xml;
using System.Xml.Schema;

namespace XmlFilter
{
  public class WrappingReader : XmlReader, IXmlLineInfo
  {
    private XmlReader _reader;
    protected IXmlLineInfo? ReaderAsIXmlLineInfo;

        public WrappingReader(XmlReader baseReader)
        {
            this._reader = baseReader;
        }

        public override void Close()
        {
            this.Reader.Close();
        }

        protected override void Dispose(bool disposing)
        {
            this.Reader.Dispose();
        }

        public override string GetAttribute(int i)
        {
            return this.Reader.GetAttribute(i);
        }

        public override string? GetAttribute(string name)
        {
            return this.Reader.GetAttribute(name);
        }

        public override string? GetAttribute(string name, string? namespaceUri)
        {
            return this.Reader.GetAttribute(name, namespaceUri);
        }

        public virtual bool HasLineInfo()
        {
            return this.ReaderAsIXmlLineInfo != null && this.ReaderAsIXmlLineInfo.HasLineInfo();
        }

        public override string? LookupNamespace(string? prefix)
        {
            return prefix == null ? null : this.Reader.LookupNamespace(prefix);
        }

        public override void MoveToAttribute(int i)
        {
            this.Reader.MoveToAttribute(i);
        }

        public override bool MoveToAttribute(string name)
        {
            return this.Reader.MoveToAttribute(name);
        }

        public override bool MoveToAttribute(string name, string? ns)
        {
            return this.Reader.MoveToAttribute(name, ns);
        }

        public override bool MoveToElement()
        {
            return this.Reader.MoveToElement();
        }

        public override bool MoveToFirstAttribute()
        {
            return this.Reader.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return this.Reader.MoveToNextAttribute();
        }

        public override bool Read()
        {
            return this.Reader.Read();
        }

        public override bool ReadAttributeValue()
        {
            return this.Reader.ReadAttributeValue();
        }

        public override void ResolveEntity()
        {
            this.Reader.ResolveEntity();
        }

        public override void Skip()
        {
            this.Reader.Skip();
        }

        public override int AttributeCount => this.Reader.AttributeCount;

        public override string BaseURI => this.Reader.BaseURI;

        public override bool CanResolveEntity => this.Reader.CanResolveEntity;

        public override int Depth => this.Reader.Depth;

        public override bool EOF => this.Reader.EOF;

        public override bool HasAttributes => this.Reader.HasAttributes;

        public override bool HasValue => this.Reader.HasValue;

        public override bool IsDefault => this.Reader.IsDefault;

        public override bool IsEmptyElement => this.Reader.IsEmptyElement;

        public virtual int LineNumber => this.ReaderAsIXmlLineInfo != null ? this.ReaderAsIXmlLineInfo.LineNumber : 0;

        public virtual int LinePosition => this.ReaderAsIXmlLineInfo != null ? this.ReaderAsIXmlLineInfo.LinePosition : 0;

        public override string LocalName => this.Reader.LocalName;

        public override string Name => this.Reader.Name;

        public override string NamespaceURI => this.Reader.NamespaceURI;

        public override XmlNameTable NameTable => this.Reader.NameTable;

        public override XmlNodeType NodeType => this.Reader.NodeType;

        public override string Prefix => this.Reader.Prefix;

        public override char QuoteChar => this.Reader.QuoteChar;

        protected XmlReader Reader
        {
            get => this._reader;
            set
      {
        this._reader = value;
        this.ReaderAsIXmlLineInfo = (IXmlLineInfo) value;
      }
        }

        public override ReadState ReadState => this.Reader.ReadState;

        public override IXmlSchemaInfo? SchemaInfo => this.Reader.SchemaInfo;

        public override XmlReaderSettings? Settings => this.Reader.Settings;

        public override string Value => this.Reader.Value;

        public override Type ValueType => this.Reader.ValueType;

        public override string XmlLang => this.Reader.XmlLang;

        public override XmlSpace XmlSpace => this.Reader.XmlSpace;
  }
}
