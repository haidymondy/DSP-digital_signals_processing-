
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;


namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            OutputConvolvedSignal = new Signal(new List<float>(), false);
            int x = (InputSignal1.Samples.Count + InputSignal2.Samples.Count) - 1;
            List<float> signal1 = new List<float>();
            List<float> signal2 = new List<float>();
            List<Complex> list1complex = new List<Complex>();
            List<Complex> list2complex = new List<Complex>();
            double theta1;
            double theta2;

            for (int i = InputSignal1.Samples.Count; i < x; i++)
                InputSignal1.Samples.Add(0);

            for (int i = InputSignal2.Samples.Count; i < x; i++)
                InputSignal2.Samples.Add(0);

            for (int k = 0; k < x; k++)    //DFT
            {
                float real1 = 0;
                float imagine1 = 0;
                float real2 = 0;
                float imagine2 = 0;
                for (int n = 0; n < InputSignal1.Samples.Count; n++)
                {

                    theta1 = (double)(k * 2 * Math.PI * n) / InputSignal1.Samples.Count;

                    real1 += (float)Math.Cos(theta1) * InputSignal1.Samples[n];

                    imagine1 += -(float)Math.Sin(theta1) * InputSignal1.Samples[n];

                    theta2 = (double)(k * 2 * Math.PI * n) / InputSignal2.Samples.Count;

                    real2 += (float)Math.Cos(theta2) * InputSignal2.Samples[n];

                    imagine2 += -(float)Math.Sin(theta2) * InputSignal2.Samples[n];

                }

                Complex number1 = new Complex(real1, imagine1);
                list1complex.Add(number1);

                Complex number2 = new Complex(real2, imagine2);
                list2complex.Add(number2);
            }
            float N = InputSignal1.Samples.Count;

            List<Complex> listcomplex = new List<Complex>();

            for (int i = 0; i < N; i++)
                listcomplex.Add(Complex.Multiply(list2complex[i], list1complex[i]));

            List<float> output = new List<float>();
            List<float> normalized = new List<float>();
            for (int i = 0; i < N; i++) // IDFT
            {
                Complex number = 0;
                Complex j = new Complex(0, 1);
                float sample = 0;

                for (int k = 0; k < N; k++)
                {
                    number += listcomplex[k] * (Math.Cos(2 * Math.PI * k * i / N)
                        + j * Math.Sin(2 * Math.PI * k * i / N));
                }

                sample = (float)number.Real / (N);

                output.Add(sample);

            }
            OutputConvolvedSignal.Samples = output;


        }
    }
}
