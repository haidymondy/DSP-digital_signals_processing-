using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            List<float> X = new List<float>();
            float N = InputFreqDomainSignal.FrequenciesAmplitudes.Count;// signals_count 8
            for (int n = 0; n < N; n++)
            {
                // n is the index of the time domain signal
                float amp, pi, freq, shift, x = 0;
                pi = 2 * (float)Math.PI;
                // at each time domain signal time we need to calculate the sum of all the frequencies functions at that time
                for (int k = 0; k < N; k++)
                {
                    // k is the index of the frequency domain signal

                    // sinousoidal signal representation in frequency domain
                    amp = InputFreqDomainSignal.FrequenciesAmplitudes[k];
                    freq = InputFreqDomainSignal.Frequencies[k];
                    shift = InputFreqDomainSignal.FrequenciesPhaseShifts[k];

                    // formula in cos form: x += amp * cos(2 * pi * f * k / N + shift);
                    x += amp * (float)Math.Cos( pi * freq * n / N + shift);
                }
                X.Add(x / N);
            }
            OutputTimeDomainSignal = new Signal(X, false);
        }
    }
}