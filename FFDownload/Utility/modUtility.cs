using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using static System.Reflection.Assembly;
using System.Text;
using static System.Text.Encoding;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace HtmlGrabber
{

    static class modUtility
    {

        public static Stream EmbeddedObj(string Name)
        {

            Assembly assy;

            Stream obj;

            string str = "";

            assy = GetExecutingAssembly();
            string[] resources;

            resources = assy.GetManifestResourceNames();

            foreach (string resourceName in resources)
            {
                if (Strings.InStr(resourceName, Name) != 0)
                {
                    str = resourceName;
                    break;
                }
            }

            obj = assy.GetManifestResourceStream(str);

            return obj;

        }

        public static void GetEmbeddedFile(string filename)
        {

            UnmanagedMemoryStream UMS;
            Stream outfile;
            const int sz = 4096;
            byte[] buf;
            int nRead;

            buf = new byte[4097];

            UMS = (UnmanagedMemoryStream)EmbeddedObj(filename);

            File.Delete(filename);
            outfile = File.Create(filename);

            while (true)
            {
                nRead = UMS.Read(buf, 0, sz);
                if (nRead < 1)
                {
                    break;
                }
                outfile.Write(buf, 0, nRead);
            }

            outfile.Close();


        }

        public static string GetTempFileName()
        {

            return Path.GetTempFileName();

        }

        public static bool runAndWait(string command, string commandLine)


        {
            bool runAndWaitRet = default;

            Process runProcess;
            string path;

            path = My.MyProject.Application.Info.DirectoryPath;

            try
            {
                runProcess = new Process();



                {
                    var withBlock = runProcess.StartInfo;
                    withBlock.FileName = path + @"\" + command;
                    withBlock.Arguments = commandLine;
                    // .WindowStyle = ProcessWindowStyle.Hidden
                    withBlock.WindowStyle = ProcessWindowStyle.Minimized;
                }

                runProcess.Start();

                // Wait until the process passes back an exit code 
                runProcess.WaitForExit();

                // Free resources associated with this proces
                runProcess.Close();
                runAndWaitRet = true;
            }
            catch (Exception ex)
            {
                runAndWaitRet = false;
            }

            return runAndWaitRet;
        }

        public static Stream StringToStream(string data)
        {

            var bytes = UTF8.GetBytes(data);
            var ms = new MemoryStream(bytes);

            return ms;

        }

        public static string FormatLineEndings(string str)
        {
            string FormatLineEndingsRet = default;
            // this function converts all line endings to Windows CrLf line endings
            string prevChar;
            string nextChar;
            string curChar;

            string strRet;

            long X;

            prevChar = "";
            nextChar = "";
            curChar = "";
            strRet = "";

            var loopTo = (long)Strings.Len(str);
            for (X = 1L; X <= loopTo; X++)
            {
                prevChar = curChar;
                curChar = Strings.Mid(str, (int)X, 1);

                if (!string.IsNullOrEmpty(nextChar) & (curChar ?? "") != (nextChar ?? ""))
                {
                    curChar = curChar + nextChar;
                    nextChar = "";
                }
                else if ((curChar ?? "") == Constants.vbLf)
                {
                    if ((prevChar ?? "") != Constants.vbCr)
                    {
                        curChar = Constants.vbCrLf;
                    }

                    nextChar = "";
                }
                else if ((curChar ?? "") == Constants.vbCr)
                {
                    nextChar = Constants.vbLf;
                }

                strRet = strRet + curChar;
            }

            FormatLineEndingsRet = strRet;
            return FormatLineEndingsRet;
        }

        public static int LastPos(string source, string search)
        {

            int idx = 1;
            int last_idx = 0;

            while (idx != 0)
            {
                idx = Strings.InStr(last_idx + 1, source, search);
                if (idx > 0)
                {
                    last_idx = idx;
                }
            }

            return last_idx;

        }

        public static string ConvertToAscii(string str)
        {

            var ecp1252 = GetEncoding(1252);

            var ascii = new ASCIIEncoding();
            byte[] bytearray;
            byte[] asciiarray;
            string ret;

            bytearray = UTF8.GetBytes(str);
            asciiarray = Convert(UTF8, ASCII, bytearray);
            ret = ascii.GetString(asciiarray);

            return ret;

        }

        public static string CleanString(string s)
        {

            string ret;

            ret = Regex.Replace(s, @"[^\x20-\x7E]", string.Empty);

            return ret;

        }

        public static void Increment_FileNumber(string Folder)
        {

            string @base = Folder;

            var strFiles = Directory.GetFiles(@base, "*.htm", SearchOption.TopDirectoryOnly);
            string filename = "";
            string newfilename = "";
            int idx;
            FileInfo fi;

            int num;

            if (strFiles.Length > 0)
            {
                for (idx = Information.UBound(strFiles); idx >= 0; idx -= 1)
                {
                    filename = strFiles[idx];
                    fi = new FileInfo(filename);
                    newfilename = Strings.Split(fi.Name, ".")[0];

                    num = Conversions.ToInteger(Strings.Mid(newfilename, Strings.Len(newfilename) - 1, 2));
                    num += 1;

                    newfilename = Strings.Mid(newfilename, 1, Strings.Len(newfilename) - 2);
                    newfilename += Strings.Format(num, "0#");
                    newfilename += ".htm";

                    fi.MoveTo(@base + @"\" + newfilename);

                }
            }

        }

        public static void Decrement_FileNumber(string Folder)
        {

            string @base = Folder;

            var strFiles = Directory.GetFiles(@base, "*.htm", SearchOption.TopDirectoryOnly);
            string filename = "";
            string newfilename = "";
            int idx;
            FileInfo fi;

            int num;

            if (strFiles.Length > 0)
            {
                for (idx = Information.UBound(strFiles); idx >= 0; idx -= 1)
                {
                    filename = strFiles[idx];
                    fi = new FileInfo(filename);
                    newfilename = Strings.Split(fi.Name, ".")[0];

                    num = Conversions.ToInteger(Strings.Mid(newfilename, Strings.Len(newfilename) - 1, 2));
                    num -= 1;

                    newfilename = Strings.Mid(newfilename, 1, Strings.Len(newfilename) - 2);
                    newfilename += Strings.Format(num, "0#");
                    newfilename += ".htm";

                    fi.MoveTo(@base + @"\" + newfilename);

                }
            }

        }

    }
}