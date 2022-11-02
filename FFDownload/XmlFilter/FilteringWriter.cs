using System.Text;
using System.Xml;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace AspxToCode.Parser.Xml
{
    public class FilteringWriter : WrappingWriter
    {
        public bool FilterOutput;
        public bool ReduceConsecutiveSpace;
        private string _replacementTag;
        public bool RemoveNewlines;
        public bool ConvertPrefixesToTags;
        private string _lastElement;
        private string? _lastPrefix;
        private bool _hasPrefix;
        private bool _prefixSameAsRoot;
        private string? _root;
        private string[] _xmlns;

        public string[] AllowedTags { get; set; }

        public virtual string[] AllowedAttributes { get; set; }

        public virtual string ReplacementTag
        {
            get => this._replacementTag;
            set => this._replacementTag = value;
        }

        public FilteringWriter(TextWriter writer)
          : base(XmlWriter.Create(writer))
        {
            this.FilterOutput = false;
            this.ReduceConsecutiveSpace = true;
            this._replacementTag = "dd";
            this.RemoveNewlines = true;
            this.ConvertPrefixesToTags = false;
        }

        public FilteringWriter(StringBuilder builder)
          : base(XmlWriter.Create((TextWriter)new StringWriter(builder)))
        {
            this.FilterOutput = false;
            this.ReduceConsecutiveSpace = true;
            this._replacementTag = "dd";
            this.RemoveNewlines = true;
            this.ConvertPrefixesToTags = false;
        }

        public FilteringWriter(Stream stream)
          : base(XmlWriter.Create(stream))
        {
            this.FilterOutput = false;
            this.ReduceConsecutiveSpace = true;
            this._replacementTag = "dd";
            this.RemoveNewlines = true;
            this.ConvertPrefixesToTags = false;
        }

        public override void WriteWhitespace(string? ws)
        {
            if (this.FilterOutput)
                return;
            base.WriteWhitespace(ws);
        }

        public override void WriteComment(string? text)
        {
            if (this.FilterOutput)
                return;
            base.WriteComment(text);
        }

        public override void WriteStartElement(string? inPrefix, string localName, string? ns)
        {
            string prefix = inPrefix ?? string.Empty;

            if (this.ConvertPrefixesToTags)
            {
                if (!this._prefixSameAsRoot)
                {
                    if (!Information.IsNothing((object?)prefix) && Operators.CompareString(prefix?.ToLower(), localName.ToLower(), false) == 0)
                    {
                        this._prefixSameAsRoot = true;
                        this._root = prefix?.ToLower();
                        prefix = "";
                        ns = "";
                    }
                }
                else if (!Information.IsNothing((object?)prefix) && Operators.CompareString(prefix?.ToLower(), this._root, false) == 0)
                {
                    prefix = "";
                    ns = "";
                }
                if (Operators.CompareString(ns, "", false) != 0)
                    ns = "";
                if (Operators.CompareString(localName, this._lastPrefix, false) != 0 && Operators.CompareString(this._lastPrefix, "", false) != 0 && Operators.CompareString(prefix, "", false) == 0 | Operators.CompareString(prefix, this._lastPrefix, false) != 0 && Operators.CompareString(localName, this._lastElement, false) != 0)
                {
                    this.WriteEndElement();
                    this._lastPrefix = "";
                    this._lastElement = "";
                }
                if (Operators.CompareString(prefix, "", false) != 0)
                {
                    if (Operators.CompareString(prefix, this._lastPrefix, false) != 0)
                    {
                        this._lastPrefix = prefix;
                        prefix ??= "";
                        this.WriteStartElement(prefix);
                        prefix = "";
                        ns = "";
                        this._lastElement = localName;
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
            if (Information.IsNothing((object)reader))
                throw new ArgumentNullException(nameof(reader));
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
