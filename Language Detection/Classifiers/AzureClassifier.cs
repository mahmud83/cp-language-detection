using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Language_Detection
{
    class AzureClassifier : IClassifier
    {
        private IFeatureExtractor fe;
        private string azureEndpoint;
        private string azureApiKey;

        public AzureClassifier(IFeatureExtractor featureExtractor, string endpoint, string apikey)
        {
            fe = featureExtractor;
            azureEndpoint = endpoint;
            azureApiKey = apikey;
        }
        
        public string GetClassification(string snippet)
        {
            return GetClassifications(new List<CodeSnippet>() { new CodeSnippet("", snippet) })[0].Guess;
        }

        private List<ClassifiedCodeSnippet> GetClassifications(List<CodeSnippet> snippets)
        {
            List<ClassifiedCodeSnippet> done = new List<ClassifiedCodeSnippet>();

            using (var client = new HttpClient())
            {
                string[,] values = new string[snippets.Count, 1];
                for (int i = 0; i < snippets.Count; i++)
                {
                    values[i, 0] = fe.ExtractFeatures(snippets[i].Snippet).Aggregate((c, n) => c + " " + n);
                }

                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "code",
                            new StringTable()
                            {
                                ColumnNames = new string[] { "Snippet"},
                                Values = values
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureApiKey);
                client.BaseAddress = new Uri(azureEndpoint);
                
                HttpResponseMessage response = client.PostAsJsonAsync("", scoreRequest).Result;

                if (response.IsSuccessStatusCode)
                {
                    dynamic json = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                    for (int i = 0; i < snippets.Count; i++)
                    {
                        done.Add(new ClassifiedCodeSnippet() { Snippet = snippets[i].Snippet, Guess = json.Results.language.value.Values[i][0], Correct = snippets[i].Language } );
                    }

                    return done;
                }
                else
                {
                    File.WriteAllText("error.log", response.Content.ReadAsStringAsync().Result);
                    return new List<ClassifiedCodeSnippet>();
                }
            }
        }

        public ClassifierResult ScoreClassifier(List<CodeSnippet> snippets, string file)
        {
            var correct = 0;

            using (StreamWriter w = new StreamWriter(file))
            {
                w.WriteLine("correct,guess,snippet");

                var classifications = GetClassifications(snippets);

                foreach (var result in classifications)
                {
                    if (result.Guess.ToLower() == result.Correct.ToLower())
                    {
                        correct++;
                    }

                    w.WriteLine(result.Correct + "," + result.Guess + ",\"" + result.Snippet.Replace("\r", "").Replace("\n", "").Replace(",", "").Replace("\"", "") + "\"");
                }

                w.WriteLine("accuracy," + (correct * 100.0 / classifications.Count).ToString("0.00"));
            }

            return new ClassifierResult
            {
                Correct = correct,
                Incorrect = snippets.Count - correct
            };
        }

        class StringTable
        {
            public string[] ColumnNames { get; set; }
            public string[,] Values { get; set; }
        }
    }
}
