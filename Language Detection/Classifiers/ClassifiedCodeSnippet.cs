using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Detection
{
    class ClassifiedCodeSnippet
    {
        public string Snippet { get; set; }

        public string Correct { get; set; }

        public string Guess { get; set; }
    }
}
