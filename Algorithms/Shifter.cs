using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            // throw new NotImplementedException();
            List<float> X = new List<float>();
            
                        if (InputSignal.Periodic == false)
                        {
                            for (int i = 0; i < InputSignal.Samples.Count; i++)
                            {
                                InputSignal.SamplesIndices[i] -= ShiftingValue;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < InputSignal.Samples.Count; i++)
                            {
                                InputSignal.SamplesIndices[i] += ShiftingValue;
                            }
                        }
                        OutputShiftedSignal = InputSignal;

         
        }
    }

}
