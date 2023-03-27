
using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public void save_signal(Signal signal, string path, bool time_freq = true)
        {
            StreamWriter file = new StreamWriter(path);
            if (time_freq)
                file.WriteLine(0);
            else
                file.WriteLine(1);

            if (signal.Periodic)
                file.WriteLine(1);
            else
                file.WriteLine(0);

            file.WriteLine(signal.SamplesIndices.Count);//n
            if (time_freq)
            {
                for (int i = 0; i < signal.SamplesIndices.Count; ++i)//el samples
                    file.WriteLine(signal.SamplesIndices[i] + " " + signal.Samples[i]);
            }
            else
            {
                for (int i = 0; i < signal.Frequencies.Count; ++i)//el samples
                    file.WriteLine(signal.Frequencies[i] + " " + signal.FrequenciesAmplitudes[i]
                        + " " + signal.FrequenciesPhaseShifts[i]);
            }


            file.Flush();
            file.Close();
        }

        public override void Run()
        {  

            FIR fir = new FIR();
            Sampling s = new Sampling();
            DC_Component dc = new DC_Component();
            Normalizer no = new Normalizer();
            DiscreteFourierTransform dft = new DiscreteFourierTransform();

            Signal InputSignal = LoadSignal(SignalPath);

            //fir 
            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;
            fir.InputF1 = miniF;
            fir.InputF2 = maxF;
            fir.InputFS = Fs;
            fir.InputFilterType = FILTER_TYPES.BAND_PASS;
            fir.InputTimeDomainSignal = InputSignal;
            fir.Run();
       
            //sampling or  not
            if (newFs >= 2 * maxF)
            {
                //if sampling true take sampling output into dc

                s.L = L;
                s.M = M;
                s.InputSignal = fir.OutputYn;
                s.Run();
                dc.InputSignal = s.OutputSignal;
            }
            //if sampling false take fir ouput into dc

            else
                dc.InputSignal = fir.OutputYn;

            dc.Run();
           
            //normalize dc out
            no.InputSignal = dc.OutputSignal;
            no.InputMinRange = -1;
            no.InputMaxRange = 1;
            no.Run();

            //dft
            dft.InputTimeDomainSignal = no.OutputNormalizedSignal;
            dft.InputSamplingFrequency = Fs;
            dft.Run();
         
            OutputFreqDomainSignal = dft.OutputFreqDomainSignal;
        }

      
        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}