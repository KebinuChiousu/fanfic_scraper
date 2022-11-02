using System.Xml;
using HtmlAgilityPack;
using HtmlScraper.BusinessLogic.Sites.Base;
using HtmlScraper.Utility.Browser.HTML;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using static System.Web.HttpUtility;

namespace HtmlScraper.BusinessLogic.Sites
{

    public class HPFFA : clsFanfic
    {

        private Story info;

        #region Chapter Navigation

        public override string ProcessChapters(string link, int index)
        {

            string htmldoc;

            link += "&";
            link += "chapter=" + (index + 1);

            htmldoc = GrabData(link);

            return htmldoc;

        }

        public override string[] GetChapters(string htmlDoc)
        {

            string[] chapters;

            HtmlAgilityPack.HtmlDocument doc = null;
            HtmlAgilityPack.HtmlDocument tdoc = null;
            HtmlNodeCollection nodes;

            string story_url;

            doc = modHTML.CleanHTML(ref htmlDoc);

            nodes = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "id", "pagetitle", false);

            string arghtml = nodes[0].InnerHtml;
            tdoc = modHTML.CleanHTML(ref arghtml);
            nodes[0].InnerHtml = arghtml;

            nodes = modHTML.FindLinksByHref(tdoc.DocumentNode, "viewstory.php");

            story_url = nodes[0].Attributes["href"].Value;

            nodes = modHTML.FindLinksByHref(doc.DocumentNode, story_url + "&amp;chapter=");

            chapters = new string[nodes.Count];

            for (int node_idx = 0, loopTo = nodes.Count - 1; node_idx <= loopTo; node_idx++)
                chapters[node_idx] = HtmlDecode(nodes[node_idx].Attributes["href"].Value);

            GetStoryInfo(htmlDoc);

            return chapters;

        }

        private void GetStoryInfo(string htmlDoc)
        {

            string html;
            HtmlAgilityPack.HtmlDocument doc = null;
            HtmlAgilityPack.HtmlDocument tdoc = null;

            HtmlNodeCollection temp;

            string dummy;
            string[] summary;

            var ch_idx = default(int);
            var p_idx = default(int);
            var u_idx = default(int);

            int idx;

            doc = modHTML.CleanHTML(ref htmlDoc);

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "id", "pagetitle", false);
            string arghtml = temp[0].InnerHtml;
            tdoc = modHTML.CleanHTML(ref arghtml);
            temp[0].InnerHtml = arghtml;
            temp = tdoc.DocumentNode.SelectNodes("//a");

            info.Category = "Harry Potter";

            info.StoryURL = "http://www." + HostName + "/stories/" + temp[0].Attributes["href"].Value;
            info.AuthorURL = "http://www." + HostName + "/stories/" + temp[1].Attributes["href"].Value;

            info.Title = temp[0].InnerText;
            info.Author = temp[1].InnerText;

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "class", "content", false);

            html = temp[2].InnerHtml;

            doc = modHTML.CleanHTML(ref html);

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "span", "class", "label", false);

            for (int temp_idx = 0, loopTo = temp.Count - 1; temp_idx <= loopTo; temp_idx++)


                html = Strings.Replace(html, temp[temp_idx].OuterHtml, "|" + temp[temp_idx].InnerText);

            summary = Strings.Split(html, "|");

            var loopTo1 = Information.UBound(summary);
            for (idx = 0; idx <= loopTo1; idx++)
            {
                if (Strings.InStr(summary[idx], "Chapters:") != 0)
                {
                    ch_idx = idx;
                }
                if (Strings.InStr(summary[idx], "Published:") != 0)
                {
                    p_idx = idx;
                }
                if (Strings.InStr(summary[idx], "Updated:") != 0)
                {
                    u_idx = idx;
                }
            }


            tdoc = modHTML.CleanHTML(ref summary[ch_idx]);

            dummy = tdoc.DocumentNode.InnerText;
            dummy = Strings.Mid(dummy, Strings.InStr(dummy, ":") + 1);

            info.ChapterCount = Strings.Trim(dummy);

            tdoc = modHTML.CleanHTML(ref summary[p_idx]);
            dummy = tdoc.DocumentNode.InnerText;

            dummy = Strings.Replace(dummy, "Published:", "");

            if (Strings.InStr(dummy, ">") > 0)
            {

                dummy = Strings.Mid(dummy, Strings.InStr(dummy, ">") + 1);
                dummy = Strings.Mid(dummy, 1, Strings.InStr(dummy, "<") - 1);

            }

            info.PublishDate = Conversions.ToDate(dummy).ToShortDateString();

            tdoc = modHTML.CleanHTML(ref summary[u_idx]);
            dummy = tdoc.DocumentNode.InnerText;

            if (Strings.InStr(dummy, ">") > 0)
            {

                dummy = Strings.Mid(dummy, Strings.InStr(dummy, ">") + 1);
                dummy = Strings.Mid(dummy, 1, Strings.InStr(dummy, "<") - 1);

            }

            dummy = Strings.Replace(dummy, "Updated:", "");

            info.UpdateDate = Conversions.ToDate(dummy).ToShortDateString();

            info.ID = GetStoryID(info.StoryURL);

        }

        #endregion

        #region Data Extraction

        public override string GrabTitle(string htmlDoc)
        {

            HtmlAgilityPack.HtmlDocument doc = null;
            HtmlNodeCollection nodes;

            string ret = "";

            if (Strings.InStr(htmlDoc, "<select class=\"textbox\" name=\"chapter\"") == 0)
            {
                ret = info.Title;
            }
            else
            {

                doc = modHTML.CleanHTML(ref htmlDoc);

                nodes = modHTML.FindNodesByAttribute(doc.DocumentNode, "select", "name", "chapter", false);

                string arghtml = nodes[0].InnerHtml;
                doc = modHTML.CleanHTML(ref arghtml);
                nodes[0].InnerHtml = arghtml;

                nodes = modHTML.FindNodesByAttribute(doc.DocumentNode, "option", "selected");

                if (!(nodes == null))
                {
                    if (nodes.Count > 0)
                    {
                        ret = info.Title;
                        ret += "<br /><br />";
                        ret += nodes[0].NextSibling.InnerText;
                    }
                    else
                    {
                        ret = info.Title;
                    }
                }
                else
                {
                    ret = info.Title;
                }

                return ret;

            }

            return ret;

        }

        public override string GrabSeries(string htmlDoc)
        {
            return info.Category;
        }

        public override string GrabDate(string htmlDoc, string title)
        {

            string ret = "";

            switch (title ?? "")
            {
                case "Published: ":
                    {
                        ret = info.PublishDate;
                        break;
                    }
                case "Updated: ":
                    {
                        ret = info.UpdateDate;
                        break;
                    }
            }

            return ret;

        }

        public override string GrabAuthor(string htmlDoc)
        {
            return info.Author;
        }

        public override string GrabBody(string htmlDoc)
        {

            HtmlAgilityPack.HtmlDocument doc = null;
            HtmlNodeCollection nodes;
            string ret = "";

            doc = modHTML.CleanHTML(ref htmlDoc);

            nodes = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "id", "story");

            ret = nodes[0].InnerHtml;

            return ret;

        }

        public override string GetAuthorURL(string link)
        {
            return info.AuthorURL;
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

            return "http://www." + HostName + "/stories/viewstory.php?sid=" + id;

        }


        #endregion

        #region Download Routines

        protected override XmlDocument GrabFeedData(ref string rss)
        {

            string html;
            HtmlAgilityPack.HtmlDocument doc = null;
            HtmlAgilityPack.HtmlDocument tdoc = null;

            HtmlNodeCollection nodes;
            HtmlNodeCollection temp;

            string dummy;

            int temp_idx;
            int node_idx;

            string author_url;

            string[] summary;

            var ch_idx = default(int);

            Story[] fic;

            int idx;

            XmlDocument xmldoc = null;

            author_url = rss;

            if (Strings.InStr(rss, "action=storiesby") == 0 & Strings.InStr(rss, "viewuser.php") > 0)
            {
                rss += "&action=storiesby";
            }
            html = GrabData(rss);

            doc = modHTML.CleanHTML(ref html);

            nodes = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "class", "listbox", true);

            fic = new Story[nodes.Count];

            var loopTo = nodes.Count - 1;
            for (node_idx = 0; node_idx <= loopTo; node_idx++)
            {

                string arghtml = nodes[node_idx].InnerHtml;
                doc = modHTML.CleanHTML(ref arghtml);
                nodes[node_idx].InnerHtml = arghtml;

                temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "class", "title", false);
                string arghtml1 = temp[0].InnerHtml;
                tdoc = modHTML.CleanHTML(ref arghtml1);
                temp[0].InnerHtml = arghtml1;
                temp = tdoc.DocumentNode.SelectNodes("//a");

                fic[node_idx].Category = "Harry Potter";

                fic[node_idx].StoryURL = "http://www." + HostName + "/stories/" + temp[0].Attributes["href"].Value;
                fic[node_idx].AuthorURL = "http://www." + HostName + "/stories/" + temp[1].Attributes["href"].Value;

                fic[node_idx].Title = temp[0].InnerText;
                fic[node_idx].Author = temp[1].InnerText;


                temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "class", "tail", false);

                dummy = temp[0].InnerHtml;

                tdoc = modHTML.CleanHTML(ref dummy);

                temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "span", "class", "label", false);

                var loopTo1 = temp.Count - 1;
                for (temp_idx = 0; temp_idx <= loopTo1; temp_idx++)


                    dummy = Strings.Replace(dummy, temp[temp_idx].OuterHtml, "|" + temp[temp_idx].InnerText);

                dummy = Strings.Replace(dummy, "</span>", "");
                summary = Strings.Split(dummy, "|");

                fic[node_idx].PublishDate = Conversions.ToDate(Strings.Mid(summary[1], Strings.InStr(summary[1], ":") + 1)).ToShortDateString();
                fic[node_idx].UpdateDate = Conversions.ToDate(Strings.Mid(summary[2], Strings.InStr(summary[2], ":") + 1)).ToShortDateString();

                temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "class", "content", false);

                html = temp[0].InnerHtml;

                doc = modHTML.CleanHTML(ref html);

                temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "span", "class", "label", false);

                var loopTo2 = temp.Count - 1;
                for (temp_idx = 0; temp_idx <= loopTo2; temp_idx++)


                    html = Strings.Replace(html, temp[temp_idx].OuterHtml, "|" + temp[temp_idx].InnerText);

                summary = Strings.Split(html, "|");

                tdoc = modHTML.CleanHTML(ref summary[1]);

                dummy = tdoc.DocumentNode.InnerText + Constants.vbCrLf;

                var loopTo3 = Information.UBound(summary);
                for (idx = 0; idx <= loopTo3; idx++)
                {
                    if (Strings.InStr(summary[idx], "Chapters:") != 0)
                    {
                        ch_idx = idx;
                        break;
                    }
                }

                var loopTo4 = ch_idx - 2;
                for (idx = 2; idx <= loopTo4; idx++)
                {

                    if (idx != 4)
                    {

                        tdoc = modHTML.CleanHTML(ref summary[idx]);
                        dummy += HtmlDecode(tdoc.DocumentNode.InnerText) + Constants.vbCrLf;

                    }

                }

                fic[node_idx].Summary = dummy;

                tdoc = modHTML.CleanHTML(ref summary[ch_idx]);

                dummy = tdoc.DocumentNode.InnerText;
                dummy = Strings.Left(dummy, Strings.InStr(dummy, "Table") - 1);
                dummy = Strings.Mid(dummy, Strings.InStr(dummy, ":") + 1);

                fic[node_idx].ChapterCount = Strings.Trim(dummy);

                fic[node_idx].ID = GetStoryID(fic[node_idx].StoryURL);

            }

            html = GenerateAtomFeed(fic);

            doc = modHTML.CleanHTML(ref html);

            html = doc.DocumentNode.OuterHtml;

            xmldoc = new XmlDocument();

            xmldoc.LoadXml(html);

            return xmldoc;

        }

        #endregion


        #region RSS Routines

        public override Story GrabStoryInfo(int idx)
        {

            var dsRSS = datasetRSS;

            var fic = new Story();

            string[] temp;

            // Story Name
            fic.Title = Conversions.ToString(dsRSS.Tables["entry"].Rows[idx]["title"]);

            // Story Author
            fic.Author = Conversions.ToString(dsRSS.Tables["author"].Rows[idx][0]);
            // Story Author URL
            fic.AuthorURL = Conversions.ToString(dsRSS.Tables["author"].Rows[idx][1]);

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

        #region Properties

        public override string HostName
        {
            get
            {
                return "hpfanficarchive.com";
            }
        }

        public override string Name
        {
            get
            {
                return "HPFFA";
            }
        }

        public override string ErrorMessage
        {
            get
            {
                return "does not exist on this archive";
            }
        }

        #endregion

    }
}