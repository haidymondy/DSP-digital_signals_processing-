using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;


namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            List<Complex> list1complex = new List<Complex>();
            List<Complex> list2complex = new List<Complex>();
            float sum1 = 0;
            float sum2 = 0;
            float norm;
            double theta;
            for (int k = 0; k < InputSignal1.Samples.Count; k++)    //DFT
            {
                float real = 0;
                float imagine = 0;
                for (int n = 0; n < InputSignal1.Samples.Count; n++)
                {

                    theta = (double)(k * 2 * Math.PI * n) / InputSignal1.Samples.Count;

                    real += (float)Math.Cos(theta) * InputSignal1.Samples[n];

                    imagine += -(float)Math.Sin(theta) * InputSignal1.Samples[n];

                }

                Complex number = new Complex(real, imagine);
                list1complex.Add(number);
            }
            if (InputSignal2 != null)
            {
                for (int k = 0; k < InputSignal2.Samples.Count; k++)    //DFT
                {
                    float real = 0;
                    float imagine = 0;
                    for (int n = 0; n < InputSignal2.Samples.Count; n++)
                    {

                        theta = (double)(k * 2 * Math.PI * n) / InputSignal2.Samples.Count;

                        real += (float)Math.Cos(theta) * InputSignal2.Samples[n];

                        imagine += -(float)Math.Sin(theta) * InputSignal2.Samples[n];

                    }
                    Complex number = new Complex(real, imagine);
                    list2complex.Add(number);
                }
            }

            List<Complex> listcomplex = new List<Complex>();
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                sum1 = sum1 + (InputSignal1.Samples[i] * InputSignal1.Samples[i]);
            }
            if (InputSignal2 == null)
            {
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    listcomplex.Add(Complex.Multiply(list1complex[i], Complex.Conjugate(list1complex[i])));



                norm = (float)Math.Sqrt(sum1 * sum1) / InputSignal1.Samples.Count();
            }
            else
            {
                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                    listcomplex.Add(Complex.Multiply(list2complex[i], Complex.Conjugate(list1complex[i])));

                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                    sum2 = sum2 + (InputSignal2.Samples[i] * InputSignal2.Samples[i]);


                norm = (float)Math.Sqrt(sum1 * sum2) / InputSignal1.Samples.Count;

            }
            List<float> output = new List<float>();
            float N = InputSignal1.Samples.Count;
            List<float> normalized = new List<float>();
            for (int i = 0; i < N; i++) // IDFT
            {
                Complex number = 0;
                Complex j = new Complex(0, 1);
                float sample = 0;

                for (int k = 0; k < N; k++)
                    number += listcomplex[k] * (Math.Cos(2 * Math.PI * k * i / N) + (j * Math.Sin(2 * Math.PI * k * i / N)));


                sample = (float)number.Real / (N * N);

                output.Add(sample);

                normalized.Add(sample / norm);
            }
            OutputNonNormalizedCorrelation = output;
            OutputNormalizedCorrelation = normalized;
        }
    }
}