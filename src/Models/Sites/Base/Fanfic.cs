using System;
using Microsoft.VisualBasic;
using HtmlAgilityPack;
using System.Xml;
using System.Data;

using web_scraper.Models;
using web_scraper.Models.Utility;

namespace web_scraper.Models.Sites.Base
{

    public abstract class Fanfic
    {
        protected DataSet datasetRSS;
        protected string StoryURL;

        public abstract string ProcessChapters(string link, int index);

        public virtual string WriteDate(DateTime? publish, DateTime? update, int index, int lstop)
        {
            string dte = "";

            if ((publish != null) && (index == 1))
            {
                dte = Convert.ToDateTime(publish).ToLongDateString();
            }

            if ((update != null) && (index == lstop))
            {
                dte = Convert.ToDateTime(update).ToLongDateString();
            }

            string ret = "<p>" + dte + "</p>";
            return ret;
        }

        public abstract string GrabTitle(string htmlDoc);

        public abstract string GrabSeries(string htmlDoc);

        public abstract DateTime? GrabDate(string htmlDoc, string title);

        public abstract string GrabAuthor(string htmlDoc);

        public abstract string GrabBody(string htmldoc);

        public abstract string GetAuthorURL(string link);

        public abstract string GetStoryID(string link);

        public abstract string GetStoryURL(string id);

        public virtual string[] GetChapters(string htmlDoc)
        {
            string[] data;

            data = HtmlHelper.GetOptionValues(htmlDoc);

            if (Information.IsNothing(data))
            {
                data = new string[1];
                data[0] = "Chapter 1";
            }

            this.Chapters = data;

            return data;
        }

        public virtual string InitialDownload(string url)
        {
            string htmldoc;

            htmldoc = GrabData(url);

            return htmldoc;
        }

        public virtual string GrabData(string url)
        {
            string html;

            this.StoryURL = url;

            html = Browser.DownloadPage(url);

            HtmlHelper.CleanHTML(ref html);

            return html;
        }

        protected abstract System.Xml.XmlDocument GrabFeedData(ref string rss);

        public System.Data.DataSet GrabFeed(ref string rss)
        {
            XmlDocument xmldoc;

            datasetRSS = null;
            datasetRSS = new DataSet();

            xmldoc = GrabFeedData(ref rss);

            if (!Information.IsNothing(xmldoc))
                // Read in XML from file
                datasetRSS.ReadXml(Functions.StringToStream(xmldoc.OuterXml));
            else
                datasetRSS = null;

            return datasetRSS;
        }

        public abstract Story GrabStoryInfo(int idx);        

        public Story GetStoryInfoByID(string StoryID)
        {
            Story fic = default(Story);

            int idx;

            for (idx = 0; idx <= datasetRSS.Tables[0].Rows.Count - 1; idx++)
            {
                fic = GrabStoryInfo(idx);

                if (Strings.InStr(fic.ID, StoryID) != 0)
                    break;
            }

            return fic;
        }

        protected string[] Chapters;

        protected string GenerateAtomFeed(Story[] fic)
        {
            string html = "";
            int node_idx;

            html = "<feed>";

            for (node_idx = 0; node_idx <= Information.UBound(fic); node_idx++)
            {
                html += "<entry>";
                html += "<author>";
                html += "<name>";
                html += fic[node_idx].Author;
                html += "</name>";
                html += "<uri>";
                html += fic[node_idx].AuthorURL;
                html += "</uri>";
                html += "</author>";
                html += "<published>";
                html += fic[node_idx].PublishDate.ToString();
                html += "</published>";
                html += "<updated>";
                html += fic[node_idx].UpdateDate.ToString();
                html += "</updated>";
                html += "<title>";
                html += fic[node_idx].Title;
                html += "</title>";
                html += "<link rel=\"alternate\" href=\"" + fic[node_idx].StoryURL + "\" />";
                html += "<id>";
                html += fic[node_idx].ID;
                html += ":";
                html += fic[node_idx].Category;
                html += ":";
                html += fic[node_idx].ChapterCount;
                html += "</id>";
                html += "<summary type=\"html\">";
                html += System.Web.HttpUtility.HtmlEncode(fic[node_idx].Summary);
                html += "</summary>";
                html += "</entry>";
            }

            html += "</feed>";

            return html;
        }

        public abstract string ErrorMessage { get; }

        public virtual string HostName
        {
            get
            {
                return "";
            }
        }

        public abstract string Name { get; }

        public DataSet dsRSS
        {
            get
            {
                return datasetRSS;
            }
        }
    }
}