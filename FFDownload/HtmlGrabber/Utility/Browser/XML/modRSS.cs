using System.Xml;

namespace HtmlGrabber
{

    static class modRSS
    {

        #region RSS Feed Conversion

        public static XmlDocument RDFtoRSS(XmlDocument xmlDoc)
        {

            var xml_Doc = new XmlDocument();

            xml_Doc = xmlDoc;

            var xmlList = xml_Doc.DocumentElement.ChildNodes;

            long count;
            long max = xmlList.Count - 1;

            string name;


            for (count = max; count >= 0L; count += -1)
            {
                name = xmlList.Item((int)count).Name;
                switch (name ?? "")
                {
                    // Do Nothing
                    case "item":
                        {
                            break;
                        }

                    default:
                        {
                            xml_Doc.DocumentElement.RemoveChild(xmlList.Item((int)count));
                            break;
                        }
                }
            }

            xmlDoc = xml_Doc;

            string xml;
            xmlList = xmlDoc.SelectNodes("//item");

            if (xmlList.Count == 0)
            {
                return null;
            }

            int count2;

            xml = "<rss>";

            var loopTo = xmlList.Count - 1;
            for (count2 = 0; count2 <= loopTo; count2++)


                xml += xmlList[count2].OuterXml;

            xml += "</rss>";

            xml_Doc.LoadXml(xml);

            return xml_Doc;

        }

        public static XmlDocument ATOMtoRSS(XmlDocument xmlDoc)
        {

            var xml_Doc = new XmlDocument();

            xml_Doc = xmlDoc;

            var xmlList = xml_Doc.DocumentElement.ChildNodes;

            long count;
            long max = xmlList.Count - 1;

            string name;

            for (count = max; count >= 0L; count += -1)
            {
                name = xmlList.Item((int)count).Name;
                switch (name ?? "")
                {
                    // Do Nothing
                    case "entry":
                        {
                            break;
                        }

                    default:
                        {
                            xml_Doc.DocumentElement.RemoveChild(xmlList.Item((int)count));
                            break;
                        }
                }
            }

            xmlDoc = xml_Doc;

            string xml;
            xmlList = xmlDoc.SelectNodes("//entry");

            if (xmlList.Count == 0)
            {
                return null;
            }

            int count2;

            xml = "<rss>";

            var loopTo = xmlList.Count - 1;
            for (count2 = 0; count2 <= loopTo; count2++)


                xml += xmlList[count2].OuterXml;

            xml += "</rss>";

            xml_Doc.LoadXml(xml);


            return xml_Doc;

        }

        public static XmlDocument CleanRSS(XmlDocument xmlDoc)
        {

            var xml_Doc = new XmlDocument();

            xml_Doc = xmlDoc;

            XmlNode node;

            var xmlList = xml_Doc.DocumentElement.ChildNodes;

            node = xmlList[0];

            xmlList = node.ChildNodes;

            long count;
            long max = xmlList.Count - 1;

            string name;

            for (count = max; count >= 0L; count += -1)
            {
                name = xmlList.Item((int)count).Name.ToLower();
                switch (name ?? "")
                {
                    // Do Nothing
                    case "item":
                        {
                            break;
                        }

                    default:
                        {
                            node.RemoveChild(xmlList.Item((int)count));
                            break;
                        }
                }
            }


            xmlDoc = xml_Doc;

            string xml;
            xmlList = xmlDoc.SelectNodes("//item");

            if (xmlList.Count == 0)
            {
                return null;
            }

            int count2;

            xml = "<rss>";

            var loopTo = xmlList.Count - 1;
            for (count2 = 0; count2 <= loopTo; count2++)


                xml += xmlList[count2].OuterXml;

            xml += "</rss>";

            xml_Doc.LoadXml(xml);


            return xml_Doc;

        }

        #endregion

        public static XmlDocument CleanFeed(XmlDocument xmlDoc)
        {

            XmlDocument rss;

            rss = xmlDoc;

            string type;

            type = rss.DocumentElement.LocalName.ToLower();

            switch (type ?? "")
            {
                case "rdf":
                    {
                        rss = RDFtoRSS(rss);
                        break;
                    }
                case "feed":
                    {
                        rss = ATOMtoRSS(rss);
                        break;
                    }
                case "rss":
                    {
                        rss = CleanRSS(rss);
                        break;
                    }

                default:
                    {
                        return null;
                    }
            }

            return rss;

        }

    }
}