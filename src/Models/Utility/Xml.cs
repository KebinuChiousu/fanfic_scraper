using System.IO;
using System.Xml;
//using XMLFilter;

using web_scraper.Models.Utility;

namespace web_scraper.Models.Utility
{
    static class Xml
    {
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

        /*
        public static XmlDocument CleanXML(XmlDocument source, bool filter = false
                        )
        {
            string xml;
            XmlDocument xmlDoc = new XmlDocument();

            xml = ConvertXMLtoFeed(source.OuterXml, filter);

            HtmlHelper.StripTag(ref xml, "dd");

            xmlDoc.LoadXml(xml);

            return xmlDoc;

        }
        */

        /*
        public static string ConvertXMLtoFeed(string xml, bool Filter = false
                                )
        {
            string result;

            XMLFilteringReader reader;
            StringWriter outputFile;
            XMLFilteringWriter writer;


            reader = new XMLFilteringReader(xml);
            reader.StripPrefix = false;

            outputFile = new StringWriter();
            writer = new XMLFilteringWriter(outputFile);

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
        */
    }
}
