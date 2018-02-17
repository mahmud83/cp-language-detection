using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Detection
{
    interface IFeatureExtractor
    {
        List<string> ExtractFeatures(string input);
    }
}
