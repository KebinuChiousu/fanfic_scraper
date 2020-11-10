using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Xml;
using Web.Utility.Helper;
using Web.Utility.Xml;

namespace Web.Utility.Xml
{
    public static class Utility
    {
        public enum ParamType
        {
            Tag = 1,
            Attribute = 2
        }

        public static XmlNodeList GetNodes(XmlDocument xmldoc, string xpath
                        )
        {
            XmlNodeList XmlList;

            XmlList = xmldoc.SelectNodes(xpath);

            return XmlList;
        }

        public static XmlDocument ReturnNodes(XmlDocument xmlDoc, string XPath)
        {
            XmlDocument xml_Doc = new XmlDocument();

            XmlNodeList xmlList;

            string xml;
            xmlList = xmlDoc.SelectNodes(XPath);

            // If xmlList.Count = 0 Then
            // Return Nothing
            // End If

            int count2;

            xml = "<html><body>";

            for (count2 = 0; count2 <= xmlList.Count - 1; count2++)

                xml += xmlList[count2].OuterXml;

            xml += "</body></html>";

            xml_Doc.LoadXml(xml);


            return xml_Doc;
        }

        public static string GetFirstNodeValue(XmlDocument xmldoc, string xpath
                                )
        {
            string result = "";
            XmlNode xml_node;
            XmlNodeList XmlList;

            XmlList = xmldoc.SelectNodes(xpath);

            if (XmlList.Count > 0)
            {
                xml_node = XmlList[0];
                result = xml_node.InnerText;
            }


            return result;
        }

        public static void SearchForParameter(XmlNode xml_node, string param, ref string result, ParamType type, bool PartialMatch)
        {
            int attr_count;
            string name;
            string value;

            XmlNodeList xmllist = xml_node.ChildNodes;
            XmlNode child_node;

            for (int count = (xmllist.Count - 1); count >= 0; count += -1)
            {
                child_node = xmllist.Item(count);

                switch (type)
                {
                    case ParamType.Tag:
                        {
                            name = child_node.LocalName;
                            if (name == param)
                            {
                                result = child_node.InnerXml;
                                return;
                            }

                            break;
                        }

                    case ParamType.Attribute:
                        {
                            if (!Information.IsNothing(child_node.Attributes))
                            {
                                bool match = false;

                                for (attr_count = (child_node.Attributes.Count - 1); attr_count >= 0; attr_count += -1)
                                {
                                    if (child_node.Attributes.Count > 0)
                                    {
                                        value = child_node.Attributes[attr_count].Value;

                                        switch (PartialMatch)
                                        {
                                            case false:
                                                {
                                                    match = (value == param);
                                                    break;
                                                }

                                            case true:
                                                {
                                                    match = (Strings.InStr(value, param) > 0);
                                                    break;
                                                }
                                        }

                                        if (match)
                                        {
                                            result = child_node.InnerXml;
                                            return;
                                        }
                                    }
                                }
                            }

                            break;
                        }
                }

                if (child_node.ChildNodes.Count > 0)
                    SearchForParameter(child_node, param, ref result, type, PartialMatch);

                if (result != "")
                    return;
            }
        }

        public static string GetAttrValue(XmlDocument xmldoc, string element, string attribute, string search = ""
                        )
        {
            string value = "";
            string temp;

            XmlNodeList xmlList;

            string xpath = "//";
            xpath += element;
            xpath += "[@" + attribute + "]";

            xmlList = xmldoc.SelectNodes(xpath);

            long max = xmlList.Count - 1;

            for (int count = 0; count <= max; count++)
            {
                temp = xmlList[count].Attributes[attribute].Value;

                if (search == "")
                {
                    value = temp;
                    break;
                }

                if (Strings.InStr(temp, search) != 0)
                {
                    value = temp;
                    break;
                }
            }

            return value;
        }

        public static XmlDocument DownloadXML(string URL)
        {

            string xml;
            XmlDocument xmldoc = new XmlDocument();

            xml = Browser.DownloadPage(URL);

            try
            {
                xmldoc.LoadXml(xml);
            }
            catch
            {
                xmldoc = null;
            }

            return xmldoc;

        }


        public static XmlDocument CleanXML(XmlDocument source, bool filter = false)
        {
            string xml;
            XmlDocument xmlDoc = new XmlDocument();

            xml = ConvertXMLtoFeed(source.OuterXml, filter);

            HtmlHelper.StripTag(ref xml, "dd");

            xmlDoc.LoadXml(xml);

            return xmlDoc;

        }
        
        public static string ConvertXMLtoFeed(string xml, bool Filter = false
                                )
        {
            string result;

            XmlFiltering.Reader reader;
            StringWriter outputFile;
            XmlFiltering.Writer writer;


            reader = new XmlFiltering.Reader(xml)
            {
                StripPrefix = false
            };

            outputFile = new StringWriter();

            writer = new XmlFiltering.Writer(outputFile)
            {
                FilterOutput = Filter,
                ConvertPrefixesToTags = true
            };

            reader.Read();
            while (!reader.EOF)
                writer.WriteNode(reader, true);

            writer.WriteEndDocument();
            writer.Flush();

            result = outputFile.ToString();

            outputFile.Close();

            return result;
        }
    }
}