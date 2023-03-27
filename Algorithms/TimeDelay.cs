using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            List<float> correlated_value = new List<float>();
            // call DirectCorrelation function
            DirectCorrelation directCorrelation = new DirectCorrelation();
            // Adding two input signals
            directCorrelation.InputSignal1 = InputSignal1;
            directCorrelation.InputSignal2 = InputSignal2;
            // Run the function
            directCorrelation.Run();

            int N = InputSignal1.Samples.Count();
            // Storing Output values
            for (int j = 0; j < N; j++)
            {
                correlated_value.Add(directCorrelation.OutputNormalizedCorrelation[j]);
            }

            // Find Maximum value & its index
            float max = Math.Abs(correlated_value[0]);
            int index = -1;
            for (int i = 1; i < N; i++)
            {
                if (Math.Abs(correlated_value[i]) > max)
                {
                    max = Math.Abs(correlated_value[i]);
                    index = i;
                }
            }

            if (index == -1)
            {
                OutputTimeDelay = 0; //maximun value found at index 0  => 0 * Ts = 0
            }
            else
            {
                OutputTimeDelay = InputSamplingPeriod * index;
            }
        }
    }
}