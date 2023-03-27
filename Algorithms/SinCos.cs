using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.CodeDom;


namespace DSPAlgorithms.Algorithms
{
    public class SinCos : Algorithm
    {
        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }




        public override void Run()
        {
            samples = new List<float>();

            if (type == "sin")
            {
                for (int i = 0; i < SamplingFrequency; i++)
                {
                    float result = (float)(A * (Math.Sin((2 * Math.PI * AnalogFrequency * i / SamplingFrequency) + PhaseShift)));
                    samples.Add(result);
                }
            }

            else if (type == "cos")
            {
                for (int i = 0; i < SamplingFrequency; i++)
                {
                    float result = (float)(A * (Math.Cos((2 * Math.PI * AnalogFrequency * i / SamplingFrequency) + PhaseShift)));
                    samples.Add(result);
                }
            }



        }

























        /*  public override void Run()
        {
            
            samples = new List<float>();
            float z, x;
         //   int n = (int) SamplingFrequency;
        
            if (SamplingFrequency > 2 * AnalogFrequency)
            {
                for (int i = 0; i < SamplingFrequency; i++)
                {
                    z = (float) Math.PI * 2 * AnalogFrequency/ SamplingFrequency;
                    if (type == "cos")
                        x = (float)(Math.Cos(z * i + PhaseShift) * A);

                    else if (type == "sin")
                    {
                        x = (float)(Math.Sin(z * i + PhaseShift) * A);
                        samples.Add(x);
                    }
                }
            }
        }*/
    }
}
//  float PI = 3.1415926535f;
//  double PI = Math.PI;

            // float n = (int) SamplingFrequency/ AnalogFrequency;