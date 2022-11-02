using System;
using System.Diagnostics;
using System.IO;
using static System.Web.HttpUtility;
using HtmlAgilityPack;
using Microsoft.VisualBasic;

namespace HtmlGrabber
{

    static class modHTML
    {

        [DebuggerStepThrough()]
        public static HtmlAgilityPack.HtmlDocument CleanHTML(ref string html)
        {

            HtmlAgilityPack.HtmlDocument ret;
            var doc = new HtmlAgilityPack.HtmlDocument();

            StringWriter outputFile;

            outputFile = new StringWriter();

            doc.LoadHtml(html);

            doc.OptionFixNestedTags = true;
            doc.OptionOutputAsXml = true;

            doc.Save(outputFile);

            ret = doc;

            html = outputFile.ToString();

            outputFile.Close();

            doc = null;

            return ret;

        }

        [DebuggerStepThrough()]
        public static string GetTitle(string html)
        {

            HtmlAgilityPack.HtmlDocument doc;
            string title;

            doc = CleanHTML(ref html);

            try
            {
                title = doc.DocumentNode.SelectSingleNode("//title").InnerText;
            }
            catch
            {
                title = "";
            }

            doc = null;

            return title;

        }

        [DebuggerStepThrough()]
        public static HtmlNodeCollection FindNodesByValue(HtmlNode node, string NodeName, string NodeValue)



        {

            HtmlNodeCollection ret;

            ret = node.SelectNodes("//" + NodeName + "[contains(text(), '" + NodeValue + "')]");

            return ret;

        }

        [DebuggerStepThrough()]
        public static HtmlNodeCollection FindNodesByAttribute(HtmlNode node, string NodeName, string Attr, string AttrValue, bool PartialMatch = true)





        {

            HtmlNodeCollection ret;


            if (PartialMatch)
            {
                ret = node.SelectNodes("//" + NodeName + "[contains(@" + Attr + ", '" + AttrValue + "')]");
            }
            else
            {
                ret = node.SelectNodes("//" + NodeName + "[@" + Attr + "='" + AttrValue + "']");
            }

            return ret;

        }

        [DebuggerStepThrough()]
        public static HtmlNodeCollection FindNodesByAttribute(HtmlNode node, string NodeName, string Attr)



        {

            HtmlNodeCollection ret;

            ret = node.SelectNodes("//" + NodeName + "[@" + Attr + "]");

            return ret;

        }

        [DebuggerStepThrough()]
        public static HtmlNodeCollection FindLinksByHref(HtmlNode node, string href)


        {

            HtmlNodeCollection ret;

            ret = node.SelectNodes("//a[contains(@href, '" + href + "')]");

            return ret;



        }

        [DebuggerStepThrough()]
        public static string[] GetOptionValues(string htmlDoc, string param = "")


        {

            HtmlAgilityPack.HtmlDocument doc;

            doc = CleanHTML(ref htmlDoc);

            string result = "";
            string[] values;
            int idx = 0;

            values = new string[1];

            HtmlNodeCollection temp;

            if (string.IsNullOrEmpty(param))
            {
                temp = doc.DocumentNode.SelectNodes("//select");
            }
            else
            {
                temp = doc.DocumentNode.SelectNodes("//select[@title='" + param + "']");
            }

            if (temp == null)
            {
                return null;
            }

            if (temp.Count == 0)
            {
                return null;
            }

            result = temp[0].InnerHtml;

            result = "<select>" + result + "</select>";

            doc = CleanHTML(ref result);

            int count;
            HtmlNode node;

            temp = doc.DocumentNode.SelectNodes("//option");


            var loopTo = temp.Count - 1;
            for (count = 0; count <= loopTo; count++)
            {
                node = temp[count];

                if (!string.IsNullOrEmpty(node.NextSibling.InnerText))
                {
                    Array.Resize(ref values, idx + 1);
                    values[idx] = node.NextSibling.InnerText;
                    idx = idx + 1;
                }

            }

            doc = null;

            return values;

        }

        [DebuggerStepThrough()]
        public static HtmlNodeCollection GetListNodes(HtmlNode node, string Attr = "", string AttrValue = "", bool PartialMatch = true)




        {

            string NodeName;
            string result;
            HtmlNodeCollection ret;
            HtmlNodeCollection temp;
            HtmlAgilityPack.HtmlDocument doc;

            if (Strings.InStr(node.OuterHtml, "ul") > 0)
            {
                NodeName = "ul";
            }
            else
            {
                NodeName = "ol";
            }

            if (string.IsNullOrEmpty(Attr))
            {
                temp = node.SelectNodes("//" + NodeName);
            }
            else if (PartialMatch)
            {
                temp = node.SelectNodes("//" + NodeName + "[contains(@" + Attr + ", '" + AttrValue + "')]");
            }
            else
            {
                temp = node.SelectNodes("//" + NodeName + "[@" + Attr + "='" + AttrValue + "']");
            }

            if (temp == null)
            {
                return null;
            }

            if (temp.Count == 0)
            {
                return null;
            }

            result = temp[0].OuterHtml;

            doc = CleanHTML(ref result);

            ret = doc.DocumentNode.SelectNodes("//li");

            doc = null;
            temp = null;

            return ret;

        }

        [DebuggerStepThrough()]
        public static HtmlNodeCollection GetHTMLNodes(HtmlAgilityPack.HtmlDocument htmldoc, string xpath)


        {

            HtmlNodeCollection XmlList;

            XmlList = htmldoc.DocumentNode.SelectNodes(xpath);

            return XmlList;

        }

        [DebuggerStepThrough()]
        public static bool IsHtmlEncoded(string text)
        {

            bool ret = false;
            string result;

            result = HtmlDecode(text);

            if ((result ?? "") != (text ?? ""))
            {
                ret = true;
            }
            else
            {
                ret = false;
            }

            return ret;

        }

        [DebuggerStepThrough()]
        public static string DecodeHTML(string html)
        {

            string ret;

            ret = html;

            while (!(IsHtmlEncoded(ret) == false | string.IsNullOrEmpty(ret)))
                ret = HtmlDecode(ret);

            return ret;

        }


    }
}