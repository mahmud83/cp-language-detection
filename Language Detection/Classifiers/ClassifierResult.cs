using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language_Detection
{
    class ClassifierResult
    {
        public int Correct { get; set; }

        public int Incorrect { get; set; }

        public int Total
        {
            get
            {
                return Correct + Incorrect;
            }
        }

        public double Accuracy
        {
            get
            {
                if (Correct > 0 || Incorrect > 0)
                {
                    return (Correct * 100.0) / (Correct + Incorrect);
                }

                return 0;
            }
        }
    }
}
