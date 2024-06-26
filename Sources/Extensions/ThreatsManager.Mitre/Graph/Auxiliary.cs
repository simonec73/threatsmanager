﻿using System.IO;
using HtmlAgilityPack;

namespace ThreatsManager.Mitre.Graph
{
    static class Auxiliary
    {
        public static string ConvertToString(this Capec.StructuredTextType structuredText)
        {
            string result = null;

            if (structuredText != null)
            {
                result = ConvertXhtml(structuredText.Serialize());
            }

            return result;
        }

        public static string ConvertToString(this Cwe.StructuredTextType structuredText)
        {
            string result = null;

            if (structuredText != null)
            {
                result = ConvertXhtml(structuredText.Serialize());
            }

            return result;
        }

        private static string ConvertXhtml(string xhtml)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(xhtml))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(xhtml);

                StringWriter sw = new StringWriter();
                ConvertTo(doc.DocumentNode, sw);
                sw.Flush();
                result = sw.ToString();
            }

            return result;
        }

        private static void ConvertTo(HtmlNode node, TextWriter outText)
        {
            string html;
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    // don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // script and style must not be output
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // get text
                    html = ((HtmlTextNode)node).Text;

                    // is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0)
                    {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name)
                    {
                        case "p":
                            // treat paragraphs as crlf
                            outText.Write("\r\n");
                            break;
                    }

                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }

        private static void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (HtmlNode subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }
    }
}
