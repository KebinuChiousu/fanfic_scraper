using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Web;

using web_scraper.Models.Sites;
using web_scraper.Models.Sites.Base;
using web_scraper.Models.Utility;

namespace web_scraper.Models
{
    public class BusinessLogic
    {

        private Fanfic cls = null;

        public BusinessLogic()
        {
        }

        private string txtResult;
        private Story story = new Story();

        public string OutputPath;

        public bool GetChapters(string link)
        {
            string htmldoc;

            // Download Information from Source

            if (link == "")
                return false;

            htmldoc = cls.GrabData(link);
            txtResult = htmldoc;

            if (Strings.InStr(Strings.LCase(txtResult), cls.ErrorMessage) == 0)
            {
                if (txtResult == "")
                    return false;


                story.Chapters = cls.GetChapters(htmldoc);



                htmldoc = cls.ProcessChapters(link, 0
                                            );

                story.StoryURL = link;

                story.ID = cls.GetStoryID(link);
                story.Title = cls.GrabTitle(htmldoc);
                story.Author = cls.GrabAuthor(htmldoc);
                story.Category = cls.GrabSeries(htmldoc);

                story.PublishDate = cls.GrabDate(htmldoc, "Published: ");
                story.UpdateDate = cls.GrabDate(htmldoc, "Updated: ");

                if (story.UpdateDate == null)
                    story.UpdateDate = story.PublishDate;

                story.ChapterCount = story.Chapters.Length;

                txtResult = cls.GrabBody(htmldoc);

                return true;
            }
            else
                return false;
        }

        public void ProcessChapters(string link, int Start, int Count, string FileMask, string Category = ""
                                )
        {
            string htmldoc;
            int idx;
            string msg;

            // Process Chapters from Source

            for (idx = Start; idx <= Count; idx++)
            {
                msg = "Chapter " + idx + " of " + Count;

                //frmMain.UpdateProgess(msg);

                htmldoc = cls.ProcessChapters(link, idx - 1
                                            );

                ProcessChapter(ref htmldoc, FileMask, idx, Count, link, Category
                            );
            }
        }

        public void ProcessChapter(ref string htmlDoc, string FileMask, int chapter, int ChapterCount = 0, string link = "", string Category = ""
    )
        {

            // Dim ecp1252 As Encoding = Encoding.GetEncoding(1252)
            StreamReader sr;
            StreamWriter sw;

            string Body;
            string data = "";

            string Path;

            if (ChapterCount == 0)
                ChapterCount = chapter;


            txtResult = htmlDoc;

            if (Strings.InStr(Strings.LCase(txtResult), "chapter not found") == 0 & Strings.InStr(Strings.LCase(txtResult), "story not found") == 0)
            {
                story.Title = cls.GrabTitle(htmlDoc);
                story.Author = cls.GrabAuthor(htmlDoc);
                story.Category = cls.GrabSeries(htmlDoc);
                story.PublishDate = cls.GrabDate(htmlDoc, "Published: ");
                story.UpdateDate = cls.GrabDate(htmlDoc, "Updated: ");

                if (story.UpdateDate == null)
                    story.UpdateDate = story.PublishDate;

                data = cls.GrabBody(htmlDoc);

                Body = "<p></p>" + data;

                txtResult = "<html><body>";
                txtResult += "<p>" + story.Title + "</p>";
                txtResult += "<p>" + story.Author + "</p>";

                if (story.Category != "")
                    txtResult += "<p>" + story.Category + "</p>";

                txtResult += cls.WriteDate(story.PublishDate, story.UpdateDate, chapter, ChapterCount
                                        );
                txtResult += "<p>----------------------------------</p>";
                txtResult += Body;
            }
            else
            {
                txtResult = "<p>Error writing file</p>";
                txtResult += "<p>Try downloading the file from " + "<a href=" + Strings.Chr(34) + link + chapter + "/" + Strings.Chr(34) + ">here</a> in a regular browser</p>";
                txtResult += "<p>DOWNLOAD ERROR</p>";
            }

            txtResult = HttpUtility.HtmlDecode(txtResult);

            sr = new StreamReader(Functions.StringToStream(txtResult));

            FileMask = Strings.Replace(FileMask, "-", "");

            if (Information.IsNumeric(Strings.Mid(FileMask, Strings.Len(FileMask), 1)))
                FileMask += "-";

            Path = OutputPath;

            if (Category != "")
            {
                Path += @"\";
                Path += Strings.Replace(Category, " ", "_");
            }

            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            sw = new StreamWriter(Path
                                + @"\" + FileMask + Strings.Format(chapter, "0#") + ".htm", false
                                );

            sw.Write(sr.ReadToEnd());
            sr.Close();
            sw.Close();
            sr = null;
            sw = null;
        }

        public void ProcessError(string link, int chapter, string filemask, string category = "")
        {
            Encoding ecp1252 = Encoding.GetEncoding(1252);
            StreamReader sr;
            StreamWriter sw;

            string Path;

            string txtResult;

            txtResult = "<p>Error writing file</p>";
            txtResult += "<p>Try downloading the file from " + "<a href=" + Strings.Chr(34) + link + chapter + Strings.Chr(34) + ">here</a> in a regular browser</p>";
            txtResult += "<p>DOWNLOAD ERROR</p>";

            txtResult = HttpUtility.HtmlDecode(txtResult);

            sr = new StreamReader(Functions.StringToStream(txtResult));

            filemask = Strings.Replace(filemask, "-", "");

            if (Information.IsNumeric(Strings.Mid(filemask, Strings.Len(filemask), 1)))
                filemask += "-";

            Path = OutputPath;

            if (category != "")
            {
                Path += @"\";
                Path += Strings.Replace(category, " ", "_");
            }

            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            sw = new StreamWriter(Path
                                + @"\" + filemask + Strings.Format(chapter, "0#") + ".htm", false, ecp1252
                                );

            sw.Write(sr.ReadToEnd());
            sr.Close();
            sw.Close();
            sr = null;
            sw = null;
        }


        public Story GrabStoryInfo(int idx)
        {
            Story fic = cls.GrabStoryInfo(idx);
            return fic;
        }

        public System.Data.DataSet GrabFeed(string link)
        {
            DataSet ds;

            ds = cls.GrabFeed(ref link);

            return ds;
        }

        public string GetStoryID(string link)
        {
            string id;

            id = cls.GetStoryID(link);

            return id;
        }

        public Story GetStoryInfoByID(string StoryID)
        {
            Story fic = cls.GetStoryInfoByID(StoryID);
            return fic;
        }

        public string GetStoryURL(string id)
        {
            string link;

            link = cls.GetStoryURL(id);

            return link;
        }

        public bool CheckUrl(ref string link)
        {
            string host;
            bool ret = false;

            Url url = new Url();
            url = UrlHelper.ExtractUrl(link);

            host = url.Host;

            if (Information.UBound(Strings.Split(host, ".")) == 2)
                host = Strings.Mid(host, Strings.InStr(host, ".") + 1);

            ret = LoadSiteByHost(host);

            if (Functions.IsNothing(cls))
                ret = false;

            return ret;
        }



        public bool LoadSiteByHost(string host)
        {
            bool ret = false;

            if (!Functions.IsNothing(cls))
            {
                if (host == cls.HostName)
                    return true;
                else
                    cls = null;
            }

            switch (host)
            {
                case "fanfiction.net":
                    {
                        cls = new FanFicNet();
                        break;
                    }

                default:
                    {
                        cls = null;
                        break;
                    }
            }

            if (cls != null)
                ret = true;

            return ret;
        }

        public bool LoadSiteByName(string clsname)
        {
            bool ret = true;

            cls = null;

            switch (clsname)
            {
                case "FFNet":
                    {
                        cls = new FanFicNet();
                        break;
                    }

                default:
                    {
                        cls = null;
                        break;
                    }
            }

            if (cls != null)
                ret = true;
            else
                ret = false;

            return ret;
        }



        public string Result
        {
            get
            {
                return txtResult;
            }
        }

        public Story FanFic
        {
            get
            {
                return story;
            }
        }

        public string Host
        {
            get
            {
                if (cls != null)
                    return cls.HostName;
                else
                    return "";
            }
        }

        public string Name
        {
            get
            {
                if (cls != null)
                    return cls.Name;
                else
                    return "";
            }
        }

        public string ErrorMessage
        {
            get
            {
                if (cls != null)
                    return cls.ErrorMessage;
                else
                    return "";
            }
        }
    }
}
