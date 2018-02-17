using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Detection
{
    interface ISnippetParser
    {
        List<CodeSnippet> ExtractLabeledCodeSnippets(string input);
    }
}
