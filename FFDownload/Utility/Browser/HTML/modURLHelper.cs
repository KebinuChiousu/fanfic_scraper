using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace HtmlGrabber
{

    struct URL
    {
        public string Scheme;
        public string Host;
        public long Port;
        public string URI;
        public QueryString[] Query;
    }

    struct QueryString
    {
        public string Name;
        public string Value;
    }

    static class URLHelper
    {

        // returns as type URL from a string
        public static URL ExtractUrl(string strUrl)
        {
            URL ExtractUrlRet = default;
            int intPos1;
            int intPos2;

            var retURL = new URL();

            // 1 look for a scheme it ends with ://
            intPos1 = Strings.InStr(strUrl, "://");

            if (intPos1 > 0)
            {
                retURL.Scheme = Strings.Mid(strUrl, 1, intPos1 - 1);
                strUrl = Strings.Mid(strUrl, intPos1 + 3);
            }

            // 2 look for a port
            intPos1 = Strings.InStr(strUrl, ":");
            intPos2 = Strings.InStr(strUrl, "/");

            if (intPos1 > 0 & intPos1 < intPos2)
            {
                // a port is specified
                retURL.Host = Strings.Mid(strUrl, 1, intPos1 - 1);

                if (Information.IsNumeric(Strings.Mid(strUrl, intPos1 + 1, intPos2 - intPos1 - 1)))
                {
                    retURL.Port = Conversions.ToInteger(Strings.Mid(strUrl, intPos1 + 1, intPos2 - intPos1 - 1));
                }
            }
            else if (intPos2 > 0)
            {
                retURL.Host = Strings.Mid(strUrl, 1, intPos2 - 1);
            }
            else
            {
                retURL.Host = strUrl;
                retURL.URI = "/";

                ExtractUrlRet = retURL;
                return ExtractUrlRet;
            }

            strUrl = Strings.Mid(strUrl, intPos2);

            // find a question mark ?
            intPos1 = Strings.InStr(strUrl, "?");

            if (intPos1 > 0)
            {
                retURL.URI = Strings.Mid(strUrl, 1, intPos1 - 1);
                retURL.Query = SplitParms(Strings.Mid(strUrl, intPos1 + 1));
            }
            else
            {
                retURL.URI = strUrl;
            }

            ExtractUrlRet = retURL;
            return ExtractUrlRet;
        }

        public static QueryString[] SplitParms(string Query)
        {

            int idx;
            string[] parms;
            string[] value;
            QueryString[] ret;

            parms = Strings.Split(Query, "&");

            ret = new QueryString[Information.UBound(parms) + 1];

            var loopTo = Information.UBound(parms);
            for (idx = 0; idx <= loopTo; idx++)
            {

                value = Strings.Split(parms[idx], "=");

                ret[idx].Name = value[0];
                ret[idx].Value = value[1];

            }

            return ret;

        }

        // url encodes a string
        public static string URLEncode(string str)
        {
            string URLEncodeRet = default;
            int intLen;
            int X;
            long curChar;
            string newStr;
            intLen = Strings.Len(str);
            newStr = "";
            var loopTo = intLen;
            for (X = 1; X <= loopTo; X++)
            {
                curChar = Strings.Asc(Strings.Mid(str, X, 1));

                if ((curChar < 48L | curChar > 57L) & (curChar < 65L | curChar > 90L) & (curChar < 97L | curChar > 122L))

                {
                    newStr = newStr + "%" + Conversion.Hex(curChar);
                }
                else
                {
                    newStr = newStr + Strings.Chr((int)curChar);
                }
            }

            URLEncodeRet = newStr;
            return URLEncodeRet;
        }

        // decodes a url encoded string
        public static string UrlDecode(string str)
        {
            string UrlDecodeRet = default;
            int intLen;
            int X;
            string curChar;
            string strCode;

            string newStr;

            intLen = Strings.Len(str);
            newStr = "";

            var loopTo = intLen;
            for (X = 1; X <= loopTo; X++)
            {
                curChar = Strings.Mid(str, X, 1);

                if (curChar == "%")
                {
                    strCode = "&h" + Strings.Mid(str, X + 1, 2);

                    if (Information.IsNumeric(strCode))
                    {
                        curChar = Conversions.ToString(Strings.Chr(Conversions.ToInteger(Conversion.Int(strCode))));
                    }
                    else
                    {
                        curChar = "";
                    }
                    X = X + 2;
                }

                newStr = newStr + curChar;
            }

            UrlDecodeRet = newStr;
            return UrlDecodeRet;
        }

    }
}