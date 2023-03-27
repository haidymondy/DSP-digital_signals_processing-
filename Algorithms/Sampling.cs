using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> Up_List = new List<float>();
            List<float> Down_List = new List<float>();
            if (M == 0 && L != 0)
            {
                //up sampling by L
                L = L - 1;
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    Up_List.Add(InputSignal.Samples[i]);
                    if (i == InputSignal.Samples.Count - 1)
                    { break; }
                    for (int j = 0; j < L; j++)
                    {
                        Up_List.Add(0);
                    }
                }
                //low pass filter
                FIR f = new FIR();
                f.InputTimeDomainSignal = new Signal(Up_List, false);
                f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                f.InputFS = 8000;
                f.InputStopBandAttenuation = 50;
                f.InputCutOffFrequency = 1500;
                f.InputTransitionBand = 500;
                f.Run();
                OutputSignal = f.OutputYn;
            }
            else if (M != 0 && L == 0)
            {
                //low pass filter
                FIR f = new FIR();
                f.InputTimeDomainSignal = InputSignal;
                f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                f.InputFS = 8000;
                f.InputStopBandAttenuation = 50;
                f.InputCutOffFrequency = 1500;
                f.InputTransitionBand = 500;
                f.Run();
                //down sampling by M
                for (int i = 0; i < f.OutputYn.Samples.Count; i += M)
                {
                    Down_List.Add(f.OutputYn.Samples[i]);
                }
                OutputSignal = new Signal(Down_List, false);
            }
            else if (M != 0 && L != 0)
            {
                //up sampling by L
                L = L - 1;

                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    Up_List.Add(InputSignal.Samples[i]);
                    if (i == InputSignal.Samples.Count - 1)
                    { break; }
                    for (int j = 0; j < L; j++)
                    {
                        Up_List.Add(0);
                    }
                }
                //low pass filter
                FIR f = new FIR();
                f.InputTimeDomainSignal = new Signal(Up_List, false);
                f.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                f.InputFS = 8000;
                f.InputStopBandAttenuation = 50;
                f.InputCutOffFrequency = 1500;
                f.InputTransitionBand = 500;
                f.Run();
                OutputSignal = f.OutputYn;//////////
                //down sampling by M
                for (int i = 0; i < f.OutputYn.Samples.Count; i += M)
                {
                    if (f.OutputYn.Samples[i] != 0)
                    {
                        Down_List.Add(f.OutputYn.Samples[i]);
                    }
                }
                OutputSignal = new Signal(Down_List, false);
            }
            else
            {
                Console.WriteLine("ERROR_NO_VALUES \n");
            }
        }
    }

}