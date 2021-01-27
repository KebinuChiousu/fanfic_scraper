using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace Web
{
    public static class Util
    {

        /// <summary>  
        /// This is equivalent to Mid Function in VB6  
        /// </summary>  
        /// <param name="s"> String to Check.</param>  
        /// <param name="a">Position of Character</param>  
        /// <param name="b">Length </param>  
        /// <returns></returns>  
        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a, b);

            return temp;
        }

        public static string Mid(string s, int a)
        {
            string temp = s.Substring(a);
            return temp;
        }

        public static bool IsNothing(object value)
        {
            if (value == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Stream EmbeddedObj(string Name)
        {
            Assembly assy;

            Stream obj;

            string str = "";

            assy = Assembly.GetExecutingAssembly();
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
                    break;
                outfile.Write(buf, 0, nRead);
            }

            outfile.Close();
        }

        public static string GetTempFileName()
        {
            return System.IO.Path.GetTempFileName();
        }

        public static bool RunAndWait(string command, string commandLine
                        )
        {
            bool ret;

            string path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Util)).Location);

            try
            {
                var runProcess = new Process();

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
                ret = true;
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;
        }

        public static Stream StringToStream(string data)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            MemoryStream ms = new MemoryStream(bytes);

            return (Stream)ms;
        }

        public static string FormatLineEndings(string str)
        {

            // this function converts all line endings to Windows CrLf line endings
            string prevChar;
            string nextChar;
            string curChar;

            string strRet;

            int X;

            nextChar = "";
            curChar = "";
            strRet = "";

            for (X = 1; X <= Strings.Len(str); X++)
            {
                prevChar = curChar;
                curChar = Mid(str, X, 1);

                if (nextChar != Constants.vbNullString & curChar != nextChar)
                {
                    curChar +=  nextChar;
                    nextChar = "";
                }
                else if (curChar == Constants.vbLf)
                {
                    if (prevChar != Constants.vbCr)
                        curChar = Constants.vbCrLf;

                    nextChar = "";
                }
                else if (curChar == Constants.vbCr)
                    nextChar = Constants.vbLf;

                strRet += curChar;
            }

            return strRet;
        }

        public static int LastPos(string source, string search)
        {
            int idx = 1;
            int last_idx = 0;

            while (idx > 0)
            {
                idx = Strings.InStr(last_idx + 1, source, search);
                if (idx > 0)
                    last_idx = idx;
            }

            return last_idx;
        }

        public static string ConvertToAscii(string str)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] bytearray;
            byte[] asciiarray;
            string ret;

            bytearray = Encoding.UTF8.GetBytes(str);
            asciiarray = Encoding.Convert(Encoding.UTF8, Encoding.ASCII, bytearray);
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

            string[] strFiles = Directory.GetFiles(@base, "*.htm", SearchOption.TopDirectoryOnly);
            string newfilename;
            int idx;
            FileInfo fi;

            int num;

            if (strFiles.Length > 0)
            {
                for (idx = Information.UBound(strFiles); idx >= 0; idx += -1)
                {
                    string filename = strFiles[idx];
                    fi = new FileInfo(filename);

                    newfilename = fi.Name.Split(".")[0];

                    num = System.Convert.ToInt32(Strings.Mid(newfilename, Strings.Len(newfilename) - 1, 2));
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

            string[] strFiles = Directory.GetFiles(@base, "*.htm", SearchOption.TopDirectoryOnly);
            string newfilename;
            int idx;
            FileInfo fi;

            int num;

            if (strFiles.Length > 0)
            {
                for (idx = Information.UBound(strFiles); idx >= 0; idx += -1)
                {
                    string filename = strFiles[idx];
                    fi = new FileInfo(filename);
                    newfilename = fi.Name.Split(".")[0];

                    num = System.Convert.ToInt32(Strings.Mid(newfilename, Strings.Len(newfilename) - 1, 2));
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
