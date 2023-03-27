using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> X = new List<float>();
            List<int> SamplesIndices = new List<int>();


            X = new List<float>();
            for (int i = InputSignal.Samples.Count - 1; i >= 0; i--)
            {
                SamplesIndices.Add(-1*InputSignal.SamplesIndices[i]);
                X.Add(InputSignal.Samples[i]);
            }

            OutputFoldedSignal = new Signal(X, SamplesIndices, false);

            OutputFoldedSignal.Periodic = !InputSignal.Periodic;
        }
    }
}
