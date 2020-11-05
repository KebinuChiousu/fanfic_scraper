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
using HtmlAgilityPack;
using System.Xml;
using System.Web;

namespace web_scraper.Models.Utility
{
    public static class HtmlHelper
    {

        [DebuggerStepThrough()]
        public static HtmlAgilityPack.HtmlDocument CleanHTML(ref string html)
        {
            HtmlDocument ret;
            HtmlDocument doc = new HtmlDocument();

            StringWriter outputFile;

            outputFile = new StringWriter();

            doc.LoadHtml(html);

            doc.OptionFixNestedTags = true;
            doc.OptionOutputAsXml = true;

            doc.Save(outputFile);

            ret = doc;

            html = outputFile.ToString();

            outputFile.Close();

            doc = null/* TODO Change to default(_) if this is not a reference type */;

            return ret;
        }

        [DebuggerStepThrough()]
        public static string GetTitle(string html)
        {
            HtmlDocument doc;
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

            doc = null/* TODO Change to default(_) if this is not a reference type */;

            return title;
        }

        [DebuggerStepThrough()]
        public static HtmlNodeCollection FindNodesByValue(HtmlNode node, string NodeName, string NodeValue
                                )
        {
            HtmlNodeCollection ret;

            ret = node.SelectNodes("//" + NodeName + "[contains(text(), '" + NodeValue + "')]");

            return ret;
        }

        [DebuggerStepThrough()]
        public static HtmlNodeCollection FindNodesByAttribute(HtmlNode node, string NodeName, string Attr, string AttrValue, bool PartialMatch = true
                                    )
        {
            HtmlNodeCollection ret;


            if (PartialMatch)
                ret = node.SelectNodes("//" + NodeName + "[contains(@" + Attr + ", '" + AttrValue + "')]");
            else
                ret = node.SelectNodes("//" + NodeName + "[@" + Attr + "='" + AttrValue + "']");

            return ret;
        }

        [DebuggerStepThrough()]
        public static HtmlNodeCollection FindNodesByAttribute(HtmlNode node, string NodeName, string Attr
                                    )
        {
            HtmlNodeCollection ret;

            ret = node.SelectNodes("//" + NodeName + "[@" + Attr + "]");

            return ret;
        }

        [DebuggerStepThrough()]
        public static HtmlNodeCollection FindLinksByHref(HtmlNode node, string href
                            )
        {
            HtmlNodeCollection ret;

            ret = node.SelectNodes("//a[contains(@href, '" + href + "')]");

            return ret;
        }

        [DebuggerStepThrough()]
        public static string[] GetOptionValues(string htmlDoc, string param = ""
                                )
        {
            HtmlDocument doc;

            doc = CleanHTML(ref htmlDoc);

            string result = "";
            string[] values;
            int idx = 0;

            values = new string[1];

            HtmlNodeCollection temp;

            if (param == "")
            {
                temp = doc.DocumentNode.SelectNodes("//select");
            } else {
                temp = doc.DocumentNode.SelectNodes("//select[@title='" + param + "']");
            }

            if (Functions.IsNothing(temp))
                return null;

            if (temp.Count == 0)
                return null;

            result = temp[0].InnerHtml;

            result = "<select>" + result + "</select>";

            doc = CleanHTML(ref result);

            temp = doc.DocumentNode.SelectNodes("//option");

            foreach (HtmlNode node in temp)
            {
                if (node.NextSibling.InnerText != "")
                {
                    var oldValues = values;
                    values = new string[idx + 1];
                    if (oldValues != null)
                    {
                        Array.Copy(oldValues, values, Math.Min(idx + 1, oldValues.Length));
                    }
                        
                    values[idx] = node.NextSibling.InnerText;
                    idx = idx + 1;
                }
            }

            doc = null/* TODO Change to default(_) if this is not a reference type */;

            return values;
        }

        [DebuggerStepThrough()]
        public static HtmlNodeCollection GetListNodes(HtmlNode node, string Attr = "", string AttrValue = "", bool PartialMatch = true
                            )
        {
            string NodeName;
            string result;
            HtmlNodeCollection ret;
            HtmlNodeCollection temp;
            HtmlDocument doc;

            if (node.OuterHtml.IndexOf("ul") > 0)
                NodeName = "ul";
            else
                NodeName = "ol";

            if (Attr == "")
                temp = node.SelectNodes("//" + NodeName);
            else if (PartialMatch)
                temp = node.SelectNodes("//" + NodeName + "[contains(@" + Attr + ", '" + AttrValue + "')]");
            else
                temp = node.SelectNodes("//" + NodeName + "[@" + Attr + "='" + AttrValue + "']");

            if (Functions.IsNothing(temp))
                return null/* TODO Change to default(_) if this is not a reference type */;

            if (temp.Count == 0)
                return null/* TODO Change to default(_) if this is not a reference type */;

            result = temp[0].OuterHtml;

            doc = CleanHTML(ref result);

            ret = doc.DocumentNode.SelectNodes("//li");

            doc = null/* TODO Change to default(_) if this is not a reference type */;
            temp = null/* TODO Change to default(_) if this is not a reference type */;

            return ret;
        }

        [DebuggerStepThrough()]
        public static HtmlNodeCollection GetHTMLNodes(HtmlDocument htmldoc, string xpath
                            )
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

            result = System.Web.HttpUtility.HtmlDecode(text);

            if (result != text)
                ret = true;
            else
                ret = false;

            return ret;
        }

        [DebuggerStepThrough()]
        public static string DecodeHTML(string html)
        {
            string ret;

            ret = html;

            while (!IsHtmlEncoded(ret) == false | ret == "")
                ret = System.Web.HttpUtility.HtmlDecode(ret);

            return ret;
        }

        public static void StripTag(ref string html, string attribute, string value, bool PartialMatch = false)
        {
            HtmlDocument doc = new HtmlDocument();
            StringWriter outputFile;

            outputFile = new StringWriter();

            html = Functions.CleanString(html);

            doc.LoadHtml(html);

            List<HtmlNode> nodesToRemove = null;

            nodesToRemove = doc.DocumentNode.Descendants()
                                    .Where(n => n.Attributes.Contains(attribute))
                                    .ToList();

            foreach (var node in nodesToRemove)
            {
                if (node.Attributes[attribute].Value.Contains(value))
                    node.Remove();
            }

            doc.OptionOutputAsXml = true;

            doc.Save(outputFile);

            html = outputFile.ToString();

            outputFile.Close();

            doc = null/* TODO Change to default(_) if this is not a reference type */;
        }

        public static void StripTag(ref string html, string tag, bool PartialMatch = false)
        {
            HtmlDocument doc = new HtmlDocument();
            StringWriter outputFile;

            outputFile = new StringWriter();

            html = Functions.CleanString(html);

            doc.LoadHtml(html);

            List<HtmlNode> nodesToRemove = null;

            switch (PartialMatch)
            {
                case true:
                    {
                        nodesToRemove = doc.DocumentNode.Descendants()
                                        .Where(n => n.Name.Contains(tag))
                                        .ToList();
                        break;
                    }

                case false:
                    {
                        nodesToRemove = doc.DocumentNode.Descendants()
                                        .Where(n => n.Name == tag)
                                        .ToList();
                        break;
                    }
            }

            foreach (var node in nodesToRemove)
                node.Remove();

            doc.OptionOutputAsXml = true;

            doc.Save(outputFile);

            html = outputFile.ToString();

            outputFile.Close();

            doc = null/* TODO Change to default(_) if this is not a reference type */;
        }
    }
}
