using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }
        public List<float> list = new List<float>();
        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<float> X = InputSignal1.Samples;
            List<float> H = InputSignal2.Samples;

            int start = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0];
            int end = InputSignal1.SamplesIndices[InputSignal1.Samples.Count - 1] + InputSignal2.SamplesIndices[InputSignal2.Samples.Count - 1];
            float Y;
            OutputConvolvedSignal = new Signal(list, false);
            for (int n = start; n <= end; n++)//start ann end
            {
                Y = 0;
                for (int k = InputSignal1.SamplesIndices[0]; k < InputSignal1.Samples.Count(); k++)// InputSignal1.SamplesIndices
                {
                    if ((n - k) >= InputSignal2.Samples.Count() || k > InputSignal1.SamplesIndices.Max() ||   
                        (n - k) > InputSignal2.SamplesIndices.Max() || k < InputSignal1.SamplesIndices.Min())
                        continue;
                    else if ((n - k) < InputSignal2.SamplesIndices[0])//check negative
                        break;
                    else//if okay
                    {
                        int index1 = InputSignal1.SamplesIndices.IndexOf(k); //index of k
                        int index2 = InputSignal2.SamplesIndices.IndexOf(n - k); //index of n-k
                        Y += (X[index1] * H[index2]);  
                    }

                }
                if (n == end && Y == 0) //ignore it 
                    continue;
                OutputConvolvedSignal.Samples.Add(Y);//get samples
                OutputConvolvedSignal.SamplesIndices.Add(n);// get samples indecies
            }
        }
    }
}
