using System.Xml;
using HtmlAgilityPack;
using HtmlScraper.BusinessLogic.Sites.Base;
using HtmlScraper.Utility;
using HtmlScraper.Utility.Browser.HTML;
using HtmlScraper.Utility.Browser.XML;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace HtmlScraper.BusinessLogic.Sites
{

    class FFNet : clsFanfic
    {

        #region Retrieval Functions

        public override string GrabTitle(string htmldoc)


        {

            string value;
            HtmlAgilityPack.HtmlDocument doc;
            int ch = 0;
            string title;

            doc = modHTML.CleanHTML(ref htmldoc);

            value = doc.DocumentNode.SelectSingleNode("//title").InnerText;
            value = Strings.Replace(value, "| FanFiction", "");

            ch = Strings.InStr(value, "Chapter");

            if (ch > 0)
            {
                title = Strings.Trim(Strings.Mid(value, 1, ch - 1));
                value = Strings.Replace(value, title, "");
                ch = modUtility.LastPos(value, ",");
                value = title + "<br><br>" + Strings.Mid(value, 1, ch - 1);
                // value = title

            }

            doc = null;

            return value;

        }

        public override string GrabSeries(string htmlDoc)
        {

            string value;
            HtmlAgilityPack.HtmlDocument doc;

            doc = modHTML.CleanHTML(ref htmlDoc);

            value = doc.DocumentNode.SelectSingleNode("//title").InnerText;
            value = Strings.Replace(value, "| FanFiction", "");

            if (!string.IsNullOrEmpty(value))
            {
                value = Strings.Trim(Strings.Mid(value, modUtility.LastPos(value, ",") + 1));
            }

            doc = null;

            return value;

        }

        public override string GrabDate(string htmlDoc, string title)



        {

            string ret = "";

            string author = GrabAuthor(htmlDoc);
            string rss_link = author;
            Story fic = default;

        Retry:
            ;


            if (datasetRSS == null)
            {
                GrabFeed(ref rss_link);
            }

            fic = GetStoryInfoByID(GetStoryID(StoryURL));

            if ((Strings.LCase(fic.Author) ?? "") != (Strings.LCase(author) ?? ""))
            {
                datasetRSS = null;
                goto Retry;
            }

            switch (title ?? "")
            {
                case "Published: ":
                    {
                        ret = fic.PublishDate;
                        break;
                    }
                case "Updated: ":
                    {
                        if (Conversions.ToDate(fic.PublishDate) > Conversions.ToDate(fic.UpdateDate))
                        {
                            ret = fic.PublishDate;
                        }
                        else
                        {
                            ret = fic.UpdateDate;
                        }

                        break;
                    }
            }

            return ret;

        }

        public override string GrabAuthor(string htmldoc)
        {

            string ret;
            HtmlNodeCollection temp;
            HtmlAgilityPack.HtmlDocument doc;

            doc = modHTML.CleanHTML(ref htmldoc);

            temp = modHTML.FindLinksByHref(doc.DocumentNode, "/u/");

            ret = temp[0].InnerText;

            temp = null;
            doc = null;

            return ret;

        }

        public override string GrabBody(string htmldoc)
        {

            string ret = "";

            HtmlAgilityPack.HtmlDocument doc;
            HtmlNodeCollection temp;

            doc = modHTML.CleanHTML(ref htmldoc);

            temp = doc.DocumentNode.SelectNodes("//div[@id='storytext']");

            htmldoc = temp[0].InnerHtml;

            doc = null;
            temp = null;

            ret = htmldoc;

            return ret;

        }

        public override string[] GetChapters(string htmlDoc)


        {

            string[] data;

            data = modHTML.GetOptionValues(htmlDoc, "Chapter Navigation");

            if (data == null)
            {
                data = new string[1];
                data[0] = "Chapter 1";
            }

            Chapters = data;

            return data;

        }

        #endregion

        public override string GrabData(string url)
        {

            string htmldoc;

            htmldoc = base.GrabData(url);

            modStripTags.StripTag(ref htmldoc, "class", "menu", modStripTags.partialM.Yes);
            modStripTags.StripTag(ref htmldoc, "clsas", "dropdown", modStripTags.partialM.Yes);

            modStripTags.StripTag(ref htmldoc, "button");
            modStripTags.StripTag(ref htmldoc, "link");
            modStripTags.StripTag(ref htmldoc, "img");
            modStripTags.StripTag(ref htmldoc, "script");
            modStripTags.StripTag(ref htmldoc, "style");
            modStripTags.StripTag(ref htmldoc, "meta");

            return htmldoc;

        }

        protected override XmlDocument GrabFeedData(ref string rss)
        {

            string txtatom;
            string txtresult;
            XmlDocument xmldoc;

            rss = Strings.Replace(rss, " ", "");
            rss = Strings.Replace(rss, "-", "");
            rss = Strings.Replace(rss, "'", "");

            if (Strings.InStr(rss, HostName) == 0)
            {
                rss = Strings.Replace(rss, ".", "");
                rss = "http://www.fanfiction.net/~" + rss;
            }

            if (Strings.InStr(rss, "atom") == 0)
            {

                txtresult = Program.Browser.DownloadPage(rss);

                try
                {

                    txtresult = Strings.Mid(txtresult, Strings.InStr(txtresult, "id: "));
                    txtresult = Strings.Mid(txtresult, 1, Strings.InStr(txtresult, "<") - 1);
                    if (Strings.InStr(txtresult, ",") > 0)
                    {
                        txtresult = Strings.Mid(txtresult, 1, Strings.InStr(txtresult, ",") - 1);
                    }

                    txtatom = "/atom/u/";
                    txtatom += Strings.Replace(txtresult, "id: ", "");

                    if (string.IsNullOrEmpty(txtatom))
                        rss = "";

                    URL url;
                    url = URLHelper.ExtractUrl(rss);

                    if (Strings.InStr(txtatom, url.Scheme) == 0)
                    {
                        txtatom = url.Scheme + "://" + url.Host + txtatom;
                    }

                    rss = txtatom;
                }

                catch
                {
                    rss = "";
                }

            }

            if (string.IsNullOrEmpty(rss))
            {
                xmldoc = null;
            }
            else
            {
                xmldoc = modXML.DownloadXML(rss);
                xmldoc = modXML.CleanXML(xmldoc);
                xmldoc = modRSS.CleanFeed(xmldoc);
            }

            if (!(xmldoc == null))
            {

            }

            return xmldoc;

        }

        public override string InitialDownload(string link)
        {
            string InitialDownloadRet = default;

            string host;
            string htmlDoc;

            URL URL;

            URL = URLHelper.ExtractUrl(link);

            host = URL.Scheme + "://" + URL.Host + "/s/";
            link = Strings.Replace(link, host, "");
            host = host + Strings.Mid(link, 1, Strings.InStr(link, "/") - 1);
            link = host + "/1/";

            htmlDoc = GrabData(link);

            InitialDownloadRet = htmlDoc;
            return InitialDownloadRet;

        }

        public override string ProcessChapters(string link, int index)



        {
            string ProcessChaptersRet = default;

            string host;
            string temp;
            string[] @params;

            URL url;

            url = URLHelper.ExtractUrl(link);

            host = url.Scheme + "://" + url.Host + "/s/";

            temp = Strings.Replace(link, host, "");
            @params = Strings.Split(temp, "/");

            link = host + @params[0] + "/";

            string htmldoc;

            htmldoc = GrabData(link + (index + 1) + "/");

            ProcessChaptersRet = htmldoc;
            return ProcessChaptersRet;

        }

        public override Story GrabStoryInfo(int idx)
        {

            var dsRSS = datasetRSS;

            var fic = new Story();

            string[] txtSummary;
            string txtResult;
            string category;

            int index;

            string rss_url;

            // Story Name
            fic.Title = Conversions.ToString(dsRSS.Tables["entry"].Rows[idx]["title"]);



            // Story Author
            fic.Author = Conversions.ToString(dsRSS.Tables["author"].Rows[idx][0]);

            rss_url = fic.Author;

            fic.AuthorURL = GetAuthorURL(rss_url);

            // Story Location
            fic.StoryURL = Conversions.ToString(dsRSS.Tables["link"].Rows[idx][1]);

            // Process Summary
            txtResult = Conversions.ToString(Operators.ConcatenateObject(dsRSS.Tables["summary"].Rows[idx][1], Constants.vbCrLf));

            fic.ID = GetStoryID(fic.StoryURL);

            txtResult = Strings.Replace(txtResult, "<hr>", "<br>");
            txtResult = Strings.Replace(txtResult, "<hr size=1>", "<br>");

            txtResult = Strings.Replace(txtResult, ", Words", "<br>Words");
            txtResult = Strings.Replace(txtResult, ", Reviews", "<br>Reviews");

            txtSummary = Strings.Split(txtResult, "<br>");

            // Category
            category = Strings.Mid(txtSummary[1], Strings.Len("Category: "));
            category = Strings.Mid(category, Strings.InStr(category, ">") + 1);
            category = Strings.Replace(category, "</a>", "");

            fic.Category = category;

            if (Strings.InStr(txtSummary[5], "Words") == 0)
            {
                index = 5;
            }
            else
            {
                index = 4;
            }


            // Chapter Count
            fic.ChapterCount = txtSummary[index];

            fic.PublishDate = Conversions.ToString(dsRSS.Tables["entry"].Rows[idx]["published"]);

            // Last Updated
            fic.UpdateDate = Conversions.ToString(dsRSS.Tables["entry"].Rows[idx]["id"]);
            fic.UpdateDate = Strings.Split(fic.UpdateDate, ",")[1];
            fic.UpdateDate = Strings.Split(fic.UpdateDate, ":")[0];
            fic.UpdateDate = Conversions.ToDate(fic.UpdateDate).ToShortDateString();

            // Published
            fic.PublishDate = Conversions.ToString(dsRSS.Tables["entry"].Rows[idx]["published"]);
            fic.PublishDate = Conversions.ToDate(fic.PublishDate).ToShortDateString();

            // Summary

            fic.Summary = txtSummary[Information.UBound(txtSummary)];

            return fic;

        }


        public override string GetStoryID(string link)
        {

            string ret;
            URL hl;
            string[] parms;

            hl = URLHelper.ExtractUrl(link);

            parms = Strings.Split(hl.URI, "/");

            ret = parms[2];

            return ret;

        }

        public override string GetStoryURL(string id)
        {

            string link;

            link = "http://www.fanfiction.net/s/" + id + "/1/";

            return link;

        }

        public override string GetAuthorURL(string link)
        {

            string ret;

            if (Strings.InStr(link, HostName) == 0)
            {
                link = Strings.Replace(link, ".", "");
                link = "http://www.fanfiction.net/~" + link;
            }

            ret = Strings.Replace(link, "atom/", "");

            return ret;

        }

        #region Properties

        public override string ErrorMessage
        {
            get
            {
                return "story not found";
            }
        }

        public override string HostName
        {
            get
            {
                return "fanfiction.net";
            }
        }

        public override string Name
        {
            get
            {
                return "FFNet";
            }
        }

        #endregion

    }
}