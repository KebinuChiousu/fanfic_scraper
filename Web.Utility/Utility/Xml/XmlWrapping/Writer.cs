using System;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Web.Utility.Xml.XmlWrapping
{
  public class Writer : XmlWriter
  {
    protected XmlWriter _writer;

    public Writer(XmlWriter baseWriter) => this.XmlWriter = baseWriter;

    public override void Close() => this.XmlWriter.Close();

    protected override void Dispose(bool disposing) => ((IDisposable) this.XmlWriter).Dispose();

    public override void Flush() => this.XmlWriter.Flush();

    public override string LookupPrefix(string ns) => this.XmlWriter.LookupPrefix(ns);

    public override void WriteBase64(byte[] buffer, int index, int count) => this.XmlWriter.WriteBase64(buffer, index, count);

    public override void WriteCData(string text) => this.XmlWriter.WriteCData(text);

    public override void WriteCharEntity(char ch) => this.XmlWriter.WriteCharEntity(ch);

    public override void WriteChars(char[] buffer, int index, int count) => this.XmlWriter.WriteChars(buffer, index, count);

    public override void WriteComment(string text) => this.XmlWriter.WriteComment(text);

    public override void WriteDocType(string name, string pubid, string sysid, string subset) => this.XmlWriter.WriteDocType(name, pubid, sysid, subset);

    public override void WriteEndAttribute() => this.XmlWriter.WriteEndAttribute();

    public override void WriteEndDocument() => this.XmlWriter.WriteEndDocument();

    public override void WriteEndElement() => this.XmlWriter.WriteEndElement();

    public override void WriteEntityRef(string name) => this.XmlWriter.WriteEntityRef(name);

    public override void WriteFullEndElement() => this.XmlWriter.WriteFullEndElement();

    public override void WriteProcessingInstruction(string name, string text) => this.XmlWriter.WriteProcessingInstruction(name, text);

    public override void WriteRaw(string data) => this.XmlWriter.WriteRaw(data);

    public override void WriteRaw(char[] buffer, int index, int count) => this.XmlWriter.WriteRaw(buffer, index, count);

    public override void WriteStartAttribute(string prefix, string localName, string ns) => this.XmlWriter.WriteStartAttribute(prefix, localName, ns);

    public override void WriteStartDocument() => this.XmlWriter.WriteStartDocument();

    public override void WriteStartDocument(bool standalone) => this.XmlWriter.WriteStartDocument(standalone);

    public override void WriteStartElement(string prefix, string localName, string ns) => this.XmlWriter.WriteStartElement(prefix, localName, ns);

    public override void WriteString(string text) => this.XmlWriter.WriteString(text);

    public override void WriteSurrogateCharEntity(char lowChar, char highChar) => this.XmlWriter.WriteSurrogateCharEntity(lowChar, highChar);

    public override void WriteValue(bool value) => this.XmlWriter.WriteValue(value);

    public override void WriteValue(DateTime value) => this.XmlWriter.WriteValue(value);

    public override void WriteValue(Decimal value) => this.XmlWriter.WriteValue(value);

    public override void WriteValue(double value) => this.XmlWriter.WriteValue(value);

    public override void WriteValue(int value) => this.XmlWriter.WriteValue(value);

    public override void WriteValue(long value) => this.XmlWriter.WriteValue(value);

    public override void WriteValue(object value) => this.XmlWriter.WriteValue(RuntimeHelpers.GetObjectValue(value));

    public override void WriteValue(float value) => this.XmlWriter.WriteValue(value);

    public override void WriteValue(string value) => this.XmlWriter.WriteValue(value);

    public override void WriteWhitespace(string ws) => this.XmlWriter.WriteWhitespace(ws);

    public override XmlWriterSettings Settings => this.XmlWriter.Settings;

    protected XmlWriter XmlWriter
    {
      get => this._writer;
      set => this._writer = value;
    }

    public override WriteState WriteState => this.XmlWriter.WriteState;

    public override string XmlLang => this.XmlWriter.XmlLang;

    public override XmlSpace XmlSpace => this.XmlWriter.XmlSpace;
  }
}
