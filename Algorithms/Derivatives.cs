using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> x = InputSignal.Samples;
            List<float> output_1 = new List<float>();
            List<float> output_2 = new List<float>();

            for (int i = 1; i < InputSignal.Samples.Count; i++) //first derivative
            {
                float y1 = x[i] - x[i - 1];
                output_1.Add(y1);
            }

            for (int i = 1; i < InputSignal.Samples.Count - 1; i++) //second derivative
            {
                float y2 = x[i + 1] - 2 * x[i] + x[i - 1];
                output_2.Add(y2);
            }
            FirstDerivative = new Signal(output_1, false);
            SecondDerivative = new Signal(output_2, false);
        }
    }
}
