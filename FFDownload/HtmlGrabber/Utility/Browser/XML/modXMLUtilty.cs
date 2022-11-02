using System.Xml;
using Microsoft.VisualBasic;

namespace HtmlGrabber
{

    static class modXMLUtilty
    {

        public enum paramType
        {
            Tag = 1,
            Attribute = 2
        }

        public static XmlNodeList GetNodes(XmlDocument xmldoc, string xpath)


        {

            XmlNodeList XmlList;

            XmlList = xmldoc.SelectNodes(xpath);

            return XmlList;

        }

        public static XmlDocument ReturnNodes(XmlDocument xmlDoc, string XPath)
        {

            var xml_Doc = new XmlDocument();

            XmlNodeList xmlList;

            string xml;
            xmlList = xmlDoc.SelectNodes(XPath);

            // If xmlList.Count = 0 Then
            // Return Nothing
            // End If

            int count2;

            xml = "<html><body>";

            var loopTo = xmlList.Count - 1;
            for (count2 = 0; count2 <= loopTo; count2++)


                xml += xmlList[count2].OuterXml;

            xml += "</body></html>";

            xml_Doc.LoadXml(xml);


            return xml_Doc;

        }

        public static string GetFirstNodeValue(XmlDocument xmldoc, string xpath)


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

        public static void SearchForParameter(XmlNode xml_node, string param, ref string result, paramType type, modStripTags.partialM partialMatch)





        {

            long count;
            int attr_count;
            string name;
            string value;

            var xmllist = xml_node.ChildNodes;
            XmlNode child_node;

            for (count = xmllist.Count - 1; count >= 0L; count += -1)
            {
                child_node = xmllist.Item((int)count);

                switch (type)
                {
                    case paramType.Tag:
                        {

                            name = child_node.LocalName;
                            if ((name ?? "") == (param ?? ""))
                            {
                                result = child_node.InnerXml;
                                return;
                            }

                            break;
                        }

                    case paramType.Attribute:
                        {
                            if (!(child_node.Attributes == null))
                            {

                                bool match = false;

                                for (attr_count = child_node.Attributes.Count - 1; attr_count >= 0; attr_count -= 1)
                                {
                                    if (child_node.Attributes.Count > 0)
                                    {
                                        name = child_node.Attributes[attr_count].Name;
                                        value = child_node.Attributes[attr_count].Value;

                                        switch (partialMatch)
                                        {
                                            case modStripTags.partialM.No:
                                                {
                                                    match = (value ?? "") == (param ?? "");
                                                    break;
                                                }
                                            case modStripTags.partialM.Yes:
                                                {
                                                    match = Strings.InStr(value, param) > 0;
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
                {
                    SearchForParameter(child_node, param, ref result, type, partialMatch);
                }

                if (!string.IsNullOrEmpty(result))
                {
                    return;
                }

            }

        }

        public static string GetAttrValue(XmlDocument xmldoc, string element, string attribute, string search = "")




        {

            string value = "";
            string temp = "";

            XmlNodeList xmlList;

            string xpath = "//";
            xpath += element;
            xpath += "[@" + attribute + "]";

            xmlList = xmldoc.SelectNodes(xpath);

            long count;
            long max = xmlList.Count - 1;

            var loopTo = max;
            for (count = 0L; count <= loopTo; count++)
            {
                temp = xmlList[(int)count].Attributes[attribute].Value;

                if (string.IsNullOrEmpty(search))
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

    }
}