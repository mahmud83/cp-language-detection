using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Detection
{
    class CleanFeatureExtractor : IFeatureExtractor
    {
        public List<string> ExtractFeatures(string input)
        {
            return input.Replace("\r", " ").Replace("\n", " ").Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
