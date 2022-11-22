using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionA.Tuner.Decoder.FastFourierTransform
{
    public struct FrequencyPoint
    {
        private double _frequency;
        public double Frequency => _frequency;
        private double _magnitude;
        public double Magnitude => _magnitude;
        private double _phase;
        public double Phase => _phase;
        private ComplexNumber _complexNumber;
        public ComplexNumber ComplexNumber => _complexNumber;

        public FrequencyPoint(double frequency, double magnitude, double phase, ComplexNumber complexNumber)
        {
            _frequency = frequency;
            _magnitude = magnitude;
            _phase = phase;
            _complexNumber = complexNumber;
        }

        public FrequencyPoint(double frequency, double magnitude, double phase, double real, double imaginary) : this(frequency, magnitude, phase, new ComplexNumber(real, imaginary))
        {
        }
    }
}
