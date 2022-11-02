using System;
using System.IO;
using System.Windows.Forms;

namespace HtmlGrabber
{

    static class modMain
    {

        public static clsFanfic cls;
        public static clsBL BL = new clsBL();
        public static clsWeb Browser;

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

        public static void Main(string[] args)
        {

            var frmMain = new HtmlGrabber();
            Browser = new clsWeb();

            InitIniFile();

            frmMain.ShowDialog();

            Browser.Dispose();

        }


    }
}