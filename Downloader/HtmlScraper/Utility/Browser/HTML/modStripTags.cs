using HtmlAgilityPack;

namespace HtmlScraper.Utility.Browser.HTML
{

    static class modStripTags
    {



        public enum partialM
        {
            Yes = 1,
            No = 2
        }

        public static void StripTag(ref string html, string attribute, string value, partialM partialMatch = partialM.No)




        {

            var doc = new HtmlAgilityPack.HtmlDocument();
            StringWriter outputFile;

            outputFile = new StringWriter();

            html = modUtility.CleanString(html);

            doc.LoadHtml(html);

            List<HtmlNode> nodesToRemove = null;

            nodesToRemove = doc.DocumentNode.Descendants().Where(n => n.Attributes.Contains(attribute)).ToList();


            foreach (var node in nodesToRemove)
            {
                if (node.Attributes[attribute].Value.Contains(value))
                {
                    node.Remove();
                }
            }

            doc.OptionOutputAsXml = true;

            doc.Save(outputFile);

            html = outputFile.ToString();

            outputFile.Close();

            doc = null;

        }

        public static void StripTag(ref string html, string tag, partialM partialMatch = partialM.No)



        {

            var doc = new HtmlAgilityPack.HtmlDocument();
            StringWriter outputFile;

            outputFile = new StringWriter();

            html = modUtility.CleanString(html);

            doc.LoadHtml(html);

            List<HtmlNode> nodesToRemove = null;

            switch (partialMatch)
            {
                case partialM.Yes:
                    {
                        nodesToRemove = doc.DocumentNode.Descendants().Where(n => n.Name.Contains(tag)).ToList();

                        break;
                    }
                case partialM.No:
                    {
                        nodesToRemove = doc.DocumentNode.Descendants().Where(n => (n.Name ?? "") == (tag ?? "")).ToList();

                        break;
                    }
            }

            foreach (var node in nodesToRemove)
                node.Remove();

            doc.OptionOutputAsXml = true;

            doc.Save(outputFile);

            html = outputFile.ToString();

            outputFile.Close();

            doc = null;

        }

    }
}