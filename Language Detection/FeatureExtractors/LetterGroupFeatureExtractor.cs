using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Language_Detection
{
    class LetterGroupFeatureExtractor : IFeatureExtractor
    {
        public int LetterGroupLength { get; set; }

        public LetterGroupFeatureExtractor(int length)
        {
            LetterGroupLength = length;
        }

        public List<string> ExtractFeatures(string input)
        {
            List<string> features = new List<string>();

            // Clean input
            input = input.ToLower().Replace("\r", "").Replace("\n", "").Replace(" ", "");

            for (int i = 0; i < input.Length - LetterGroupLength; i++)
            {
                features.Add(input.Substring(i, LetterGroupLength));
            }

            return features;
        }
    }
}
