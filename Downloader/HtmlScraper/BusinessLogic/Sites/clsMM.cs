using System.Xml;
using HtmlAgilityPack;
using HtmlScraper.BusinessLogic.Sites.Base;
using HtmlScraper.Utility.Browser.HTML;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using static System.Web.HttpUtility;

namespace HtmlScraper.BusinessLogic.Sites
{

    public class MM : clsFanfic
    {

        #region Downloading HTML

        public override string GrabData(string url)
        {

            string htmldoc;

            htmldoc = base.GrabData(url);

            return htmldoc;

        }

        #endregion

        #region RSS

        protected override XmlDocument GrabFeedData(ref string rss)
        {

            string html;
            HtmlAgilityPack.HtmlDocument doc = null;

            HtmlNodeCollection nodes;
            HtmlNodeCollection temp;

            string dummy;

            string title;
            string link;

            int node_idx;

            string author_url;

            string[] summary;
            Story[] fic;

            int idx;

            XmlDocument xmldoc = null;

            author_url = rss;

            html = GrabData(rss);

            doc = modHTML.CleanHTML(ref html);

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "select", "name", "id", false);
            html = temp[0].InnerHtml;

            doc = modHTML.CleanHTML(ref html);

            nodes = doc.DocumentNode.SelectNodes("//option");

            fic = new Story[nodes.Count];

            var loopTo = nodes.Count - 1;
            for (node_idx = 0; node_idx <= loopTo; node_idx++)
            {

                fic[node_idx].ID = nodes[node_idx].Attributes["value"].Value;
                fic[node_idx].StoryURL = "http://mediaminer.org/fanfic/view_st.php?id=" + fic[node_idx].ID;

                title = nodes[node_idx].NextSibling.InnerText;
                if (Strings.InStr(title, "(") > 0)
                {
                    fic[node_idx].Title = Strings.Mid(title, 1, Strings.InStr(title, "(") - 1);
                }
                else
                {
                    fic[node_idx].Title = title;
                }

                html = GrabData(fic[node_idx].StoryURL);
                doc = modHTML.CleanHTML(ref html);

                temp = doc.DocumentNode.SelectNodes("//table");
                string arghtml = temp[4].InnerHtml;
                doc = modHTML.CleanHTML(ref arghtml);
                temp[4].InnerHtml = arghtml;
                temp = doc.DocumentNode.SelectNodes("//td");
                dummy = temp[1].InnerHtml;

                summary = Strings.Split(dummy, "<br />");

                doc = modHTML.CleanHTML(ref summary[0]);
                temp = doc.DocumentNode.SelectNodes("//a");

                fic[node_idx].Category = temp[0].InnerText;

                idx = 2;
                dummy = summary[idx];
                doc = modHTML.CleanHTML(ref dummy);
                temp = doc.DocumentNode.SelectNodes("//a");

                if (temp == null)
                {
                    idx = 3;
                    doc = modHTML.CleanHTML(ref summary[idx]);
                    temp = doc.DocumentNode.SelectNodes("//a");
                }

                fic[node_idx].Author = temp[0].InnerText;
                fic[node_idx].AuthorURL = "http://" + HostName + temp[0].Attributes["href"].Value;

                if (idx == 3)
                {
                    idx = 2;
                }
                else
                {
                    idx = 3;
                }

                dummy = Strings.Split(summary[idx], "</b>")[1];
                dummy = Strings.Trim(Strings.Split(dummy, "|")[0]);
                if (Strings.InStr(dummy, "DT") == Strings.Len(dummy) - 1 | Strings.InStr(dummy, "ST") == Strings.Len(dummy) - 1)
                {
                    dummy = Strings.Mid(dummy, 1, Strings.Len(dummy) - 3);
                }

                fic[node_idx].UpdateDate = Conversions.ToDate(dummy).ToShortDateString();

                doc = modHTML.CleanHTML(ref summary[6]);
                dummy = doc.DocumentNode.InnerText;
                dummy = HtmlDecode(dummy);
                dummy = HtmlDecode(dummy);
                dummy = Strings.Replace(dummy, Conversions.ToString(Strings.Chr(160)), "");

                fic[node_idx].Summary = dummy;

                doc = modHTML.CleanHTML(ref html);
                temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "select", "name", "cid", false);

                if (temp == null)
                {
                    fic[node_idx].ChapterCount = 1.ToString();
                    fic[node_idx].PublishDate = fic[node_idx].UpdateDate;
                }
                else
                {

                    dummy = temp[0].InnerHtml;
                    doc = modHTML.CleanHTML(ref dummy);
                    temp = doc.DocumentNode.SelectNodes("//option");

                    fic[node_idx].ChapterCount = temp.Count.ToString();

                    link = "http://mediaminer.org/fanfic/view_ch.php?cid=";
                    link += temp[0].Attributes["value"].Value;
                    link += "&id=" + fic[node_idx].ID;

                    html = GrabData(link);
                    doc = modHTML.CleanHTML(ref html);

                    temp = doc.DocumentNode.SelectNodes("//table");
                    string arghtml1 = temp[4].InnerHtml;
                    doc = modHTML.CleanHTML(ref arghtml1);
                    temp[4].InnerHtml = arghtml1;
                    temp = doc.DocumentNode.SelectNodes("//td");
                    html = temp[1].InnerHtml;

                    summary = Strings.Split(html, "<br />");

                    dummy = Strings.Split(summary[3], "</b>")[1];
                    dummy = Strings.Trim(Strings.Split(dummy, "|")[0]);
                    if (Strings.InStr(dummy, "DT") == Strings.Len(dummy) - 1 | Strings.InStr(dummy, "ST") == Strings.Len(dummy) - 1)
                    {
                        dummy = Strings.Mid(dummy, 1, Strings.Len(dummy) - 3);
                    }

                    fic[node_idx].PublishDate = Conversions.ToDate(dummy).ToShortDateString();

                }

            }

            html = GenerateAtomFeed(fic);

            doc = modHTML.CleanHTML(ref html);

            html = doc.DocumentNode.OuterHtml;

            xmldoc = new XmlDocument();

            xmldoc.LoadXml(html);

            return xmldoc;


        }

        public override Story GrabStoryInfo(int idx)
        {

            var dsRSS = datasetRSS;

            var fic = new Story();

            string[] temp;

            // Story Name
            fic.Title = Conversions.ToString(dsRSS.Tables["entry"].Rows[idx]["title"]);

            // Story Author
            fic.Author = Conversions.ToString(dsRSS.Tables["author"].Rows[idx][0]);
            // Story Location
            fic.StoryURL = Conversions.ToString(dsRSS.Tables["link"].Rows[idx][1]);

            fic.ID = GetStoryID(fic.StoryURL);

            fic.PublishDate = Conversions.ToString(Conversions.ToDate(dsRSS.Tables["entry"].Rows[idx]["published"]));

            fic.UpdateDate = Conversions.ToString(Conversions.ToDate(dsRSS.Tables["entry"].Rows[idx]["updated"]));

            temp = Strings.Split(Conversions.ToString(dsRSS.Tables["entry"].Rows[idx]["id"]), ":");

            fic.Category = temp[1];
            fic.ChapterCount = temp[2];

            fic.Summary = Conversions.ToString(dsRSS.Tables["summary"].Rows[idx]["summary_Text"]);

            return fic;

        }

        #endregion

        #region Chapter Navigation

        public override string[] GetChapters(string htmlDoc)
        {

            string[] ret;

            HtmlAgilityPack.HtmlDocument doc;
            HtmlNodeCollection temp;

            int idx;

            // Clean up html into xhtml
            doc = modHTML.CleanHTML(ref htmlDoc);

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "select", "name", "cid", false);

            if (!(temp == null))
            {

                htmlDoc = temp[0].InnerHtml;
                doc = modHTML.CleanHTML(ref htmlDoc);
                temp = doc.DocumentNode.SelectNodes("//option");

                ret = new string[temp.Count];
                var loopTo = temp.Count - 1;
                for (idx = 0; idx <= loopTo; idx++)
                    ret[idx] = temp[idx].Attributes["value"].Value;
            }
            else
            {
                ret = new string[1];
                ret[0] = "Chapter 1";
            }

            Chapters = ret;

            return ret;

        }

        public override string ProcessChapters(string link, int index)
        {

            string htmldoc;

            if (Information.UBound(Chapters) > 1)
            {
                link = GetChapterURL(link, Conversions.ToInteger(Chapters[index]));
            }

            htmldoc = GrabData(link);

            return htmldoc;

        }

        private string GetChapterURL(string link, int index)
        {

            string ret;

            ret = "http://mediaminer.org/fanfic/view_ch.php?cid=";
            ret += index.ToString();
            ret += "&";
            ret += "id=";
            ret += GetStoryID(link);

            return ret;

        }

        public override string GetAuthorURL(string link)
        {

            string ret;

            string[] temp;

            if (Strings.InStr(link, "/src.php/u") > 0)
            {

                temp = Strings.Split(link, "/");

                link += "http://mediaminer.org/user_info.php/" + temp[Information.UBound(temp)];

                ret = link;
            }
            else if (Strings.InStr(link, "user_info.php/") > 0)
            {
                ret = link;
            }
            else
            {
                return "";
            }

            return ret;

        }

        public override string GetStoryID(string link)
        {

            string ret = "";
            URL url;

            url = URLHelper.ExtractUrl(link);
            ret = url.Query[0].Value;

            return ret;

        }

        public override string GetStoryURL(string id)
        {

            string link;

            link = "http://mediaminer.org/fanfic/view_st.php?id=" + id;

            return link;

        }

        #endregion

        #region HTML Processing

        private string[] GrabHeaderInfo(string htmldoc)
        {

            HtmlAgilityPack.HtmlDocument doc = null;
            HtmlNodeCollection temp;

            string dummy;
            string[] summary;

            doc = modHTML.CleanHTML(ref htmldoc);

            temp = doc.DocumentNode.SelectNodes("//table");
            string arghtml = temp[4].InnerHtml;
            doc = modHTML.CleanHTML(ref arghtml);
            temp[4].InnerHtml = arghtml;
            temp = doc.DocumentNode.SelectNodes("//td");
            dummy = temp[1].InnerHtml;

            summary = Strings.Split(dummy, "<br />");

            doc = null;
            temp = null;

            return summary;

        }

        public override string GrabAuthor(string htmlDoc)
        {

            string author;

            int idx;

            HtmlAgilityPack.HtmlDocument doc = null;
            HtmlNodeCollection temp;

            string dummy;
            string[] summary;

            summary = GrabHeaderInfo(htmlDoc);

            idx = 2;
            dummy = summary[idx];
            doc = modHTML.CleanHTML(ref dummy);
            temp = doc.DocumentNode.SelectNodes("//a");

            if (temp == null)
            {
                idx = 3;
                doc = modHTML.CleanHTML(ref summary[idx]);
                temp = doc.DocumentNode.SelectNodes("//a");
            }

            author = temp[0].InnerText;

            doc = null;
            temp = null;

            return author;

        }

        public override string GrabBody(string htmldoc)
        {

            int idx;

            string ret;

            string content;

            HtmlAgilityPack.HtmlDocument doc = null;
            HtmlNodeCollection temp;

            doc = modHTML.CleanHTML(ref htmldoc);

            // <div align="left" style=" padding: 0.00mm 0.00mm 0.00mm 0.00mm;">

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "style", "padding: 0.00mm 0.00mm 0.00mm 0.00mm;", true);

            ret = "<html>";
            ret += "<body>";

            var loopTo = temp.Count - 1;
            for (idx = 0; idx <= loopTo; idx++)
            {

                content = HtmlDecode(HtmlDecode(temp[idx].OuterHtml));
                content = Strings.Replace(content, "<br />", "<br /><br />");

                ret += content;
                ret += "<br /><br />";


            }

            ret += "</body>";
            ret += "</html>";

            doc = null;
            temp = null;

            return ret;

        }

        public override string GrabDate(string htmlDoc, string title)
        {

            string ret;

            int idx;

            HtmlAgilityPack.HtmlDocument doc = null;
            HtmlNodeCollection temp;

            string dummy;
            string[] summary;

            summary = GrabHeaderInfo(htmlDoc);

            idx = 2;
            dummy = summary[idx];
            doc = modHTML.CleanHTML(ref dummy);
            temp = doc.DocumentNode.SelectNodes("//a");

            if (temp == null)
            {
                idx = 3;
            }

            if (idx == 3)
            {
                idx = 2;
            }
            else
            {
                idx = 3;
            }

            dummy = Strings.Split(summary[idx], "</b>")[1];
            dummy = Strings.Trim(Strings.Split(dummy, "|")[0]);
            if (Strings.InStr(dummy, "DT") == Strings.Len(dummy) - 1 | Strings.InStr(dummy, "ST") == Strings.Len(dummy) - 1)
            {
                dummy = Strings.Mid(dummy, 1, Strings.Len(dummy) - 3);
            }

            ret = Conversions.ToDate(dummy).ToShortDateString();

            doc = null;
            temp = null;

            return ret;

        }

        public override string GrabSeries(string htmlDoc)
        {

            string series;

            HtmlAgilityPack.HtmlDocument doc = null;
            HtmlNodeCollection temp;

            string[] summary;

            summary = GrabHeaderInfo(htmlDoc);

            doc = modHTML.CleanHTML(ref summary[0]);
            temp = doc.DocumentNode.SelectNodes("//a");

            series = temp[0].InnerText;

            doc = null;
            temp = null;

            return series;

        }

        public override string GrabTitle(string htmlDoc)
        {

            string ret;
            int pos;

            HtmlAgilityPack.HtmlDocument doc = null;
            HtmlNodeCollection temp;

            doc = modHTML.CleanHTML(ref htmlDoc);
            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "td", "class", "ffh", false);

            ret = temp[0].InnerText;

            pos = Strings.InStr(ret, ":");

            if (pos > 1)
            {
                ret = Strings.Mid(ret, 1, pos - 1);
            }

            doc = null;
            temp = null;

            return ret;

        }

        #endregion

        #region Properties

        public override string ErrorMessage
        {
            get
            {

                string ret;

                ret = "<h2 style=\"margin: 5px;\">Jump to Anime Series:</h2>";

                return ret;

            }
        }

        public override string HostName
        {
            get
            {
                return "mediaminer.org";
            }
        }

        public override string Name
        {
            get
            {
                return "MM";
            }
        }

        #endregion

    }
}