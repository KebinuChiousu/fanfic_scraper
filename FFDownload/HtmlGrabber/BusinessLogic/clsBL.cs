using System.Data;
using System.IO;
using System.Text;
using System.Web;
using Microsoft.VisualBasic;

namespace HtmlGrabber
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

            htmldoc = modMain.cls.GrabData(link);
            txtResult = htmldoc;

            if (Strings.InStr(Strings.LCase(txtResult), modMain.cls.ErrorMessage) == 0)
            {

                if (string.IsNullOrEmpty(txtResult))
                {
                    return false;
                }


                Story.Chapters = modMain.cls.GetChapters(htmldoc);



                htmldoc = modMain.cls.ProcessChapters(link, 0);



                Story.StoryURL = link;

                Story.ID = modMain.cls.GetStoryID(link);
                Story.Title = modMain.cls.GrabTitle(htmldoc);
                Story.Author = modMain.cls.GrabAuthor(htmldoc);
                Story.Category = modMain.cls.GrabSeries(htmldoc);

                Story.PublishDate = modMain.cls.GrabDate(htmldoc, "Published: ");
                Story.UpdateDate = modMain.cls.GrabDate(htmldoc, "Updated: ");

                if (string.IsNullOrEmpty(Story.UpdateDate))
                    Story.UpdateDate = Story.PublishDate;

                Story.ChapterCount = (Information.UBound(Story.Chapters) + 1).ToString();

                txtResult = modMain.cls.GrabBody(htmldoc);

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

                htmldoc = modMain.cls.ProcessChapters(link, idx - 1);



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

                Story.Title = modMain.cls.GrabTitle(htmlDoc);
                Story.Author = modMain.cls.GrabAuthor(htmlDoc);
                Story.Category = modMain.cls.GrabSeries(htmlDoc);
                Story.PublishDate = modMain.cls.GrabDate(htmlDoc, "Published: ");
                Story.UpdateDate = modMain.cls.GrabDate(htmlDoc, "Updated: ");

                if (string.IsNullOrEmpty(Story.UpdateDate))
                    Story.UpdateDate = Story.PublishDate;

                data = modMain.cls.GrabBody(htmlDoc);

                Body = "<p></p>" + data;

                txtResult = "<html><body>";
                txtResult += "<p>" + Story.Title + "</p>";
                txtResult += "<p>" + Story.Author + "</p>";

                if (!string.IsNullOrEmpty(Story.Category))
                {
                    txtResult += "<p>" + Story.Category + "</p>";
                }

                txtResult += modMain.cls.WriteDate(Story.PublishDate, Story.UpdateDate, chapter, ChapterCount);




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

            fic = modMain.cls.GrabStoryInfo(idx);

            return fic;

        }

        public DataSet GrabFeed(string link)
        {

            DataSet ds;

            ds = modMain.cls.GrabFeed(ref link);

            return ds;

        }

        public string GetStoryID(string link)
        {

            string id;

            id = modMain.cls.GetStoryID(link);

            return id;

        }

        public clsFanfic.Story GetStoryInfoByID(string StoryID)
        {

            clsFanfic.Story fic;

            fic = modMain.cls.GetStoryInfoByID(StoryID);

            return fic;

        }

        public string GetStoryURL(string id)
        {

            string link;

            link = modMain.cls.GetStoryURL(id);

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

            if (modMain.cls == null)
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

            if (!(modMain.cls == null))
            {
                if ((host ?? "") == (modMain.cls.HostName ?? ""))
                {
                    return true;
                }
                else
                {
                    modMain.cls = null;
                }
            }

            switch (host ?? "")
            {
                case "fanfiction.net":
                    {
                        modMain.cls = new FFNet();
                        break;
                    }
                case "adult-fanfiction.org":
                    {
                        modMain.cls = new AFF();
                        break;
                    }
                case "ficwad.com":
                    {
                        modMain.cls = new FicWad();
                        break;
                    }
                case "mediaminer.org":
                    {
                        modMain.cls = new MM();
                        break;
                    }
                case "hpfanficarchive.com":
                    {
                        modMain.cls = new HPFFA();
                        break;
                    }

                default:
                    {
                        modMain.cls = null;
                        break;
                    }
            }

            if (!(modMain.cls == null))
            {
                ret = true;
            }

            return ret;

        }

        public bool LoadSiteByName(string clsname)
        {

            bool ret = true;
            string host = "";

            modMain.cls = null;

            switch (clsname ?? "")
            {
                case "FFNet":
                    {
                        modMain.cls = new FFNet();
                        break;
                    }
                case "AFF":
                    {
                        modMain.cls = new AFF();
                        break;
                    }
                case "FicWad":
                    {
                        modMain.cls = new FicWad();
                        break;
                    }
                case "MediaMiner":
                    {
                        modMain.cls = new MM();
                        break;
                    }
                case "HPFFA":
                    {
                        modMain.cls = new HPFFA();
                        break;
                    }

                default:
                    {
                        modMain.cls = null;
                        break;
                    }
            }

            if (!(modMain.cls == null))
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
                if (!(modMain.cls == null))
                {
                    return modMain.cls.HostName;
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
                if (!(modMain.cls == null))
                {
                    return modMain.cls.Name;
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
                if (!(modMain.cls == null))
                {
                    return modMain.cls.ErrorMessage;
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