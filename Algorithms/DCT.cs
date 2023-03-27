using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
         //   throw new NotImplementedException();

           
            List<float> y = new List<float>();
            float N = InputSignal.Samples.Count;

            for (int k = 0; k < N; k++)
            {
                float sum = 0;
                for (int i = 0; i < N; i++)
                {
                    sum += InputSignal.Samples[i] * (float) Math.Cos((Math.PI / (4 * N)) * (2 * i - 1) * (2 * k - 1));
                }
            y.Add(sum * (float) Math.Sqrt(2/N));

            }
            OutputSignal = new Signal(y, false);
        }
    }
}
