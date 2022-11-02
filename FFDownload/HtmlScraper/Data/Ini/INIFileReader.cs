using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace HtmlScraper.Data.Ini
{

    enum IniItemTypeEnum
    {
        GetKeys = 0,
        GetValues = 1,
        GetKeysAndValues = 2
    }

    public class IniFileReaderNotInitializedException : ApplicationException
    {
        public override string Message
        {
            get
            {
                return "The IniFileReader instance has not been properly initialized.";

            }
        }
    }

    public class IniFileReader
    {
        private string m_IniFilename;
        private XmlDocument m_XmlDoc;
        private ArrayList unattachedComments = new ArrayList();
        private StringCollection sections = new StringCollection();
        private bool m_CaseSensitive = false;
        private string m_SaveFilename;
        private bool m_initialized = false;

        public IniFileReader(string IniFilename)
        {
            InitIniFileReader(IniFilename, false);
        }

        public IniFileReader(string IniFilename, bool IsCaseSensitive)
        {
            InitIniFileReader(IniFilename, IsCaseSensitive);
        }

        private void InitIniFileReader(string IniFilename, bool IsCaseSensitive)



        {
            FileInfo fi;
            string s;
            TextReader tr = null;
            m_CaseSensitive = IsCaseSensitive;
            m_XmlDoc = new XmlDocument();

            if (IniFilename is null || string.IsNullOrEmpty(IniFilename.Trim()))
            {
                return;
            }

            // try to load the file as an XML file
            try
            {
                m_XmlDoc.Load(IniFilename);
                UpdateSections();
                m_IniFilename = IniFilename;
                m_initialized = true;
            }
            catch
            {
                // load the default XML
                string xml;
                xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml += "<sections></sections>";
                m_XmlDoc.LoadXml(xml);

                try
                {
                    fi = new FileInfo(IniFilename);
                    if (fi.Exists)
                    {
                        tr = fi.OpenText();
                        s = tr.ReadLine();
                        while (s is not null)
                        {
                            ParseLineXml(s, m_XmlDoc);
                            s = tr.ReadLine();
                        }
                        m_IniFilename = IniFilename;
                        m_initialized = true;
                    }
                    else
                    {
                        m_XmlDoc.Save(IniFilename);
                        m_IniFilename = IniFilename;
                        m_initialized = true;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    if (tr is not null)
                    {
                        tr.Close();
                    }
                }
            }
        }

        public string IniFilename
        {
            get
            {
                if (!Initialized)
                    throw new IniFileReaderNotInitializedException();
                return m_IniFilename;
            }
        }

        public bool Initialized
        {
            get
            {
                return m_initialized;
            }
        }

        public bool CaseSensitive
        {
            get
            {
                return m_CaseSensitive;
            }
        }

        private string SetNameCase(string aName)
        {
            if (CaseSensitive)
            {
                return aName;
            }
            else
            {
                return aName.ToLower();
            }
        }

        private XmlElement GetRoot()
        {
            return m_XmlDoc.DocumentElement;
        }

        private XmlElement GetLastSection()
        {
            if (sections.Count == 0)
            {
                return GetRoot();
            }
            else
            {
                return GetSection(sections[sections.Count - 1]);
            }
        }

        private XmlElement GetSection(string sectionName)
        {
            if (sectionName != default && !string.IsNullOrEmpty(sectionName))
            {
                sectionName = SetNameCase(sectionName);
                return (XmlElement)m_XmlDoc.SelectSingleNode("//section[@name='" + sectionName + "']");
            }
            return null;
        }

        private XmlElement GetItem(string sectionName, string keyName)
        {
            XmlElement section;
            if (keyName is not null && !string.IsNullOrEmpty(keyName))
            {
                keyName = SetNameCase(keyName);
                section = GetSection(sectionName);
                if (section is not null)
                {
                    return (XmlElement)section.SelectSingleNode("item[@key='" + keyName + "']");
                }
            }
            return null;
        }


        public bool SetIniSection(string oldSection, string newSection)
        {
            XmlElement section;
            if (!Initialized)
            {
                throw new IniFileReaderNotInitializedException();
            }
            if (newSection is not null && !string.IsNullOrEmpty(newSection))
            {
                section = GetSection(oldSection);
                if (section is not null)
                {
                    section.SetAttribute("name", SetNameCase(newSection));
                    UpdateSections();
                    return true;
                }
            }
            return false;
        }
        public bool SetIniValue(string sectionName, string keyName, string newValue)
        {
            XmlElement item;
            XmlElement section;
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            section = GetSection(sectionName);
            if (section is null)
            {
                if (CreateSection(sectionName))
                {
                    section = GetSection(sectionName);
                    // exit if keyName is Nothing or blank
                    if (keyName is null || string.IsNullOrEmpty(keyName))
                    {
                        return true;
                    }
                }
                else
                {
                    // can't create section
                    return false;
                }
            }
            if (keyName is null)
            {
                // delete the section
                return DeleteSection(sectionName);
            }

            item = GetItem(sectionName, keyName);
            if (item is not null)
            {
                if (newValue is null)
                {
                    // delete this item
                    return DeleteItem(sectionName, keyName);
                }
                else
                {
                    // add or update the value attribute
                    item.SetAttribute("value", newValue);
                    return true;
                }
            }
            // try to create the item
            else if (!string.IsNullOrEmpty(keyName) && newValue is not null)
            {
                // construct a new item (blank values are OK)
                item = m_XmlDoc.CreateElement("item");
                item.SetAttribute("key", SetNameCase(keyName));
                item.SetAttribute("value", newValue);
                section.AppendChild(item);
                return true;
            }
            return false;
        }

        private bool DeleteSection(string sectionName)
        {
            var section = GetSection(sectionName);
            if (section is not null)
            {
                section.ParentNode.RemoveChild(section);
                UpdateSections();
                return true;
            }
            return false;
        }

        private bool DeleteItem(string sectionName, string keyName)
        {
            var item = GetItem(sectionName, keyName);
            if (item is not null)
            {
                item.ParentNode.RemoveChild(item);
                return true;
            }
            return false;
        }

        public bool SetIniKey(string sectionName, string keyName, string newValue)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            var item = GetItem(sectionName, keyName);
            if (item is not null)
            {
                item.SetAttribute("key", SetNameCase(newValue));
                return true;
            }
            return false;
        }

        public string GetIniValue(string sectionName, string keyName)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            XmlNode N = GetItem(sectionName, keyName);
            if (N is not null)
            {
                return N.Attributes.GetNamedItem("value").Value;
            }
            return null;
        }
        public StringCollection GetIniComments(string sectionName)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            var sc = new StringCollection();
            XmlNode target;
            XmlNodeList nodes;
            if (sectionName is null)
            {
                target = m_XmlDoc.DocumentElement;
            }
            else
            {
                target = GetSection(sectionName);
            }
            if (target is not null)
            {
                nodes = target.SelectNodes("comment");
                if (nodes.Count > 0)
                {
                    foreach (XmlNode N in nodes)
                        sc.Add(N.InnerText);
                }
            }
            return sc;
        }

        public bool SetIniComments(string sectionName, StringCollection comments)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            XmlNode target;
            XmlNodeList nodes;
            XmlNode N;
            XmlElement NLastComment;
            if (sectionName is null)
            {
                target = m_XmlDoc.DocumentElement;
            }
            else
            {
                target = GetSection(sectionName);
            }
            if (target is not null)
            {
                nodes = target.SelectNodes("comment");
                foreach (XmlNode currentN in nodes)
                {
                    N = currentN;
                    target.RemoveChild(N);
                }
                foreach (string s in comments)
                {
                    N = m_XmlDoc.CreateElement("comment");
                    N.InnerText = s;
                    NLastComment = (XmlElement)target.SelectSingleNode("comment[last()]");
                    if (NLastComment is null)
                    {
                        target.PrependChild(N);
                    }
                    else
                    {
                        target.InsertAfter(N, NLastComment);
                    }
                }
                return true;
            }
            return false;
        }

        private void UpdateSections()
        {
            sections = new StringCollection();
            foreach (XmlElement N in m_XmlDoc.SelectNodes("sections/section"))
                sections.Add(N.GetAttribute("name"));
        }

        public StringCollection AllSections
        {
            get
            {
                if (!Initialized)
                {
                    throw new IniFileReaderNotInitializedException();
                }
                return sections;
            }
        }

        private StringCollection GetItemsInSection(string sectionName, IniItemTypeEnum itemType)
        {
            XmlNodeList nodes;
            var items = new StringCollection();
            XmlNode section = GetSection(sectionName);
            if (section is null)
            {
                return null;
            }
            else
            {
                nodes = section.SelectNodes("item");
                if (nodes.Count > 0)
                {
                    foreach (XmlNode N in nodes)
                    {
                        switch (itemType)
                        {
                            case IniItemTypeEnum.GetKeys:
                                {
                                    items.Add(N.Attributes.GetNamedItem("key").Value);
                                    break;
                                }
                            case IniItemTypeEnum.GetValues:
                                {
                                    items.Add(N.Attributes.GetNamedItem("value").Value);
                                    break;
                                }
                            case IniItemTypeEnum.GetKeysAndValues:
                                {
                                    items.Add(N.Attributes.GetNamedItem("key").Value + "=" + N.Attributes.GetNamedItem("value").Value);
                                    break;
                                }
                        }
                    }
                }
                return items;
            }
        }

        public StringCollection AllKeysInSection(string sectionName)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            return GetItemsInSection(sectionName, IniItemTypeEnum.GetKeys);
        }

        public StringCollection AllValuesInSection(string sectionName)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            return GetItemsInSection(sectionName, IniItemTypeEnum.GetValues);
        }

        public StringCollection AllItemsInSection(string sectionName)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            return GetItemsInSection(sectionName, IniItemTypeEnum.GetKeysAndValues);
        }

        public string GetCustomIniAttribute(string sectionName, string keyName, string attributeName)
        {
            XmlElement N;
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            if (attributeName is not null && !string.IsNullOrEmpty(attributeName))
            {
                N = GetItem(sectionName, keyName);
                if (N is not null)
                {
                    attributeName = SetNameCase(attributeName);
                    return N.GetAttribute(attributeName);
                }
            }
            return null;
        }

        public bool SetCustomIniAttribute(string sectionName, string keyName, string attributeName, string attributeValue)
        {
            XmlElement N;
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            if (!string.IsNullOrEmpty(attributeName))
            {
                N = GetItem(sectionName, keyName);
                if (N is not null)
                {
                    try
                    {
                        if (attributeValue is null)
                        {
                            // delete the attribute
                            N.RemoveAttribute(attributeName);
                            return true;
                        }
                        else
                        {
                            attributeName = SetNameCase(attributeName);
                            N.SetAttribute(attributeName, attributeValue);
                            return true;
                        }
                    }

                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                return false;
            }

            return true;

        }

        private bool CreateSection(string sectionName)
        {
            XmlElement N;
            XmlAttribute Natt;
            if (sectionName is not null && !string.IsNullOrEmpty(sectionName))
            {
                sectionName = SetNameCase(sectionName);
                try
                {
                    N = m_XmlDoc.CreateElement("section");
                    Natt = m_XmlDoc.CreateAttribute("name");
                    Natt.Value = SetNameCase(sectionName);
                    N.Attributes.SetNamedItem(Natt);
                    m_XmlDoc.DocumentElement.AppendChild(N);
                    sections.Add(Natt.Value);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
            }
            return false;
        }
        private bool CreateItem(string sectionName, string keyName, string newValue)
        {
            XmlElement item;
            XmlElement section;
            try
            {
                section = GetSection(sectionName);
                if (section is not null)
                {
                    item = m_XmlDoc.CreateElement("item");
                    item.SetAttribute("key", keyName);
                    item.SetAttribute("newValue", newValue);
                    section.AppendChild(item);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        private void ParseLineXml(string s, XmlDocument doc)
        {
            string key;
            string value;
            XmlElement N;
            XmlAttribute Natt;
            string[] parts;
            s.TrimStart();

            if (s.Length == 0)
            {
                return;
            }
            switch (s.Substring(0, 1) ?? "")
            {
                case "[":
                    {
                        // this is a section
                        // trim the first and last characters
                        s = s.TrimStart('[');
                        s = s.TrimEnd(']');
                        // create a new section element
                        CreateSection(s);
                        break;
                    }
                case ";":
                    {
                        // new comment
                        N = doc.CreateElement("comment");
                        N.InnerText = s.Substring(1);
                        GetLastSection().AppendChild(N);
                        break;
                    }

                default:
                    {
                        // split the string on the "=" sign, if present
                        if (s.IndexOf("=") > 0)
                        {
                            parts = s.Split('=');
                            key = parts[0].Trim();
                            value = parts[1].Trim();
                        }
                        else
                        {
                            key = s;
                            value = "";
                        }
                        N = doc.CreateElement("item");
                        Natt = doc.CreateAttribute("key");
                        Natt.Value = SetNameCase(key);
                        N.Attributes.SetNamedItem(Natt);
                        Natt = doc.CreateAttribute("value");
                        Natt.Value = value;
                        N.Attributes.SetNamedItem(Natt);
                        GetLastSection().AppendChild(N);
                        break;
                    }
            }

        }
        public string OutputFilename
        {
            get
            {
                if (!Initialized)
                    throw new IniFileReaderNotInitializedException();
                return m_SaveFilename;
            }
            set
            {
                FileInfo fi;
                if (!Initialized)
                    throw new IniFileReaderNotInitializedException();
                fi = new FileInfo(value);
                if (!fi.Directory.Exists)
                {
                    MessageBox.Show("Invalid path.");
                }
                else
                {
                    m_SaveFilename = value;
                }
            }
        }

        public void Save()
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            if (OutputFilename is not null && m_XmlDoc is not null)
            {
                var fi = new FileInfo(OutputFilename);
                if (!fi.Directory.Exists)
                {
                    MessageBox.Show("Invalid path.");
                    return;
                }
                if (fi.Exists)
                {
                    fi.Delete();
                    m_XmlDoc.Save(OutputFilename);
                }
            }
        }

        public string AsIniFile()
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            try
            {

                // Create the XsltSettings object with script enabled.
                var settings = new XsltSettings(true, true);

                // Execute the transform.
                var xsl = new XslCompiledTransform();

                xsl.Load(Application.StartupPath + @"\\XMLToIni.xslt", settings, new XmlUrlResolver());




                var sb = new StringBuilder();
                var sw = new StringWriter(sb);

                xsl.Transform(new XmlNodeReader(m_XmlDoc), null, sw);

                sw.Close();
                return sb.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }

        }

        public void SaveAsIniFile()
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            if (OutputFilename is not null && m_XmlDoc is not null)
            {
                var fi = new FileInfo(OutputFilename);
                if (!fi.Directory.Exists)
                {
                    MessageBox.Show("Invalid path.");
                    return;
                }
                if (fi.Exists)
                {
                    fi.Delete();
                    var encUTF8 = Encoding.UTF8;
                    StreamReader sr;
                    StreamWriter sw;

                    try
                    {
                        sr = new StreamReader(StringToStream(AsIniFile()));
                        sw = new StreamWriter(OutputFilename, false, encUTF8);
                        sw.Write(sr.ReadToEnd());

                        sr.Close();
                        sw.Close();
                    }
                    catch
                    {
                    }
                    finally
                    {
                        sr = null;
                        sw = null;
                    }
                }
            }
        }

        private Stream StringToStream(string data)
        {

            if (!string.IsNullOrEmpty(data))
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                var ms = new MemoryStream(bytes);

                return ms;
            }
            else
            {
                return null;
            }


        }



        public XmlDocument XmlDoc
        {
            get
            {
                if (!Initialized)
                    throw new IniFileReaderNotInitializedException();
                return m_XmlDoc;
            }
        }

        public string XML
        {
            get
            {
                if (!Initialized)
                    throw new IniFileReaderNotInitializedException();
                var sb = new StringBuilder();
                var sw = new StringWriter(sb);
                var xw = new XmlTextWriter(sw)
                {
                    Indentation = 3,
                    Formatting = Formatting.Indented
                };
                m_XmlDoc.WriteContentTo(xw);
                xw.Close();
                sw.Close();
                return sb.ToString();
            }
        }
    }
}