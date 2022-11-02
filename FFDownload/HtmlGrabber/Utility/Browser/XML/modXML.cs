using System.IO;
using System.Xml;
using XMLFilter;

namespace HtmlGrabber
{

    static class modXML
    {

        public static XmlDocument DownloadXML(string URL)
        {
            XmlDocument DownloadXMLRet = default;

            string xml;
            var xmldoc = new XmlDocument();

            xml = Program.Browser.DownloadPage(URL);

            try
            {
                xmldoc.LoadXml(xml);
            }
            catch
            {
                xmldoc = null;
            }

            DownloadXMLRet = xmldoc;

            xmldoc = null;
            xml = null;
            return DownloadXMLRet;


        }

        public static XmlDocument CleanXML(XmlDocument source, bool filter = false)


        {
            XmlDocument CleanXMLRet = default;

            string xml;
            var xmlDoc = new XmlDocument();

            xml = ConvertXMLtoFeed(source.OuterXml, filter);

            modStripTags.StripTag(ref xml, "dd");

            xmlDoc.LoadXml(xml);

            CleanXMLRet = xmlDoc;

            xmlDoc = null;
            return CleanXMLRet;

        }

        public static string ConvertXMLtoFeed(string xml, bool Filter = false)


        {

            string result;

            XMLFilteringReader reader;
            StringWriter outputFile;
            XMLFilteringWriter writer;


            reader = new XMLFilteringReader(xml) { StripPrefix = false };

            outputFile = new StringWriter();
            writer = new XMLFilteringWriter(outputFile)
            {
                FilterOutput = Filter,
                ConvertPrefixesToTags = true
            };

            writer.FilterOutput = Filter;
            writer.ConvertPrefixesToTags = true;

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