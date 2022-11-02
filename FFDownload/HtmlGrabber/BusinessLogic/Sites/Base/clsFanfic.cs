using System.Data;
using static System.Web.HttpUtility;
using System.Xml;
using Microsoft.VisualBasic;

namespace HtmlGrabber
{

    public abstract class clsFanfic
    {

        protected DataSet datasetRSS;
        protected string StoryURL;

        public abstract string ProcessChapters(string link, int index);





        public virtual string WriteDate(string publish, string update, int index, int lstop)





        {
            string WriteDateRet = default;

            WriteDateRet = "";

            if (index == 1)
            {
                WriteDateRet = "<p>" + publish + "</p>";
            }
            else if (index == lstop)
            {
                WriteDateRet = "<p>" + update + "</p>";
            }

            return WriteDateRet;

        }

        #region Data Extraction

        public abstract string GrabTitle(string htmlDoc);



        public abstract string GrabSeries(string htmlDoc);

        public abstract string GrabDate(string htmlDoc, string title);




        public abstract string GrabAuthor(string htmlDoc);

        public abstract string GrabBody(string htmldoc);

        public abstract string GetAuthorURL(string link);

        public abstract string GetStoryID(string link);

        public abstract string GetStoryURL(string id);


        #region Chapter Extraction

        public virtual string[] GetChapters(string htmlDoc)


        {

            string[] data;

            data = modHTML.GetOptionValues(htmlDoc);

            if (data == null)
            {
                data = new string[1];
                data[0] = "Chapter 1";
            }

            Chapters = data;

            return data;

        }



        #endregion

        #endregion

        #region Download Routines

        public virtual string InitialDownload(string url)
        {

            string htmldoc;

            htmldoc = GrabData(url);

            return htmldoc;

        }

        public virtual string GrabData(string url)
        {

            string html;

            StoryURL = url;

            html = Program.Browser.DownloadPage(url);

            modHTML.CleanHTML(ref html);

            return html;

        }

        protected abstract XmlDocument GrabFeedData(ref string rss);

        public DataSet GrabFeed(ref string rss)
        {

            XmlDocument xmldoc;

            datasetRSS = null;
            datasetRSS = new DataSet();

            xmldoc = GrabFeedData(ref rss);

            if (!(xmldoc == null))
            {
                // Read in XML from file
                datasetRSS.ReadXml(modUtility.StringToStream(xmldoc.OuterXml));
            }
            else
            {
                datasetRSS = null;
            }

            return datasetRSS;

        }
        #endregion

        #region RSS Routines

        public abstract Story GrabStoryInfo(int idx);

        public struct Story
        {
            public string ID;
            public string Title;
            public string Author;
            public string AuthorURL;
            public string StoryURL;
            public string Category;
            public string[] Chapters;
            public string ChapterCount;
            public string PublishDate;
            public string UpdateDate;
            public string Summary;
        }

        public Story GetStoryInfoByID(string StoryID)
        {

            Story fic = default;

            int idx;

            var loopTo = datasetRSS.Tables[0].Rows.Count - 1;
            for (idx = 0; idx <= loopTo; idx++)
            {

                fic = GrabStoryInfo(idx);

                if (Strings.InStr(fic.ID, StoryID) != 0)
                {
                    break;
                }

            }

            return fic;

        }

        protected string[] Chapters;

        protected string GenerateAtomFeed(Story[] fic)
        {

            string html = "";
            int node_idx;

            html = "<feed>";

            var loopTo = Information.UBound(fic);
            for (node_idx = 0; node_idx <= loopTo; node_idx++)
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
                html += fic[node_idx].PublishDate;
                html += "</published>";
                html += "<updated>";
                html += fic[node_idx].UpdateDate;
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
                html += HtmlEncode(fic[node_idx].Summary);
                html += "</summary>";
                html += "</entry>";

            }

            html += "</feed>";

            return html;

        }

        #endregion

        #region Properties

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

        #endregion

    }
}