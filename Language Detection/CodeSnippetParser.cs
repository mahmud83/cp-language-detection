using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Language_Detection
{
    class CodeSnippetParser
    {
        public CodeSnippetParser()
        {
        }

        public List<CodeSnippet> ExtractLabeledCodeSnippets(string html)
        {
            List<CodeSnippet> snippets = new List<CodeSnippet>();

            MatchCollection matches = Regex.Matches(html, "<pre.*?lang\\=\\\"(.*?)\\\".*?>(.*?)<\\/pre>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            foreach (Match m in matches)
            {
                snippets.Add(new CodeSnippet(m.Groups[1].Value, HttpUtility.HtmlDecode(m.Groups[2].Value)));
            }

            return snippets;
        }
    }
}
