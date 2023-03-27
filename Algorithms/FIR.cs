
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.IO;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; } // Sampling_freq
        public float? InputCutOffFrequency { get; set; } //Fc
        public float? InputF1 { get; set; } // Band/Stop_pass
        public float? InputF2 { get; set; } // Band/Stop_pass
        public float InputStopBandAttenuation { get; set; } // Stop_Band_Attenuation
        public float InputTransitionBand { get; set; } // transition_width
        public Signal OutputHn { get; set; } // w(d) * h(d)
        public Signal OutputYn { get; set; } // Convolution (Hn , Input)

        public void save_signal()
        {
            StreamWriter file = new StreamWriter("../../../__coefficients.txt");
            file.WriteLine(0);

            if (OutputHn.Periodic)
                file.WriteLine(1);
            else
                file.WriteLine(0);

            file.WriteLine(OutputHn.SamplesIndices.Count);//n

            for (int i = 0; i < OutputHn.SamplesIndices.Count; ++i)//el samples
                file.WriteLine(OutputHn.SamplesIndices[i] + " " + OutputHn.Samples[i]);

            file.Flush();
            file.Close();
        }
        public int get_coff() //n
        {
            int N;
            float x = 0;

            if (InputStopBandAttenuation <= 21)//rectanguler 
                x = 0.9f; // get normalized transition width
            else if (InputStopBandAttenuation <= 44)//hanning
                x = 3.1f;
            else if (InputStopBandAttenuation <= 53)//haming
                x = 3.3f;
            else if (InputStopBandAttenuation <= 74)//black man
                x = 5.5f;

            N = (int)Math.Ceiling((x / (InputTransitionBand / InputFS))); // sematric
            if (N % 2 == 0) N++; //odd  

            return N;
        }
        public List<double> get_window(int N) //from el attinuation
        {
            int count = (N - 1) / 2;
            List<double> window = new List<double>();

            if (InputStopBandAttenuation <= 21)
            {
                for (int i = 0; i <= count; ++i)
                    window.Add(1);
            }
            else if (InputStopBandAttenuation <= 44)
            {
                for (int i = 0; i <= count; ++i)
                    window.Add(0.5 + (0.5 * Math.Cos((2 * Math.PI * i) / (double)(N))));
            }
            else if (InputStopBandAttenuation <= 53)
            {
                for (int i = 0; i <= count; ++i)
                    window.Add(0.54 + (0.46 * Math.Cos((2 * Math.PI * i) / (double)(N))));
            }
            else if (InputStopBandAttenuation <= 74)
            {
                for (int i = 0; i <= count; ++i)
                    window.Add(0.42 + (0.5 * Math.Cos((2 * Math.PI * i) / (double)(N - 1))) + (0.08 * Math.Cos((4 * Math.PI * i) / (double)(N - 1))));
            }
            return window;
        }
 

        // calculate filters
        /// <summary>
        ///  get impulse H_d
        /// </summary>

        public List<double> get_low_filter_imp(int N) // from hd
        {
            int count = (N - 1) / 2;
            List<double> impulse_response = new List<double>();

            float fc = calculate_fc();
            float w = (float)(2 * Math.PI * fc);

            impulse_response.Add(2 * fc);
            for (int i = 1; i <= count; ++i)
                impulse_response.Add(2 * fc * (Math.Sin(i * w) / (i * w)));
            return impulse_response;
        }
        public List<double> get_high_filter_imp(int N)
        {
            int count = (N - 1) / 2;
            List<double> impulse_response = new List<double>();

            float fc = calculate_fcs();
            float w = (float)(2 * Math.PI * fc);

            impulse_response.Add(1 - (2 * fc));
            for (int i = 1; i <= count; ++i)
                impulse_response.Add(-2 * fc * (Math.Sin(i * w) / (i * w)));
            return impulse_response;
        }
        public List<double> get_band_pass_filter_imp(int N)
        {
            List<double> impulse_response = new List<double>();
            int count = (N - 1) / 2;
            float f1 = get_BPF_f1();
            float w1 = (float)(2 * Math.PI * f1);
            float f2 = get_BPF_f2();
            float w2 = (float)(2 * Math.PI * f2);

            impulse_response.Add(2 * (f2 - f1));
            for (int i = 1; i <= count; ++i)
                impulse_response.Add((2 * f2 * ((Math.Sin(i * w2)) / (i * w2))) - (2 * f1 * ((Math.Sin(i * w1)) / (i * w1))));
            return impulse_response;
        }
        public List<double> get_band_stop_filter_imp(int N)
        {
            int count = (N - 1) / 2;
            float f1 = get_BSF_f1();
            float w1 = (float)(2 * Math.PI * f1);
            float f2 = get_BSF_f2();
            float w2 = (float)(2 * Math.PI * f2);

            List<double> impulse_response = new List<double>();

            impulse_response.Add(1 - (2 * (f2 - f1)));
            for (int i = 1; i <= count; ++i)
                impulse_response.Add((2 * f1 * (Math.Sin(i * w1) / (i * w1))) - (2 * f2 * (Math.Sin(i * w2) / (i * w2))));
            return impulse_response;
        }

        public float calculate_fc()//low
        {
            return (float)(InputCutOffFrequency + ((InputTransitionBand) / 2)) / InputFS;
        }
        public float calculate_fcs()//high
        {
            return (float)(InputCutOffFrequency - ((InputTransitionBand) / 2)) / InputFS;
        }
        public float get_BPF_f1()//band pass f1
        {
            return (float)(InputF1 - ((InputTransitionBand * 1000) / 2)) / (InputFS * 1000);
        }
        public float get_BPF_f2()
        {
            return (float)(InputF2 + ((InputTransitionBand * 1000) / 2)) / (InputFS * 1000);
        }
        public float get_BSF_f1()//band stop f1

        {
            return (float)(InputF1 + ((InputTransitionBand * 1000) / 2)) / (InputFS * 1000);
        }
        public float get_BSF_f2()
        {
            return (float)(InputF2 - ((InputTransitionBand * 1000) / 2)) / (InputFS * 1000);
        }

        public override void Run()
        {
            //throw new NotImplementedException();
            InputTransitionBand /= 1000;
            InputCutOffFrequency /= 1000;
            InputFS /= 1000;

            // filters
            int N = get_coff();
            // get desired ideal filter
            List<double> window = get_window(N);
            List<double> impulse_response = new List<double>();
            switch (InputFilterType)
            {
                case FILTER_TYPES.LOW:
                    impulse_response = get_low_filter_imp(N);
                    break;

                case FILTER_TYPES.HIGH:
                    impulse_response = get_high_filter_imp(N);
                    break;

                case FILTER_TYPES.BAND_STOP:
                    impulse_response = get_band_stop_filter_imp(N);
                    break;

                case FILTER_TYPES.BAND_PASS:
                    impulse_response = get_band_pass_filter_imp(N);
                    break;
            }
            //generate h(N)
            OutputHn = new Signal(new List<float>(), new List<int>(), InputTimeDomainSignal.Periodic);

            //-ve
            for (int i = 0; i < impulse_response.Count; ++i)
            {
                OutputHn.SamplesIndices.Add((impulse_response.Count - i - 1) * -1);
                OutputHn.Samples.Add((float)(impulse_response[impulse_response.Count - 1 - i] * window[impulse_response.Count - 1 - i]));
            }
            //+ve
            for (int i = 1; i < impulse_response.Count; ++i)
            {
                OutputHn.SamplesIndices.Add(i);
                OutputHn.Samples.Add((float)(impulse_response[i] * window[i]));
            }
            // convolution
            DirectConvolution convolution = new DirectConvolution();
            // end
            convolution.InputSignal1 = InputTimeDomainSignal;
            convolution.InputSignal2 = OutputHn;
            convolution.Run();
            OutputYn = convolution.OutputConvolvedSignal;
            save_signal();
        }
    }
}
/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.IO;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; } // Sampling freq
        public float? InputCutOffFrequency { get; set; } // low,high  Fc
        public float? InputF1 { get; set; } // Band/Stop(reject) pass
        public float? InputF2 { get; set; } // Band/Stop(reject) pass
        public float InputStopBandAttenuation { get; set; } // Stop_Band_Attenuation
        public float InputTransitionBand { get; set; } // transition_width
        public Signal OutputHn { get; set; } // el coofecient w(d) * h(d)
        public Signal OutputYn { get; set; } // Convolution (Hn , Input)

        public void save_coefficients()
        {
            StreamWriter streamSaver = new StreamWriter("../../../__coefficients.txt");
            streamSaver.WriteLine(0);

            if (OutputHn.Periodic)
                streamSaver.WriteLine(1);
            else
                streamSaver.WriteLine(0);

            streamSaver.WriteLine(OutputHn.SamplesIndices.Count);

            for (int i = 0; i < OutputHn.SamplesIndices.Count - 1; ++i)
                streamSaver.WriteLine(OutputHn.SamplesIndices[i] + " " + OutputHn.Samples[i]);

            streamSaver.Write(OutputHn.SamplesIndices[OutputHn.SamplesIndices.Count - 1]
                + " " + OutputHn.Samples[OutputHn.SamplesIndices.Count - 1]);

            streamSaver.Flush();
            streamSaver.Close();
        }

        public List<double> get_window(int N)
        {
            int count = (N - 1) / 2;
            List<double> window = new List<double>();
            if (InputStopBandAttenuation <= 21)
            {
                for (int i = 0; i <= count; ++i)
                    window.Add(1);
            }
            else if (InputStopBandAttenuation <= 44)
            {
                for (int i = 0; i <= count; ++i)
                    window.Add(0.5 + (0.5 * Math.Cos((2 * Math.PI * i) / (double)(N))));
            }
            else if (InputStopBandAttenuation <= 53)
            {
                for (int i = 0; i <= count; ++i)
                    window.Add(0.54 + (0.46 * Math.Cos((2 * Math.PI * i) / (double)(N))));
            }
            else if (InputStopBandAttenuation <= 74)
            {
                for (int i = 0; i <= count; ++i)
                    window.Add(0.42 + (0.5 * Math.Cos((2 * Math.PI * i) / (double)(N - 1))) + (0.08 * Math.Cos((4 * Math.PI * i) / (double)(N - 1))));
            }
            return window;
        }
        public int calculate_N()
        {
            int N = -1;
            if (InputStopBandAttenuation <= 21)
                N = (int)Math.Ceiling((0.9 / (InputTransitionBand / InputFS)));
            else if (InputStopBandAttenuation <= 44)
                N = (int)Math.Ceiling((3.1 / (InputTransitionBand / InputFS)));
            else if (InputStopBandAttenuation <= 53)
                N = (int)Math.Ceiling((3.3 / (InputTransitionBand / InputFS)));
            else if (InputStopBandAttenuation <= 74)
                N = (int)Math.Ceiling((5.5 / (InputTransitionBand / InputFS)));

            if (N % 2 == 0) N++;

            return N;
        }
    
        // calculate filters 
        public List<double> get_impulse_response_low_filter(int N)
        {
            int count = (N - 1) / 2;
            List<double> impulse_response = new List<double>();
            float fc = calculate_fc();
            float w = (float)(2 * Math.PI * fc);
            for (int i = 0; i <= count; ++i)
            {
                if (i == 0)
                    impulse_response.Add(2 * fc);
                else
                    impulse_response.Add(2 * fc * (Math.Sin(i * w) / (i * w)));
            }
            return impulse_response;
        }

        public List<double> get_impulse_response_high_filter(int N)
        {
            int count = (N - 1) / 2;
            List<double> impulse_response = new List<double>();
            float fc = calculate_fcs();
            float w = (float)(2 * Math.PI * fc);

            for (int i = 0; i <= count; ++i)
            {
                if (i == 0)
                    impulse_response.Add(1 - (2 * fc));
                else
                    impulse_response.Add(-2 * fc * (Math.Sin(i * w) / (i * w)));
            }
            return impulse_response;
        }
        public List<double> bandpass(int N)
        {
            List<double> impulse_response = new List<double>();
            int count = (N - 1) / 2;
            float f1 = clcbandpass_f1();
            float w1 = (float)(2 * Math.PI * f1);
            float f2 = clcbandpass_f2();
            float w2 = (float)(2 * Math.PI * f2);

            for (int i = 0; i <= count; ++i)
            {
                if (i == 0)
                    impulse_response.Add(2 * (f2 - f1));
                else
                    impulse_response.Add((2 * f2 * ((Math.Sin(i * w2)) / (i * w2))) - (2 * f1 * ((Math.Sin(i * w1)) / (i * w1))));
            }
            return impulse_response;
        }
        public List<double> bandstop(int N)
        {
            int count = (N - 1) / 2;
            float f1 = clcbandstop_f1();
            float w1 = (float)(2 * Math.PI * f1);
            float f2 = clcbandstop_f2();
            float w2 = (float)(2 * Math.PI * f2);

            List<double> impulse_response = new List<double>();

            for (int i = 0; i <= count; ++i)
            {
                if (i == 0)
                    impulse_response.Add(1 - (2 * (f2 - f1)));
                else
                    impulse_response.Add((2 * f1 * (Math.Sin(i * w1) / (i * w1))) - (2 * f2 * (Math.Sin(i * w2) / (i * w2))));
            }
            return impulse_response;
        }
        public float calculate_fc()
        {
            float fc = (float)(InputCutOffFrequency + ((InputTransitionBand) / 2));
            return fc / InputFS;
        }
        public float calculate_fcs()
        {
            float fc = (float)(InputCutOffFrequency - ((InputTransitionBand) / 2));
            return fc / InputFS;
        }
        public float clcbandpass_f1()
        {
            float f1 = (float)(InputF1 - ((InputTransitionBand * 1000) / 2));
            return f1 / (InputFS * 1000);
        }
        public float clcbandpass_f2()
        {
            float f2 = (float)(InputF2 + ((InputTransitionBand * 1000) / 2));
            return f2 / (InputFS * 1000);
        }
        public float clcbandstop_f1()
        {
            float f1 = (float)(InputF1 + ((InputTransitionBand * 1000) / 2));
            return f1 / (InputFS * 1000);
        }
        public float clcbandstop_f2()
        {
            float f2 = (float)(InputF2 - ((InputTransitionBand * 1000) / 2));
            return f2 / (InputFS * 1000);
        }

        public override void Run()
        {
            //throw new NotImplementedException();
            InputTransitionBand /= 1000;
            InputCutOffFrequency /= 1000;
            InputFS /= 1000;

            // filters
            int N = calculate_N();
            List<double> window = get_window(N);
            List<double> impulse_response = new List<double>();
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                impulse_response = get_impulse_response_low_filter(N);
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                impulse_response = get_impulse_response_high_filter(N);
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                impulse_response = bandpass(N);
            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                impulse_response = bandstop(N);
            }

            OutputHn = new Signal(new List<float>(), new List<int>(), InputTimeDomainSignal.Periodic);

            for (int i = 0; i < impulse_response.Count; ++i)
            {
                OutputHn.SamplesIndices.Add((impulse_response.Count - i - 1) * -1);
                OutputHn.Samples.Add((float)(impulse_response[impulse_response.Count - 1 - i] * window[impulse_response.Count - 1 - i]));
            }

            for (int i = 1; i < impulse_response.Count; ++i)
            {
                OutputHn.SamplesIndices.Add(i);
                OutputHn.Samples.Add((float)(impulse_response[i] * window[i]));
            }

            DirectConvolution convolution = new DirectConvolution();
            convolution.InputSignal1 = InputTimeDomainSignal;
            convolution.InputSignal2 = OutputHn;
            convolution.Run();
            OutputYn = convolution.OutputConvolvedSignal;
            save_coefficients();
        }
    }
}
*/
