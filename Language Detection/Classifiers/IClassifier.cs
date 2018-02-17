using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Detection
{
    interface IClassifier
    {
        string GetClassification(string snippet);

        ClassifierResult ScoreClassifier(List<CodeSnippet> snippets, string file);
    }
}
