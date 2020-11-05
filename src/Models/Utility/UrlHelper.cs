using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace web_scraper.Models.Utility
{
    public static class UrlHelper
    {

        // returns as type URL from a string
        public static Url ExtractUrl(string strUrl)
        {
            int intPos1;
            int intPos2;

            Url ret = new Url();

            // 1 look for a scheme it ends with ://
            intPos1 = Strings.InStr(strUrl, "://");

            if (intPos1 > 0)
            {
                ret.Scheme = Strings.Mid(strUrl, 1, intPos1 - 1);
                strUrl = Strings.Mid(strUrl, intPos1 + 3);
            }

            // 2 look for a port
            intPos1 = Strings.InStr(strUrl, ":");
            intPos2 = Strings.InStr(strUrl, "/");

            if (intPos1 > 0 & intPos1 < intPos2)
            {
                // a port is specified
                ret.Host = Strings.Mid(strUrl, 1, intPos1 - 1);

                if ((Information.IsNumeric(Strings.Mid(strUrl, intPos1 + 1, intPos2 - intPos1 - 1))))
                    ret.Port = System.Convert.ToInt32(Strings.Mid(strUrl, intPos1 + 1, intPos2 - intPos1 - 1));
            }
            else if (intPos2 > 0)
                ret.Host = Strings.Mid(strUrl, 1, intPos2 - 1);
            else
            {
                ret.Host = strUrl;
                ret.Uri = "/";

                return ret;
            }

            strUrl = Strings.Mid(strUrl, intPos2);

            // find a question mark ?
            intPos1 = Strings.InStr(strUrl, "?");

            if (intPos1 > 0)
            {
                ret.Uri = Strings.Mid(strUrl, 1, intPos1 - 1);
                ret.Query = SplitParms(Strings.Mid(strUrl, intPos1 + 1));
            }
            else
                ret.Uri = strUrl;

            return ret;
        }

        public static QueryString[] SplitParms(string Query)
        {
            int idx;
            string[] parms;
            string[] value;
            QueryString[] ret;

            parms = Strings.Split(Query, "&");

            ret = new QueryString[Information.UBound(parms) + 1];

            for (idx = 0; idx <= Information.UBound(parms); idx++)
            {
                value = Strings.Split(parms[idx], "=");

                ret[idx].Name = value[0];
                ret[idx].Value = value[1];
            }

            return ret;
        }

        // url encodes a string
        public static string UrlEncode(string str)
        {
            int intLen;
            int X;
            int curChar;
            string newStr;
            intLen = Strings.Len(str);
            newStr = "";
            for (X = 1; X <= intLen; X++)
            {
                curChar = Strings.Asc(Strings.Mid(str, X, 1));

                if ((curChar < 48 | curChar > 57) & (curChar < 65 | curChar > 90) & (curChar < 97 | curChar > 122))
                    newStr = newStr + "%" + Conversion.Hex(curChar);
                else
                    newStr = newStr + Strings.Chr(curChar);
            }

            return newStr;
        }

        // decodes a url encoded string
        public static string UrlDecode(string str)
        {
            int intLen;
            int X;
            char curChar;
            string strCode;

            string newStr;

            intLen = Strings.Len(str);
            newStr = "";

            for (X = 1; X <= intLen; X++)
            {
                curChar = Strings.Mid(str, X, 1).ToCharArray()[0];

                if (curChar == '%')
                {
                    strCode = "&h" + Strings.Mid(str, X + 1, 2);

                    if (Information.IsNumeric(strCode))
                        curChar = Strings.Chr(int.Parse(strCode));
                    else
                        curChar = Strings.Chr(0);

                    X = X + 2;
                }

                if (curChar != Strings.Chr(0))
                {
                    newStr = newStr + curChar;
                }

            }

            return newStr;
        }
    }

}
