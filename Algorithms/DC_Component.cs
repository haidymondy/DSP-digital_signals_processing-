using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using MathNet.Numerics.Statistics;
//using System.Linq;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float mean = 0;

            List<float> output_sig = new List<float>();

            mean = InputSignal.Samples.Sum() /  InputSignal.Samples.Count;

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                output_sig.Add(InputSignal.Samples[i] - mean);
            }
            OutputSignal = new Signal(output_sig, false);
        }
    }
}
