using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Detection
{
    class FolderSnippetParser : ISnippetParser
    {
        public List<CodeSnippet> ExtractLabeledCodeSnippets(string folder)
        {
            List<CodeSnippet> snippets = new List<CodeSnippet>();

            foreach (string dir in Directory.GetDirectories(folder))
            {
                string language = new FileInfo(dir).Name;
                string extension = GetFileExtension(language);

                foreach (string file in Directory.GetFiles(dir, extension, SearchOption.AllDirectories))
                {
                    // If we get more than 2,000 samples from a single language, then stop
                    if (snippets.Count(x => x.Language == language) > 2000)
                    {
                        break;
                    }

                    try
                    {
                        string contents = File.ReadAllText(file, Encoding.UTF8);

                        List<string> parts = new List<string>();

                        if (contents.Length > 4096)
                        {
                            List<string> lines = contents.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            int maxLines = 100;

                            for (int i = 0; i < lines.Count; i+= maxLines)
                            {
                                string code = "";

                                for (int c = i; c < Math.Min(i + maxLines, lines.Count); c ++)
                                {
                                    code += lines[c] + " ";
                                }

                                parts.Add(code);
                            }
                        }
                        else
                        {
                            parts.Add(contents);
                        }

                        foreach (string part in parts)
                        {
                            if (part.Trim().Length > 0)
                            {
                                // Don't add if it already exists in the list
                                if (!snippets.Any(x => x.Snippet == part))
                                {
                                    snippets.Add(new CodeSnippet(language, part));
                                }
                            }
                        }
                    }
                    catch (PathTooLongException)
                    {
                        // Blah
                    }
                }
            }

            return snippets;
        }

        private string GetFileExtension(string language)
        {
            switch (language.ToLower())
            {
                case "angular":
                    return "*.ts"; // confused with JavaScript
                case "asm":
                    return "*.asm";
                case "asp.net":
                    return "*.cshtml"; // includes Razor
                case "c#":
                    return "*.cs";
                case "c++":
                    return "*.cpp";
                case "css":
                    return "*.scss"; // close enough
                case "delphi":
                    return "*.pas"; // is Delphi kinda the same as Pascal?
                case "html":
                    return "*.html";
                case "java":
                    return "*.java";
                case "javascript":
                    return "*.js";
                case "objectivec":
                    return "*.m";
                case "pascal":
                    return "*.pas";
                case "perl":
                    return "*.pm";
                case "php":
                    return "*.php";
                case "powershell":
                    return "*.ps1";
                case "python":
                    return "*.py";
                case "react":
                    return "*.js"; // will probably be confused with JavaScript
                case "ruby":
                    return "*.rb";
                case "scala":
                    return "*.scala";
                case "sql":
                    return "*.sql";
                case "swift":
                    return "*.swift";
                case "typescript":
                    return "*.ts";
                case "vb.net":
                    return "*.vb";
                case "xml":
                    return "*.xml";
            }

            throw new Exception("Uncategorized language: " + language);
        }
    }
}
