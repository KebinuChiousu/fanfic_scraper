using System.Data;
using System.Text;
using System.Web;
using HtmlScraper.BusinessLogic.Sites;
using HtmlScraper.BusinessLogic.Sites.Base;
using HtmlScraper.Utility;
using HtmlScraper.Utility.Browser.HTML;
using Microsoft.VisualBasic;

namespace HtmlScraper.BusinessLogic
{

    public class clsBL
    {


        private string txtResult;
        private clsFanfic.Story Story = new clsFanfic.Story();

        public string OutputPath;

        public bool GetChapters(string link)
        {

            string htmldoc;

            // Download Information from Source

            if (string.IsNullOrEmpty(link))
            {
                return false;
            }

            htmldoc = Program.cls.GrabData(link);
            txtResult = htmldoc;

            if (Strings.InStr(Strings.LCase(txtResult), Program.cls.ErrorMessage) == 0)
            {

                if (string.IsNullOrEmpty(txtResult))
                {
                    return false;
                }


                Story.Chapters = Program.cls.GetChapters(htmldoc);



                htmldoc = Program.cls.ProcessChapters(link, 0);



                Story.StoryURL = link;

                Story.ID = Program.cls.GetStoryID(link);
                Story.Title = Program.cls.GrabTitle(htmldoc);
                Story.Author = Program.cls.GrabAuthor(htmldoc);
                Story.Category = Program.cls.GrabSeries(htmldoc);

                Story.PublishDate = Program.cls.GrabDate(htmldoc, "Published: ");
                Story.UpdateDate = Program.cls.GrabDate(htmldoc, "Updated: ");

                if (string.IsNullOrEmpty(Story.UpdateDate))
                    Story.UpdateDate = Story.PublishDate;

                Story.ChapterCount = (Information.UBound(Story.Chapters) + 1).ToString();

                txtResult = Program.cls.GrabBody(htmldoc);

                return true;
            }

            else
            {
                return false;
            }

        }

        public void ProcessChapters(string link, int Start, int Count, string FileMask, string Category = "")





        {

            string htmldoc;
            int idx;
            string msg;

            // Process Chapters from Source

            var loopTo = Count;
            for (idx = Start; idx <= loopTo; idx++)
            {

                msg = "Chapter " + idx + " of " + Count;



                FormManagement.frmMain.UpdateProgess(msg);

                htmldoc = Program.cls.ProcessChapters(link, idx - 1);



                ProcessChapter(ref htmldoc, FileMask, idx, Count, link, Category);






            }

        }

        public void ProcessChapter(ref string htmlDoc, string FileMask, int chapter, int ChapterCount = 0, string link = "", string Category = "")





        {

            // Dim ecp1252 As Encoding = Encoding.GetEncoding(1252)
            StreamReader sr;
            StreamWriter sw;

            string Body;
            string data = "";

            string Path;

            if (ChapterCount == 0)
            {
                ChapterCount = chapter;
            }


            txtResult = htmlDoc;

            if (Strings.InStr(Strings.LCase(txtResult), "chapter not found") == 0 & Strings.InStr(Strings.LCase(txtResult), "story not found") == 0)

            {

                Story.Title = Program.cls.GrabTitle(htmlDoc);
                Story.Author = Program.cls.GrabAuthor(htmlDoc);
                Story.Category = Program.cls.GrabSeries(htmlDoc);
                Story.PublishDate = Program.cls.GrabDate(htmlDoc, "Published: ");
                Story.UpdateDate = Program.cls.GrabDate(htmlDoc, "Updated: ");

                if (string.IsNullOrEmpty(Story.UpdateDate))
                    Story.UpdateDate = Story.PublishDate;

                data = Program.cls.GrabBody(htmlDoc);

                Body = "<p></p>" + data;

                txtResult = "<html><body>";
                txtResult += "<p>" + Story.Title + "</p>";
                txtResult += "<p>" + Story.Author + "</p>";

                if (!string.IsNullOrEmpty(Story.Category))
                {
                    txtResult += "<p>" + Story.Category + "</p>";
                }

                txtResult += Program.cls.WriteDate(Story.PublishDate, Story.UpdateDate, chapter, ChapterCount);




                txtResult += "<p>----------------------------------</p>";
                txtResult += Body;
            }
            else
            {
                txtResult = "<p>Error writing file</p>";
                txtResult += "<p>Try downloading the file from " + "<a href=" + '"' + link + chapter + "/" + '"' + ">here</a> in a regular browser</p>";

                txtResult += "<p>DOWNLOAD ERROR</p>";
            }

            txtResult = HttpUtility.HtmlDecode(txtResult);

            sr = new StreamReader(modUtility.StringToStream(txtResult));

            FileMask = Strings.Replace(FileMask, "-", "");

            if (Information.IsNumeric(Strings.Mid(FileMask, Strings.Len(FileMask), 1)))
            {
                FileMask += "-";
            }

            Path = OutputPath;

            if (!string.IsNullOrEmpty(Category))
            {
                Path += @"\";
                Path += Strings.Replace(Category, " ", "_");
            }

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            sw = new StreamWriter(Path + @"\" + FileMask + Strings.Format(chapter, "0#") + ".htm", false);





            sw.Write(sr.ReadToEnd());
            sr.Close();
            sw.Close();
            sr = null;
            sw = null;

        }

        public void ProcessError(string link, int chapter, string filemask, string Category = "")
        {

            var ecp1252 = Encoding.GetEncoding(1252);
            StreamReader sr;
            StreamWriter sw;

            string Path;

            string txtResult;

            txtResult = "<p>Error writing file</p>";
            txtResult += "<p>Try downloading the file from " + "<a href=" + '"' + link + chapter + '"' + ">here</a> in a regular browser</p>";

            txtResult += "<p>DOWNLOAD ERROR</p>";

            txtResult = HttpUtility.HtmlDecode(txtResult);

            sr = new StreamReader(modUtility.StringToStream(txtResult));

            filemask = Strings.Replace(filemask, "-", "");

            if (Information.IsNumeric(Strings.Mid(filemask, Strings.Len(filemask), 1)))
            {
                filemask += "-";
            }

            Path = OutputPath;

            if (!string.IsNullOrEmpty(Category))
            {
                Path += @"\";
                Path += Strings.Replace(Category, " ", "_");
            }

            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }

            sw = new StreamWriter(Path + @"\" + filemask + Strings.Format(chapter, "0#") + ".htm", false, ecp1252);






            sw.Write(sr.ReadToEnd());
            sr.Close();
            sw.Close();
            sr = null;
            sw = null;

        }

        #region Pass-Thru Functions

        public clsFanfic.Story GrabStoryInfo(int idx)
        {

            clsFanfic.Story fic;

            fic = Program.cls.GrabStoryInfo(idx);

            return fic;

        }

        public DataSet GrabFeed(string link)
        {

            DataSet ds;

            ds = Program.cls.GrabFeed(ref link);

            return ds;

        }

        public string GetStoryID(string link)
        {

            string id;

            id = Program.cls.GetStoryID(link);

            return id;

        }

        public clsFanfic.Story GetStoryInfoByID(string StoryID)
        {

            clsFanfic.Story fic;

            fic = Program.cls.GetStoryInfoByID(StoryID);

            return fic;

        }

        public string GetStoryURL(string id)
        {

            string link;

            link = Program.cls.GetStoryURL(id);

            return link;

        }

        #endregion

        #region Validation Routines

        public bool CheckUrl(ref string link)
        {

            string host;
            bool ret = false;

            URL url;
            url = URLHelper.ExtractUrl(link);

            host = url.Host;

            if (Information.UBound(Strings.Split(host, ".")) == 2)
            {
                host = Strings.Mid(host, Strings.InStr(host, ".") + 1);
            }

            ret = LoadSiteByHost(host);

            if (Program.cls == null)
            {
                ret = false;
            }

            return ret;

        }

        #endregion

        #region Site Module Loading

        public bool LoadSiteByHost(string host)
        {

            bool ret = false;

            if (!(Program.cls == null))
            {
                if ((host ?? "") == (Program.cls.HostName ?? ""))
                {
                    return true;
                }
                else
                {
                    Program.cls = null;
                }
            }

            switch (host ?? "")
            {
                case "fanfiction.net":
                    {
                        Program.cls = new FFNet();
                        break;
                    }
                case "adult-fanfiction.org":
                    {
                        Program.cls = new AFF();
                        break;
                    }
                case "ficwad.com":
                    {
                        Program.cls = new FicWad();
                        break;
                    }
                case "mediaminer.org":
                    {
                        Program.cls = new MM();
                        break;
                    }
                case "hpfanficarchive.com":
                    {
                        Program.cls = new HPFFA();
                        break;
                    }

                default:
                    {
                        Program.cls = null;
                        break;
                    }
            }

            if (!(Program.cls == null))
            {
                ret = true;
            }

            return ret;

        }

        public bool LoadSiteByName(string clsname)
        {

            bool ret = true;
            string host = "";

            Program.cls = null;

            switch (clsname ?? "")
            {
                case "FFNet":
                    {
                        Program.cls = new FFNet();
                        break;
                    }
                case "AFF":
                    {
                        Program.cls = new AFF();
                        break;
                    }
                case "FicWad":
                    {
                        Program.cls = new FicWad();
                        break;
                    }
                case "MediaMiner":
                    {
                        Program.cls = new MM();
                        break;
                    }
                case "HPFFA":
                    {
                        Program.cls = new HPFFA();
                        break;
                    }

                default:
                    {
                        Program.cls = null;
                        break;
                    }
            }

            if (!(Program.cls == null))
            {
                ret = true;
            }
            else
            {
                ret = false;
            }

            return ret;

        }

        #endregion

        #region Properties

        public string Result
        {
            get
            {
                return txtResult;
            }
        }

        public clsFanfic.Story FanFic
        {
            get
            {
                return Story;
            }
        }

        public string Host
        {
            get
            {
                if (!(Program.cls == null))
                {
                    return Program.cls.HostName;
                }
                else
                {
                    return "";
                }
            }
        }

        public string Name
        {
            get
            {
                if (!(Program.cls == null))
                {
                    return Program.cls.Name;
                }
                else
                {
                    return "";
                }
            }
        }

        public string ErrorMessage
        {
            get
            {
                if (!(Program.cls == null))
                {
                    return Program.cls.ErrorMessage;
                }
                else
                {
                    return "";
                }
            }
        }

        #endregion

    }
}