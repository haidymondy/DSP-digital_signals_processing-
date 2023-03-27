﻿/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            List<float> phase = new List<float>();
            List<float> amp = new List<float>();
            float phi = 0;

            List<float> X = InputTimeDomainSignal.Samples;
            float N = InputTimeDomainSignal.Samples.Count;

            for (int k = 0; k < N; k++)
            {
                float real = 0;
                float im = 0;
                for (int n = 0; n < N; n++)
                {
                    phi = (2 * (float)Math.PI * k * n) / N;
                    real += X[n] * (float)Math.Cos(phi);
                    im += -X[n] * (float)Math.Sin(phi);
                }
                var ampp = Math.Sqrt(real * real + im * im);
                var phasse = Math.Atan2(im, real);

                phase.Add((float)phasse);
                amp.Add((float)ampp);
            }
            OutputFreqDomainSignal = new Signal(X, false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = amp;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = phase;
        }
    }
}
*/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DSPAlgorithms.DataStructures;

//namespace DSPAlgorithms.Algorithms
//{
//    public class DiscreteFourierTransform : Algorithm
//    {
//        public Signal InputTimeDomainSignal { get; set; }
//        public float InputSamplingFrequency { get; set; }
//        public Signal OutputFreqDomainSignal { get; set; }

//        public override void Run()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            List<float> ampl = new List<float>();
            List<float> phase = new List<float>();
            List<float> xaxis = new List<float>();

            int N = InputTimeDomainSignal.Samples.Count;
            float omg = ((float)(2 * Math.PI / N * (1 / InputSamplingFrequency)));    //omega 
            for (int k = 0; k < N; k++)
            {
                double x = 0;//cos 
                double y = 0; //sin 
                double z = 0;
                for (int n = 0; n < N; n++)
                {
                    x += InputTimeDomainSignal.Samples[n] * Math.Cos((k * 2 * Math.PI * n) / N);  // real 
                    y -= InputTimeDomainSignal.Samples[n] * Math.Sin((k * 2 * Math.PI * n) / N); // imagen
                }
                //A=sqrt(x^2+y^2)
                double amp = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                ampl.Add((float)amp);
                // phaseshift
                double pha = Math.Atan2(y, x);
                phase.Add((float)pha);
                // x_axis
                z = omg * (k + 1); // multiples  of omega

                xaxis.Add((float)z);
            }
            OutputFreqDomainSignal = new Signal(false, xaxis, ampl, phase);
        }
    }
}
