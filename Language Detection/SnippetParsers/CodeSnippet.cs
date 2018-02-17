using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Detection
{
    class CodeSnippet
    {
        public string Snippet { get; set; }

        public string Language { get; set; }

        public CodeSnippet(string language, string snippet)
        {
            Language = language;
            Snippet = snippet;
        }
    }
}
