using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            List<float> InputSignal2_Samples = new List<float>();
            List<float> Non_Normalized = new List<float>();
            List<float> Normalized = new List<float>();
            float sum, r, normalized_result, normalized_value;
            float sum1 = 0;
            float sum2 = 0;
            int N = InputSignal1.Samples.Count();
            // First: get sum1 & sum2
            // Second: get Normalized value of Auto-Correlation or Cross-Correlation
            for (int i = 0; i < N; i++)
            {
                sum1 += (InputSignal1.Samples[i] * InputSignal1.Samples[i]); // Σ[x1(n)*x1(n)]
            }
            if (InputSignal2 == null) //Auto-Correlation because there is only one signal
            {
                for (int i = 0; i < N; i++)
                {
                    InputSignal2_Samples.Add(InputSignal1.Samples[i]);
                }
                normalized_value = sum1 / N; // [(sum1 * sum1)^1/2]/ N    =    sum1 / N
            }
            else //Cross-Correlation
            {
                for (int i = 0; i < N; i++)
                {
                    InputSignal2_Samples.Add(InputSignal2.Samples[i]); //adding second signal 
                }
                for (int i = 0; i < N; i++)
                {
                    sum2 += (InputSignal2.Samples[i] * InputSignal2.Samples[i]); // Σ[x1(n)*x2(n)]
                }
                normalized_value = (float)(Math.Sqrt(sum1 * sum2)) / N;// [(sum1*sum2)^1/2]/N
            }

            //Third: calculate correlation & normalization
            for (int j = 0; j < N; j++)
            {
                sum = 0;
                for (int n = 0; n < N; n++)
                {
                    sum += (InputSignal1.Samples[n] * InputSignal2_Samples[n]); // Σ[x1(n)*x1(n)] or Σ[x1(n)*x2(n)]
                }
                r = sum / (N); //  r11 or r12
                Non_Normalized.Add(r);
                normalized_result = r / normalized_value; // r11/ [[(sum1*sum1)^1/2]/N] or r12/ [[(sum1*sum2)^1/2]/N]
                Normalized.Add(normalized_result);

                float sample0 = InputSignal2_Samples[0];
                for (int i = 0; i < N - 1; i++) //shifting to prevent scaling & limit correlation -1 < r < +1
                {
                    InputSignal2_Samples[i] = InputSignal2_Samples[i + 1];
                }
                if (InputSignal1.Periodic == true)
                {
                    InputSignal2_Samples[N - 1] = sample0;
                }
                else if (InputSignal1.Periodic == false)
                {
                    InputSignal2_Samples[N - 1] = 0;
                }
            }
            OutputNonNormalizedCorrelation = Non_Normalized;
            OutputNormalizedCorrelation = Normalized;
        }
    }
}