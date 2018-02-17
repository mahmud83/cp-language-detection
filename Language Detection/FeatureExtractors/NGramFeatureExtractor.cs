using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Language_Detection
{
    class NGramFeatureExtractor : IFeatureExtractor
    {
        public List<string> ExtractFeatures(string input)
        {
            List<string> features = new List<string>();

            // Clean input
            input = input.ToLower().Replace("\r", " ").Replace("\n", " ");

            // Extract words
            features.AddRange(Regex.Replace(input, "[^a-z]", " ").Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));

            // Extract symbols
            features.AddRange(Regex.Replace(input, "[a-zA-Z0-9]", " ").Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));

            return features; 
        }
    }
}
