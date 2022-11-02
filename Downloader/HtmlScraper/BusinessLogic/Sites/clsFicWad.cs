using System.Xml;
using HtmlAgilityPack;
using HtmlScraper.BusinessLogic.Sites.Base;
using HtmlScraper.Utility;
using HtmlScraper.Utility.Browser.HTML;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace HtmlScraper.BusinessLogic.Sites
{

    class FicWad : clsFanfic
    {

        #region Downloading HTML

        private bool AgeCheck = true;
        private string Username;
        private string Password;
        private string cookie_name = "ficwad_com.cookie";

        public override string GrabData(string url)
        {

            string html;
            HtmlAgilityPack.HtmlDocument doc;

            string check = "<li class=\"blocked\">";

            html = Program.Browser.DownloadPage(url, cookie_name);
            doc = modHTML.CleanHTML(ref html);
            html = doc.DocumentNode.InnerHtml;

            html = CheckBlocked(url, html, check);

            doc = null;

            return html;

        }

        public string CheckBlocked(string url, string html, string check)
        {

            MsgBoxResult ret;
            HtmlAgilityPack.HtmlDocument doc;
            string msg;
            List<KeyValuePair<string, string>> fields;

            URL link;
            string target = "";
            link = URLHelper.ExtractUrl(url);

            msg = link.Host;
            msg += Constants.vbCrLf;
            msg += Constants.vbCrLf;
            msg += "Warning: This site requires you to be logged in to view NC-17 stories.";
            msg += Constants.vbCrLf;
            msg += Constants.vbCrLf;
            msg += "Note: This information is requested in an effort to comply with ";
            msg += Constants.vbCrLf;
            msg += "the Child Online Protection Act (COPA) and related state law. ";
            msg += Constants.vbCrLf;
            msg += Constants.vbCrLf;
            msg += "Do you wish to proceed?";

            if (Strings.InStr(html, check) > 0)
            {

                ret = Interaction.MsgBox(msg, MsgBoxStyle.YesNo);

                if (ret == MsgBoxResult.Yes)
                {
                    AgeCheck = true;
                }
                else
                {
                    AgeCheck = false;
                    html = "";
                }

                if (AgeCheck)
                {

                    Username = Interaction.InputBox("Enter Username");
                    Password = Interaction.InputBox("Enter Password");

                    target = "http://www." + HostName + "/account/login";

                    fields = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("username", Username), new KeyValuePair<string, string>("password", Password), new KeyValuePair<string, string>("keeploggedin", "on"), new KeyValuePair<string, string>("submit", "Log in") };

                    Program.Browser.LogIn(target, "login", fields, cookie_name);

                    html = Program.Browser.DownloadPage(url, cookie_name);
                    doc = modHTML.CleanHTML(ref html);
                    html = doc.DocumentNode.InnerHtml;

                }
            }
            else
            {
                AgeCheck = true;
            }

            return html;

        }

        #endregion

        #region RSS

        protected override XmlDocument GrabFeedData(ref string rss)
        {

            string html;
            HtmlAgilityPack.HtmlDocument doc = null;
            HtmlAgilityPack.HtmlDocument tdoc = null;
            HtmlNodeCollection nodes;
            HtmlNodeCollection temp;

            DateTime dte;
            string[] ymd;

            string dummy;

            int temp_idx;
            int node_idx;

            string[] warning = null;

            string author_url;

            string[] summary;

            int ch_idx = 5;

            Story[] fic;

            int idx;

            XmlDocument xmldoc = null;

            author_url = rss;

            html = GrabData(rss);

            doc = modHTML.CleanHTML(ref html);

            nodes = modHTML.GetListNodes(doc.DocumentNode, "class", "storylist", true);

            try
            {

                fic = new Story[nodes.Count];

                var loopTo = nodes.Count - 1;
                for (node_idx = 0; node_idx <= loopTo; node_idx++)
                {

                    string arghtml = nodes[node_idx].InnerHtml;
                    doc = modHTML.CleanHTML(ref arghtml);
                    nodes[node_idx].InnerHtml = arghtml;

                    temp = doc.DocumentNode.SelectNodes("//a");

                    fic[node_idx].Title = temp[0].InnerText;
                    fic[node_idx].StoryURL = "http://www." + HostName + temp[0].Attributes["href"].Value;
                    fic[node_idx].Author = temp[1].InnerText;
                    fic[node_idx].AuthorURL = "http://www." + HostName + temp[1].Attributes["href"].Value;
                    fic[node_idx].Category = temp[3].InnerText;

                    temp = modHTML.FindLinksByHref(doc.DocumentNode, "/help#38");

                    if (!(temp == null))
                    {

                        warning = new string[temp.Count];

                        var loopTo1 = temp.Count - 1;
                        for (temp_idx = 0; temp_idx <= loopTo1; temp_idx++)
                            warning[temp_idx] = temp[temp_idx].Attributes["title"].Value;

                    }

                    temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "blockquote", "class", "summary", false);

                    dummy = temp[0].InnerText;

                    fic[node_idx].Summary = dummy;

                    temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "p", "class", "meta", false);

                    dummy = temp[0].InnerText;
                    dummy = modHTML.DecodeHTML(dummy);

                    summary = Strings.Split(dummy, " - ");

                    if (Strings.InStr(summary[5], "Chapter") > 0)
                    {
                        ch_idx = 5;
                        fic[node_idx].ChapterCount = modUtility.CleanString(summary[ch_idx].Split(':')[1].Trim());
                    }
                    else
                    {
                        ch_idx = 4;
                        fic[node_idx].ChapterCount = 1.ToString();
                    }

                    dummy = modUtility.CleanString(summary[ch_idx + 1].Split(':')[1].Trim());
                    ymd = Strings.Split(dummy, "-");
                    dte = new DateTime(Conversions.ToInteger(ymd[0]), Conversions.ToInteger(ymd[1]), Conversions.ToInteger(ymd[2]));

                    fic[node_idx].PublishDate = dte.ToShortDateString();

                    dummy = modUtility.CleanString(summary[ch_idx + 2].Split(':')[1].Trim());
                    ymd = Strings.Split(dummy, "-");
                    dte = new DateTime(Conversions.ToInteger(ymd[0]), Conversions.ToInteger(ymd[1]), Conversions.ToInteger(ymd[2]));

                    fic[node_idx].UpdateDate = dte.ToShortDateString();



                    dummy = fic[node_idx].Summary;
                    dummy += Constants.vbCrLf;
                    dummy += modUtility.CleanString(summary[1]);
                    dummy += Constants.vbCrLf;
                    dummy += modUtility.CleanString(summary[2]);
                    dummy += Constants.vbCrLf;
                    dummy += modUtility.CleanString(summary[3]);
                    dummy += Constants.vbCrLf;
                    dummy += "Warnings: ";

                    var loopTo2 = Information.UBound(warning);
                    for (idx = 0; idx <= loopTo2; idx++)
                    {
                        if (idx < Information.UBound(warning))
                        {
                            dummy += warning[idx] + ",";
                        }
                        else
                        {
                            dummy += warning[idx];
                        }
                    }

                    fic[node_idx].Summary = dummy;

                    fic[node_idx].ID = GetStoryID(fic[node_idx].StoryURL);

                }

                html = GenerateAtomFeed(fic);

                doc = modHTML.CleanHTML(ref html);

                html = doc.DocumentNode.OuterHtml;

                xmldoc = new XmlDocument();

                xmldoc.LoadXml(html);
            }

            catch
            {
                xmldoc = null;
            }
            finally
            {
                doc = null;
                tdoc = null;
                nodes = null;
                temp = null;
                fic = null;
            }

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
            fic.Author = Conversions.ToString(dsRSS.Tables["author"].Rows[idx]["name"]);

            fic.AuthorURL = Conversions.ToString(dsRSS.Tables["author"].Rows[idx]["uri"]);

            // Story Location
            fic.StoryURL = Conversions.ToString(dsRSS.Tables["link"].Rows[idx]["href"]);

            fic.PublishDate = Conversions.ToString(Conversions.ToDate(dsRSS.Tables["entry"].Rows[idx]["published"]));

            fic.UpdateDate = Conversions.ToString(Conversions.ToDate(dsRSS.Tables["entry"].Rows[idx]["updated"]));

            temp = Strings.Split(Conversions.ToString(dsRSS.Tables["entry"].Rows[idx]["id"]), ":");

            fic.Category = URLHelper.UrlDecode(temp[1]);
            fic.ID = temp[0];
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
            HtmlNodeCollection node;
            HtmlNodeCollection temp;
            string link;

            int idx;

            doc = modHTML.CleanHTML(ref htmlDoc);
            htmlDoc = doc.DocumentNode.InnerHtml;

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "ul", "class", "storylist");
            htmlDoc = temp[0].OuterHtml;

            doc = modHTML.CleanHTML(ref htmlDoc);
            htmlDoc = doc.DocumentNode.InnerHtml;

            node = doc.DocumentNode.SelectNodes("//li");

            if (!(node == null))
            {
                ret = new string[node.Count];
                var loopTo = node.Count - 1;
                for (idx = 0; idx <= loopTo; idx++)
                {

                    htmlDoc = node[idx].InnerHtml;
                    doc = modHTML.CleanHTML(ref htmlDoc);

                    temp = modHTML.FindLinksByHref(doc.DocumentNode, "/story/");

                    link = temp[0].Attributes["href"].Value;
                    link = Strings.Split(link, "/")[2];
                    link = GetStoryURL(link);

                    ret[idx] = link;
                }
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
                link = Chapters[index];
            }

            htmldoc = GrabData(link);

            return htmldoc;

        }

        public override string GetAuthorURL(string link)
        {

            string ret;

            if (Strings.InStr(link, "/author") > 0)
            {
                ret = link;
            }
            else
            {
                ret = "";
            }

            return ret;

        }

        public override string GetStoryID(string link)
        {

            string ret;
            URL hl;

            string[] uri;

            hl = URLHelper.ExtractUrl(link);

            uri = Strings.Split(hl.URI, "/");

            ret = uri[Information.UBound(uri)];

            hl = default;

            return ret;

        }

        public override string GetStoryURL(string id)
        {

            string ret = "";

            ret = "http://" + HostName + "/story/" + id;

            return ret;

        }

        #endregion

        #region HTML Processing

        public override string GrabAuthor(string htmlDoc)
        {

            string ret;
            HtmlAgilityPack.HtmlDocument doc;
            HtmlNodeCollection temp;

            doc = modHTML.CleanHTML(ref htmlDoc);
            htmlDoc = doc.DocumentNode.InnerHtml;

            htmlDoc = GrabStoryDiv(htmlDoc);
            doc = modHTML.CleanHTML(ref htmlDoc);

            temp = modHTML.FindLinksByHref(doc.DocumentNode, "/a");

            ret = temp[0].InnerText;

            temp = null;
            doc = null;

            return ret;

        }

        public override string GrabBody(string htmldoc)
        {

            string ret;
            HtmlAgilityPack.HtmlDocument doc;
            HtmlNodeCollection temp;

            htmldoc = GrabStoryDiv(htmldoc);
            doc = modHTML.CleanHTML(ref htmldoc);

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "id", "storytext", false);

            ret = "<div>" + temp[0].InnerHtml + "</div>";

            doc = null;
            temp = null;

            return ret;

        }

        public override string GrabDate(string htmlDoc, string title)
        {

            string ret = "";
            HtmlNodeCollection temp;
            HtmlAgilityPack.HtmlDocument doc;

            DateTime dte;
            string[] ymd;
            int ch_idx;

            string[] summary;

            doc = modHTML.CleanHTML(ref htmlDoc);
            htmlDoc = doc.DocumentNode.InnerHtml;

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "p", "class", "meta");
            ret = temp[0].InnerText;
            ret = modHTML.DecodeHTML(ret);

            summary = Strings.Split(ret, " - ");

            var loopTo = Information.UBound(summary);
            for (ch_idx = 0; ch_idx <= loopTo; ch_idx++)
            {

                if (Strings.InStr(summary[ch_idx], Strings.Trim(title)) > 0)
                {
                    break;
                }

            }

            ret = modUtility.CleanString(summary[ch_idx].Split(':')[1].Trim());
            ymd = Strings.Split(ret, "-");
            dte = new DateTime(Conversions.ToInteger(ymd[0]), Conversions.ToInteger(ymd[1]), Conversions.ToInteger(ymd[2]));

            ret = dte.ToShortDateString();

            temp = null;
            doc = null;

            return ret;

        }

        public override string GrabSeries(string htmlDoc)
        {

            string ret;

            HtmlAgilityPack.HtmlDocument doc;
            HtmlNodeCollection node;
            HtmlNodeCollection temp;

            doc = modHTML.CleanHTML(ref htmlDoc);
            htmlDoc = doc.DocumentNode.InnerHtml;

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "class", "storylist");
            htmlDoc = temp[0].OuterHtml;

            doc = modHTML.CleanHTML(ref htmlDoc);
            htmlDoc = doc.DocumentNode.InnerHtml;

            node = doc.DocumentNode.SelectNodes("//a");

            ret = node[3].InnerText;

            return ret;

        }

        private string GrabStoryDiv(string htmldoc)
        {

            string ret = "";
            HtmlNodeCollection temp;
            HtmlAgilityPack.HtmlDocument doc;

            doc = modHTML.CleanHTML(ref htmldoc);

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "id", "story");

            htmldoc = "<div>";
            htmldoc += temp[0].InnerHtml;
            htmldoc += "</div>";

            ret = htmldoc;

            temp = null;
            doc = null;

            return ret;

        }

        public override string GrabTitle(string htmlDoc)
        {

            string ret;

            HtmlAgilityPack.HtmlDocument doc;
            HtmlNodeCollection node;
            HtmlNodeCollection temp;

            doc = modHTML.CleanHTML(ref htmlDoc);
            htmlDoc = doc.DocumentNode.InnerHtml;

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "class", "storylist");
            htmlDoc = temp[0].OuterHtml;

            doc = modHTML.CleanHTML(ref htmlDoc);
            htmlDoc = doc.DocumentNode.InnerHtml;

            node = doc.DocumentNode.SelectNodes("//a");

            ret = node[0].InnerText;

            return ret;

        }

        #endregion

        #region Properties

        public override string HostName
        {
            get
            {
                return "ficwad.com";
                // Return ""
            }
        }

        public override string Name
        {
            get
            {
                return "FicWad";
            }
        }

        public override string ErrorMessage
        {
            get
            {
                return "Unknown author";
            }
        }

        #endregion

    }
}