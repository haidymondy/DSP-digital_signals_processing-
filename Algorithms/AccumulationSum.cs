using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            // throw new NotImplementedException();

            //claculate the sum of all the previous samples befor spacific index  3->>(0+1+2) etc..
            List<float> X = new List<float>();
            float sum = 0;
            for (int i= 0; i < InputSignal.Samples.Count; i++)
            {
                 sum = 0;
                for (int j = i; j >= 0; j--) 
                {
                    //  InputSignal.Samples[i] += InputSignal.Samples[j] ;
                    sum += InputSignal.Samples[j];
                }
                X.Add(sum);
            }
            OutputSignal = new Signal(X, false);
        }
    }
}
