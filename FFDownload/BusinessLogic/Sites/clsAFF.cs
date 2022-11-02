using System;
using System.Collections.Generic;
using System.Xml;
using HtmlAgilityPack;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace HtmlGrabber
{


    class AFF : clsFanfic
    {

        #region Downloading HTML

        private string cookie_name = "adultfanfiction_org.cookie";
        private bool AgeCheck = true;
        private string FullName;
        private string DOB;

        public override string GrabData(string url)
        {

            string html;
            string title = "";
            bool check1;
            bool check2;
            string target = "";
            Uri u;
            List<KeyValuePair<string, string>> fields;

            html = modMain.Browser.DownloadPage(url, cookie_name);

            title = modHTML.GetTitle(html);

            check1 = CheckPage(html);
            check2 = CheckAge(title);

            if (!AgeCheck)
            {
                html = "<AgeCheck value='False' />";
            }
            else
            {
                if (!check1)
                {

                    target = "http://www.adult-fanfiction.org/";

                    modMain.Browser.FollowLink(target, "I am 18 years of age or older.", cookie_name);

                    html = modMain.Browser.DownloadPage(url, cookie_name);

                }

                if (!check2)
                {

                    u = new Uri(url);

                    target = u.Scheme + Uri.SchemeDelimiter + u.Host + "/form_adult.php";

                    fields = null;
                    fields = new List<KeyValuePair<string, string>>();

                    fields.Add(new KeyValuePair<string, string>("cmbmonth", Conversions.ToDate(DOB).Month.ToString()));
                    fields.Add(new KeyValuePair<string, string>("cmbday", Conversions.ToDate(DOB).Day.ToString()));
                    fields.Add(new KeyValuePair<string, string>("cmbyear", Conversions.ToDate(DOB).Year.ToString()));
                    fields.Add(new KeyValuePair<string, string>("cmbname", FullName));
                    fields.Add(new KeyValuePair<string, string>("submit", "Click here to submit"));

                    modMain.Browser.LogIn(target, "form", fields, cookie_name);

                    html = modMain.Browser.DownloadPage(url, cookie_name);

                }

            }

            return html;

        }

        public bool CheckAge(string title)
        {

            bool ok = true;

            MsgBoxResult ret;
            string msg;

            string dob = "";
            TimeSpan tsDOB;

            string name = "";

            msg = "Warning: This site requires age verification using e-signing";
            msg += Constants.vbCrLf;
            msg += Constants.vbCrLf;
            msg += "I hereby affirm, under the penalties of perjury pursuant to ";
            msg += Constants.vbCrLf;
            msg += "Title 28 U.S.C. secion 1746, that I was born on [Date of Birth]";
            msg += Constants.vbCrLf;
            msg += Constants.vbCrLf;
            msg += "Signed [YOUR NAME]";
            msg += Constants.vbCrLf;
            msg += Constants.vbCrLf;
            msg += "Note: This information is provided in an effort to comply with ";
            msg += Constants.vbCrLf;
            msg += "the Child Online Protection Act (COPA) and related state law. ";
            msg += Constants.vbCrLf;
            msg += "Providing a false declaration under the penalties of perjury is ";
            msg += "a criminal offense. This document constitutes an un-sworn ";
            msg += "declaration under federal law. ";
            msg += Constants.vbCrLf;
            msg += Constants.vbCrLf;
            msg += "The information provided here will only be provided to a third party ";
            msg += Constants.vbCrLf;
            msg += "when legally required (i.e. when/if a subpoena is served to us). ";
            msg += Constants.vbCrLf;
            msg += "Your information will not be given out except in the case mentioned above.";
            msg += Constants.vbCrLf;
            msg += Constants.vbCrLf;
            msg += "Do you wish to proceed?";

            if (Strings.InStr(title, "Birthdate Verification Page") > 0)
            {

                ret = Interaction.MsgBox(msg, MsgBoxStyle.YesNo);

                if (ret == MsgBoxResult.Yes)
                {
                    AgeCheck = true;
                    ok = false;
                }
                else
                {
                    AgeCheck = false;
                    ok = false;
                }

                if (AgeCheck)
                {

                    dob = Interaction.InputBox("Enter the day you were born (mm/dd/yyyy)");
                    if (Information.IsDate(dob))
                    {
                        tsDOB = DateTime.Today.Subtract(Conversions.ToDate(dob));
                        if (tsDOB.Days > 365 * 18)
                        {
                            name = Interaction.InputBox("Enter First and Last Name");
                        }
                        else
                        {
                            AgeCheck = false;
                        }
                    }

                    FullName = name;
                    DOB = dob;


                }
            }

            else
            {
                AgeCheck = true;
                ok = true;
            }

            return ok;

        }


        public bool CheckPage(string html)
        {

            bool ok = true;

            MsgBoxResult ret;
            string msg = "In order to proceed, you must be at least 18 years of age (21 years of age in some jurisdictions)";

            if (Strings.InStr(html, msg) > 0)
            {

                msg += "Welcome to adult-fanfiction.org.  ";
                msg += "In order to proceed, you must be at least 18 years of age (21 years of age in some jurisdictions), ";
                msg += "and legally permitted to view Adult Content in your area.  ";
                msg += "This is an archive of literature written by and for Adults, only. ";
                msg += Constants.vbCrLf;
                msg += Constants.vbCrLf;
                msg += "Do you wish to proceed?";

                ret = Interaction.MsgBox(msg, MsgBoxStyle.YesNo);

                if (ret == MsgBoxResult.Yes)
                {
                    AgeCheck = true;
                }
            }

            else
            {
                AgeCheck = true;
            }

            return AgeCheck;

        }

        #endregion

        #region RSS

        protected override XmlDocument GrabFeedData(ref string rss)
        {

            string html;
            HtmlAgilityPack.HtmlDocument doc = null;

            HtmlNodeCollection nodes;
            HtmlNodeCollection temp;

            string link = "";
            URL url;

            int node_idx;

            string author;
            string author_url;

            string[] summary;
            Story[] fic;

            XmlDocument xmldoc = null;

            author_url = rss;

            if (Strings.InStr(rss, "zone=") == 0)
            {
                if (Strings.InStr(rss, "view=story") == 0)
                {

                    html = GrabData(rss);
                    doc = modHTML.CleanHTML(ref html);

                    nodes = modHTML.FindLinksByHref(doc.DocumentNode, "view=story");
                    link = nodes[0].Attributes["href"].Value;
                    link = modHTML.DecodeHTML(link);
                }

                else
                {
                    link = rss;
                }

                html = GrabData(link);
                doc = modHTML.CleanHTML(ref html);

                nodes = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "id", "contentdata", false);

                string arghtml = nodes[0].OuterHtml;
                doc = modHTML.CleanHTML(ref arghtml);

                nodes = modHTML.FindLinksByHref(doc.DocumentNode, "zone=");
                link = nodes[0].Attributes["href"].Value;
                link = modHTML.DecodeHTML(link);
            }

            else
            {
                link = rss;
            }

            html = GrabData(link);
            doc = modHTML.CleanHTML(ref html);

            nodes = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "id", "contentdata");

            html = nodes[0].OuterHtml;
            doc = modHTML.CleanHTML(ref html);

            author = doc.DocumentNode.SelectSingleNode("//h2").InnerText;

            nodes = modHTML.FindNodesByAttribute(doc.DocumentNode, "div", "class", "alist", false);
            html = nodes[0].OuterHtml;
            doc = modHTML.CleanHTML(ref html);

            nodes = doc.DocumentNode.SelectNodes("//li");

            fic = new Story[nodes.Count];

            var loopTo = nodes.Count - 1;
            for (node_idx = 0; node_idx <= loopTo; node_idx++)
            {

                html = nodes[node_idx].InnerHtml;
                doc = modHTML.CleanHTML(ref html);

                temp = modHTML.FindLinksByHref(doc.DocumentNode, "story.php");

                fic[node_idx].Author = author;
                fic[node_idx].AuthorURL = author_url;
                fic[node_idx].StoryURL = temp[0].Attributes["href"].Value;
                fic[node_idx].Title = temp[0].InnerText;

                html = Strings.Replace(html, temp[0].OuterHtml, "");
                temp = modHTML.FindLinksByHref(doc.DocumentNode, "review.php");
                html = Strings.Replace(html, temp[0].OuterHtml, "");

                doc = modHTML.CleanHTML(ref html);
                html = doc.DocumentNode.SelectSingleNode("//span").InnerHtml;

                summary = Strings.Split(html, "<br />");

                fic[node_idx].Summary = modHTML.DecodeHTML(summary[1]);

                summary = Strings.Split(summary[2], "-:-");

                fic[node_idx].PublishDate = Strings.Trim(Strings.Replace(summary[0], "Posted : ", ""));
                fic[node_idx].UpdateDate = Strings.Trim(Strings.Replace(summary[1], "Edited : ", ""));

                url = URLHelper.ExtractUrl(fic[node_idx].StoryURL);

                fic[node_idx].Category = Strings.Split(url.Host, ".")[0];

                fic[node_idx].ID = GetStoryID(fic[node_idx].StoryURL);

                html = GrabData(fic[node_idx].StoryURL);
                doc = modHTML.CleanHTML(ref html);

                temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "select", "name", "chapnav");
                html = temp[0].InnerHtml;
                doc = modHTML.CleanHTML(ref html);

                fic[node_idx].ChapterCount = doc.DocumentNode.SelectNodes("//option").Count.ToString();

                if (!AgeCheck)
                {
                    break;
                }

            }

            if (!AgeCheck)
            {
                xmldoc = null;
            }
            else
            {

                html = GenerateAtomFeed(fic);

                doc = modHTML.CleanHTML(ref html);

                html = doc.DocumentNode.OuterHtml;

                xmldoc = new XmlDocument();

                xmldoc.LoadXml(html);

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
            fic.Author = Conversions.ToString(dsRSS.Tables["author"].Rows[idx][0]);
            // Story Location
            fic.StoryURL = Conversions.ToString(dsRSS.Tables["link"].Rows[idx][1]);

            fic.PublishDate = Conversions.ToString(Conversions.ToDate(dsRSS.Tables["entry"].Rows[idx]["published"]));

            fic.UpdateDate = Conversions.ToString(Conversions.ToDate(dsRSS.Tables["entry"].Rows[idx]["updated"]));

            temp = Strings.Split(Conversions.ToString(dsRSS.Tables["entry"].Rows[idx]["id"]), ":");

            fic.ID = temp[0];
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

            doc = modHTML.CleanHTML(ref htmlDoc);

            temp = modHTML.FindNodesByAttribute(doc.DocumentNode, "select", "name", "chapnav");
            htmlDoc = temp[0].InnerHtml;
            doc = modHTML.CleanHTML(ref htmlDoc);

            temp = doc.DocumentNode.SelectNodes("//option");

            if (!(temp == null))
            {
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

            URL hl;
            string host;

            hl = URLHelper.ExtractUrl(link);
            host = hl.Host;


            link = hl.Scheme + "://" + hl.Host + hl.URI;
            link += "?";
            link += hl.Query[0].Name + "=" + hl.Query[0].Value;
            link += "&";
            link += "chapter=" + (index + 1);

            string htmldoc;

            htmldoc = GrabData(link);

            StoryURL = link;

            return htmldoc;

        }

        public override string GetAuthorURL(string link)
        {

            string htmldoc;
            HtmlAgilityPack.HtmlDocument doc;
            HtmlNodeCollection temp;

            string ret = "";

            if (Strings.InStr(link, "profile.php") > 0)
            {
                ret = link;
            }
            else
            {
                htmldoc = GrabData(link);
                htmldoc = GrabHeaderRow(htmldoc);
                doc = modHTML.CleanHTML(ref htmldoc);
                temp = modHTML.FindLinksByHref(doc.DocumentNode, "profile.php");
                ret = temp[0].Attributes["href"].Value;
            }

            return ret;

        }

        public override string GetStoryID(string link)
        {

            string ret = "";
            URL url;

            url = URLHelper.ExtractUrl(link);

            ret = Strings.Split(url.Host, ".")[0];
            ret += "|";
            ret += url.Query[0].Value;

            return ret;

        }

        public override string GetStoryURL(string id)
        {

            string[] temp;
            string link;
            string category;

            temp = Strings.Split(id, "|");
            category = temp[0];
            id = temp[1];

            link = "http://" + category + "." + HostName + "/story.php?no=" + id;

            return link;

        }

        #endregion

        #region HTML Processing

        private string GrabPageTable(string htmldoc)
        {

            string ret = "";
            HtmlNodeCollection temp;
            HtmlAgilityPack.HtmlDocument doc;

            doc = modHTML.CleanHTML(ref htmldoc);

            temp = doc.DocumentNode.SelectNodes("//table");

            htmldoc = "<table>";
            htmldoc += temp[6].InnerHtml;
            htmldoc += "</table>";

            ret = htmldoc;

            temp = null;
            doc = null;

            return ret;

        }

        private string GrabHeaderRow(string htmldoc)
        {

            string ret;
            HtmlNodeCollection temp;
            HtmlAgilityPack.HtmlDocument doc;

            htmldoc = GrabPageTable(htmldoc);

            doc = modHTML.CleanHTML(ref htmldoc);
            temp = doc.DocumentNode.SelectNodes("//tr");

            htmldoc = "<table>";
            htmldoc += "<tr>";
            htmldoc += temp[0].InnerHtml;
            htmldoc += "</tr>";
            htmldoc += "</table>";

            ret = htmldoc;

            temp = null;
            doc = null;

            return ret;

        }

        public override string GrabAuthor(string htmlDoc)
        {

            string ret;
            HtmlNodeCollection temp;
            HtmlAgilityPack.HtmlDocument doc;

            htmlDoc = GrabHeaderRow(htmlDoc);

            doc = modHTML.CleanHTML(ref htmlDoc);

            temp = modHTML.FindLinksByHref(doc.DocumentNode, "profile.php");

            ret = modUtility.CleanString(temp[0].InnerText);

            temp = null;
            doc = null;

            return ret;

        }

        public override string GrabBody(string htmldoc)
        {

            string ret = "";

            HtmlAgilityPack.HtmlDocument doc;
            HtmlNodeCollection temp;

            htmldoc = GrabPageTable(htmldoc);
            doc = modHTML.CleanHTML(ref htmldoc);

            temp = doc.DocumentNode.SelectNodes("//tr");

            htmldoc = temp[3].ChildNodes[1].InnerHtml;

            doc = null;
            temp = null;

            ret = htmldoc;

            return ret;

        }

        public override string GrabDate(string htmlDoc, string title)
        {

            string ret = "";
            HtmlNodeCollection temp;
            HtmlAgilityPack.HtmlDocument doc;

            string category;
            string[] summary;
            string link;

            URL u;

            u = URLHelper.ExtractUrl(StoryURL);

            htmlDoc = GrabHeaderRow(htmlDoc);

            doc = modHTML.CleanHTML(ref htmlDoc);

            category = Strings.Replace(u.Host, HostName, "").Replace(".", "");

            temp = modHTML.FindLinksByHref(doc.DocumentNode, "profile.php");

            link = temp[0].Attributes["href"].Value;
            link = modHTML.DecodeHTML(link);

            link += "&view=story&zone=" + category;

            htmlDoc = GrabData(link);
            doc = modHTML.CleanHTML(ref htmlDoc);

            link = u.Scheme + Uri.SchemeDelimiter + u.Host + u.URI + "?" + u.Query[0].Name + "=" + u.Query[0].Value;

            temp = modHTML.FindLinksByHref(doc.DocumentNode, link);

            htmlDoc = temp[0].ParentNode.InnerHtml;

            summary = Strings.Split(htmlDoc, "<br />");
            summary = Strings.Split(summary[2], "-:-");

            switch (title ?? "")
            {
                case "Published: ":
                    {
                        ret = Conversions.ToDate(Strings.Trim(Strings.Replace(summary[0], "Posted :", ""))).ToShortDateString();
                        break;
                    }
                case "Updated: ":
                    {
                        ret = Conversions.ToDate(Strings.Trim(Strings.Replace(summary[1], "Edited :", ""))).ToShortDateString();
                        break;
                    }
            }

            temp = null;
            doc = null;

            return ret;

        }

        public override string GrabSeries(string htmlDoc)
        {

            string ret;
            HtmlNodeCollection temp;
            HtmlAgilityPack.HtmlDocument doc;

            string category;

            htmlDoc = GrabHeaderRow(htmlDoc);
            doc = modHTML.CleanHTML(ref htmlDoc);

            temp = modHTML.FindLinksByHref(doc.DocumentNode, "main.php");

            category = temp[0].InnerText;

            ret = category;

            temp = null;
            doc = null;

            return ret;

        }

        public override string GrabTitle(string htmlDoc)
        {

            string ret;
            HtmlAgilityPack.HtmlDocument doc;

            doc = modHTML.CleanHTML(ref htmlDoc);

            ret = doc.DocumentNode.SelectSingleNode("//title").InnerText;

            ret = Strings.Trim(Strings.Replace(ret, "Story:", ""));

            doc = null;

            return ret;

        }

        #endregion



        #region Properties

        public override string HostName
        {
            get
            {
                return "adult-fanfiction.org";
            }
        }

        public override string Name
        {
            get
            {
                return "AFF";
            }
        }

        public override string ErrorMessage
        {
            get
            {
                return "The member you are looking for does not exsist.";
            }
        }

        #endregion

    }
}