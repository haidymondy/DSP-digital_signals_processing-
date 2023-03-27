using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            // throw new NotImplementedException();

            List<float> X = new List<float>();
            for (int i= InputWindowSize - 1; i < InputSignal.Samples.Count; i++) //get el sample & InputWindowSize
            {
                float x = 0;
                //get the avg of the sample with the windowsize 
                //avg the windowsize say =5 ->> (0,1,2,3,4).....6->>(1,2,3,4,5)....7->>(2,3,4,5,6) and so on for the rest of the signal

                for (int j = i - InputWindowSize+1 ; j <= i; j++)
                {
                    x+= InputSignal.Samples[j];
                }
     
                X.Add(x/InputWindowSize);
            }
            OutputAverageSignal = new Signal(X, InputSignal.SamplesIndices, false);
        }
    }
}
