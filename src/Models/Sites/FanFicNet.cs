using System;
using System.Data;
using Microsoft.VisualBasic;
using System.Xml;
using HtmlAgilityPack;

using web_scraper.Models.Sites.Base;
using web_scraper.Models.Utility;

namespace web_scraper.Models.Sites
{
    class FanFicNet : Fanfic
    {
        public override string GrabTitle(string htmldoc
                        )
        {
            string value;
            HtmlDocument doc;
            int ch = 0;
            string title;

            doc = HtmlHelper.CleanHTML(ref htmldoc);

            value = doc.DocumentNode.SelectSingleNode("//title").InnerText;
            value = Strings.Replace(value, "| FanFiction", "");

            ch = Strings.InStr(value, "Chapter");

            if (ch > 0)
            {
                title = Strings.Trim(Strings.Mid(value, 1, ch - 1));
                value = Strings.Replace(value, title, "");
                ch = Functions.LastPos(value, ",");
                value = title + "<br><br>" + Strings.Mid(value, 1, ch - 1);
            }

            doc = null/* TODO Change to default(_) if this is not a reference type */;

            return value;
        }

        public override string GrabSeries(string htmlDoc)
        {
            string value;
            HtmlDocument doc;

            doc = HtmlHelper.CleanHTML(ref htmlDoc);

            value = doc.DocumentNode.SelectSingleNode("//title").InnerText;
            value = Strings.Replace(value, "| FanFiction", "");

            if (value != "")
                value = Strings.Trim(Strings.Mid(value, Functions.LastPos(value, ",") + 1));

            doc = null/* TODO Change to default(_) if this is not a reference type */;

            return value;
        }

        public override DateTime? GrabDate(string htmlDoc, string title
                        )
        {
            DateTime? ret = null;

            string author = GrabAuthor(htmlDoc);
            string rss_link = author;
            Story fic = new Story();

        Retry:
            
            if (Functions.IsNothing(datasetRSS))
                base.GrabFeed(ref rss_link);

            fic = GetStoryInfoByID(GetStoryID(StoryURL));

            if (Strings.LCase(fic.Author) != Strings.LCase(author))
            {
                datasetRSS = null;
                goto Retry;
            }

            switch (title)
            {
                case "Published: ":
                    {
                        ret = fic.PublishDate;
                        break;
                    }

                case "Updated: ":
                    {
                        if ((DateTime)fic.PublishDate > (DateTime)fic.UpdateDate)
                            ret = fic.PublishDate;
                        else
                            ret = fic.UpdateDate;
                        break;
                    }
            }

            return ret;
        }

        public override string GrabAuthor(string htmldoc)
        {
            string ret;
            HtmlNodeCollection temp;
            HtmlDocument doc;

            doc = HtmlHelper.CleanHTML(ref htmldoc);

            temp = HtmlHelper.FindLinksByHref(doc.DocumentNode, "/u/");

            ret = temp[0].InnerText;

            temp = null;
            doc = null;

            return ret;
        }

        public override string GrabBody(string htmldoc)
        {
            string ret = "";

            HtmlDocument doc;
            HtmlNodeCollection temp;

            doc = HtmlHelper.CleanHTML(ref htmldoc);

            temp = doc.DocumentNode.SelectNodes("//div[@id='storytext']");

            htmldoc = temp[0].InnerHtml;

            doc = null/* TODO Change to default(_) if this is not a reference type */;
            temp = null/* TODO Change to default(_) if this is not a reference type */;

            ret = htmldoc;

            return ret;
        }

        public override string[] GetChapters(string htmlDoc
                            )
        {
            string[] data;

            data = HtmlHelper.GetOptionValues(htmlDoc, "Chapter Navigation");

            if (Information.IsNothing(data))
            {
                data = new string[1];
                data[0] = "Chapter 1";
            }

            this.Chapters = data;

            return data;
        }


        public override string GrabData(string url)
        {
            string htmldoc;

            htmldoc = base.GrabData(url);

            HtmlHelper.StripTag(ref htmldoc, "class", "menu", true);
            HtmlHelper.StripTag(ref htmldoc, "class", "dropdown", true);

            HtmlHelper.StripTag(ref htmldoc, "button");
            HtmlHelper.StripTag(ref htmldoc, "link");
            HtmlHelper.StripTag(ref htmldoc, "img");
            HtmlHelper.StripTag(ref htmldoc, "script");
            HtmlHelper.StripTag(ref htmldoc, "style");
            HtmlHelper.StripTag(ref htmldoc, "meta");

            return htmldoc;
        }

        protected override System.Xml.XmlDocument GrabFeedData(ref string rss)
        {
            string txtatom;
            string txtresult;
            XmlDocument xmldoc;

            rss = Strings.Replace(rss, " ", "");
            rss = Strings.Replace(rss, "-", "");
            rss = Strings.Replace(rss, "'", "");

            if (Strings.InStr(rss, this.HostName) == 0)
            {
                rss = Strings.Replace(rss, ".", "");
                rss = "http://www.fanfiction.net/~" + rss;
            }

            if ((Strings.InStr(rss, "atom") == 0))
            {
                txtresult = Browser.DownloadPage(rss);

                try
                {
                    txtresult = Strings.Mid(txtresult, Strings.InStr(txtresult, "id: "));
                    txtresult = Strings.Mid(txtresult, 1, Strings.InStr(txtresult, "<") - 1);
                    if (Strings.InStr(txtresult, ",") > 0)
                        txtresult = Strings.Mid(txtresult, 1, Strings.InStr(txtresult, ",") - 1);

                    txtatom = "/atom/u/";
                    txtatom += Strings.Replace(txtresult, "id: ", "");

                    if (txtatom == "")
                        rss = "";

                    Url url;
                    url = UrlHelper.ExtractUrl(rss);

                    if (Strings.InStr(txtatom, url.Scheme) == 0)
                        txtatom = url.Scheme + "://" + url.Host + txtatom;

                    rss = txtatom;
                }
                catch
                {
                    rss = "";
                }
            }

            if (rss == "")
                xmldoc = null;
            else
            {
                xmldoc = Xml.DownloadXML(rss);
                //xmldoc = Xml.CleanXML(xmldoc);
                //xmldoc = Xml.CleanFeed(xmldoc);
            }

            if (!Information.IsNothing(xmldoc))
            {
            }

            return xmldoc;
        }

        public override string InitialDownload(string link)
        {
            string host;
            string htmlDoc;

            Url URL;

            URL = UrlHelper.ExtractUrl(link);

            host = URL.Scheme + "://" + URL.Host + "/s/";
            link = Strings.Replace(link, host, "");
            host = host + Strings.Mid(link, 1, Strings.InStr(link, "/") - 1);
            link = host + "/1/";

            htmlDoc = GrabData(link);

            return htmlDoc;
        }

        public override string ProcessChapters(string link, int index
                                )
        {
            string host;
            string temp;
            string[] @params;

            Url url;

            url = UrlHelper.ExtractUrl(link);

            host = url.Scheme + "://" + url.Host + "/s/";

            temp = Strings.Replace(link, host, "");
            @params = Strings.Split(temp, "/");

            link = host + @params[0] + "/";

            string htmldoc;

            htmldoc = GrabData(link + (index + 1) + "/");

            return htmldoc;
        }

        public override Story GrabStoryInfo(int idx)
        {
            DataSet dsRSS = base.datasetRSS;
            DataRow dr;

            Story fic = new Story();

            string[] txtSummary;
            string txtResult;
            string category;

            int index;

            string rss_url;

            // Story Name
            dr = dsRSS.Tables["entry"].Rows[idx];
            fic.Title = dr["title"].ToString();

            // Story Author
            dr = dsRSS.Tables["author"].Rows[idx];
            fic.Author = dr[0].ToString();

            rss_url = fic.Author;

            fic.AuthorURL = GetAuthorURL(rss_url);

            // Story Location
            dr = dsRSS.Tables["link"].Rows[idx];
            fic.StoryURL = dr[1].ToString();

            // Process Summary
            dr = dsRSS.Tables["summary"].Rows[idx];
            txtResult = dr[1] + Constants.vbCrLf;

            fic.ID = this.GetStoryID(fic.StoryURL);

            txtResult = Strings.Replace(txtResult, "<hr>", "<br>");
            txtResult = Strings.Replace(txtResult, "<hr size=1>", "<br>");

            txtResult = Strings.Replace(txtResult, ", Words", "<br>Words");
            txtResult = Strings.Replace(txtResult, ", Reviews", "<br>Reviews");

            txtSummary = Strings.Split(txtResult, "<br>");

            // Category
            category = Strings.Mid(txtSummary[1], Strings.Len("Category: "));
            category = Strings.Mid(category, (Strings.InStr(category, ">") + 1));
            category = Strings.Replace(category, "</a>", "");

            fic.Category = category;

            if (Strings.InStr(txtSummary[5], "Words") == 0)
                index = 5;
            else
                index = 4;

            dr = dsRSS.Tables["entry"].Rows[idx];

            // Chapter Count
            fic.ChapterCount = int.Parse(txtSummary[index]);

            // Publish Date
            fic.PublishDate = (DateTime?)dr["published"];

            string updDate = dr["id"].ToString().Split(",")[1].Split(":")[0];

            // Last Update
            fic.UpdateDate = Convert.ToDateTime(updDate);

            // Published
            fic.PublishDate = (DateTime?)dr["published"];

            // Summary
            fic.Summary = txtSummary[Information.UBound(txtSummary)];

            return fic;
        }


        public override string GetStoryID(string link)
        {
            string ret;
            URL hl;
            string[] parms;

            hl = ExtractUrl(link);

            parms = Split(hl.URI, "/");

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

            if (Strings.InStr(link, this.HostName) == 0)
            {
                link = Strings.Replace(link, ".", "");
                link = "http://www.fanfiction.net/~" + link;
            }

            ret = Strings.Replace(link, "atom/", "");

            return ret;
        }


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
    }

}
