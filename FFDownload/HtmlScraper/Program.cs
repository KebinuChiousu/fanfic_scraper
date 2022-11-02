using HtmlScraper.BusinessLogic;
using HtmlScraper.BusinessLogic.Sites.Base;
using HtmlScraper.Data.Ini;
using HtmlScraper.Utility;
using HtmlScraper.Utility.Browser;

namespace HtmlScraper
{
    internal static class Program
    {
        public static clsFanfic cls;
        public static clsBL BL = new clsBL();
        public static clsWeb Browser;
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // TODO: Update Selenium to hide browser window
            // Browser = new clsWeb();
            InitIniFile();
            Application.Run(new Gui.HtmlGrabber());
            // Browser.Dispose();
        }

        public static void InitIniFile()
        {

            string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            FileInfo fi;
            IniFileReader ifr;
            string val = "";

            fi = new FileInfo(Application.StartupPath + @"\\" + "config.ini");

            if (fi.Exists)
            {
                ifr = new IniFileReader(Application.StartupPath + @"\config.ini", true);
                val = ifr.GetIniValue("Output", "Path");
            }

            if (string.IsNullOrEmpty(val))
            {
                val = Desktop;
            }

            BL.OutputPath = val;
        }

        /*
        public static void ExtractResources()
        {

            string path = "";

            path = My.MyProject.Application.Info.DirectoryPath;

            if (!File.Exists(path + @"\" + "XMLtoINI.xslt"))
            {
                modUtility.GetEmbeddedFile("XMLtoINI.xslt");
            }

            if (!File.Exists(path + @"\" + "phantomjs.exe"))
            {
                modUtility.GetEmbeddedFile("phantomjs.exe");
            }

        }
        */
    }
}