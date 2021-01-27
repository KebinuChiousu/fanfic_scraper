using System;
using Microsoft.VisualBasic;
using System.Xml;
using System.Data;

using Web;
using Web.Utility.Helper;

namespace web_scraper.Models.Base
{

    public abstract class FanFic
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
                datasetRSS.ReadXml(Util.StringToStream(xmldoc.OuterXml));
            else
                datasetRSS = null;

            return datasetRSS;
        }

        public abstract Story GrabStoryInfo(int idx);        

        public Story GetStoryInfoById(string StoryId)
        {
            Story fic = default;

            int idx;

            for (idx = 0; idx <= datasetRSS.Tables[0].Rows.Count - 1; idx++)
            {
                fic = GrabStoryInfo(idx);

                if (Strings.InStr(fic.ID, StoryId) != 0)
                    break;
            }

            return fic;
        }

        protected string[] Chapters;

        protected string GenerateAtomFeed(Story[] fic)
        {
            int nodeIdx;
            string html = "<feed>";

            for (nodeIdx = 0; nodeIdx <= Information.UBound(fic); nodeIdx++)
            {
                html += "<entry>";
                html += "<author>";
                html += "<name>";
                html += fic[nodeIdx].Author;
                html += "</name>";
                html += "<uri>";
                html += fic[nodeIdx].AuthorURL;
                html += "</uri>";
                html += "</author>";
                html += "<published>";
                html += fic[nodeIdx].PublishDate.ToString();
                html += "</published>";
                html += "<updated>";
                html += fic[nodeIdx].UpdateDate.ToString();
                html += "</updated>";
                html += "<title>";
                html += fic[nodeIdx].Title;
                html += "</title>";
                html += "<link rel=\"alternate\" href=\"" + fic[nodeIdx].StoryURL + "\" />";
                html += "<id>";
                html += fic[nodeIdx].ID;
                html += ":";
                html += fic[nodeIdx].Category;
                html += ":";
                html += fic[nodeIdx].ChapterCount;
                html += "</id>";
                html += "<summary type=\"html\">";
                html += System.Web.HttpUtility.HtmlEncode(fic[nodeIdx].Summary);
                html += "</summary>";
                html += "</entry>";
            }

            html += "</feed>";

            return html;
        }

        public abstract string ErrorMessage { get; }

        public virtual string HostName => "";

        public abstract string Name { get; }

        public DataSet RSS => datasetRSS;
    }
}