using System.Diagnostics.CodeAnalysis;

namespace OptionA.Tuner.Decoder.FastFourierTransform
{
    public struct ComplexNumber
    {
        /// <summary>
        /// Returns a new empty (invalid) complex number
        /// </summary>
        public static readonly ComplexNumber Empty = new(double.NaN, double.NaN);
        /// <summary>
        /// Returns a new complex number with both the real and imaginary parts set to 0
        /// </summary>
        public static readonly ComplexNumber Zero = new(0, 0);

        private double _real;
        public double Real => _real;

        private double _imaginary;
        public double Imaginary => _imaginary;

        public ComplexNumber(double real, double imaginary)
        {
            _real = real;
            _imaginary = imaginary;
        }

        public void SetReal(double real)
        {
            _real = real;
        }

        public void SetImaginary(double imaginary)
        {
            _imaginary = imaginary;
        }

        public bool IsEmpty()
        {
            return this == ComplexNumber.Empty;
        }

        /// <summary>
        /// Gets the Conjugate of the Complex number
        /// </summary>
        /// <returns>The Conjugate of the Complex number</returns>
        public ComplexNumber GetConjugate()
        {
            return new ComplexNumber(_real, -_imaginary);
        }
        /// <summary>
        /// Gets the Magnitude of the Complex Number
        /// </summary>
        /// <returns>The Magnitude of the Complex Number</returns>
        public double GetMagnitude()
        {
            return Math.Sqrt((_real * _real) + (_imaginary * _imaginary));
        }
        /// <summary>
        /// Gets the Phase of the Complex Number
        /// </summary>
        /// <returns>The Phase of the Complex Number</returns>
        public double GetPhase()
        {
            return Math.Atan2(_imaginary, _real);
        }
        /// <summary>
        /// Gets the Sin of the Complex Number
        /// </summary>
        /// <returns>The Sin of the Complex Number</returns>
        public ComplexNumber Sin()
        {
            return new ComplexNumber(Math.Sin(_real) * Math.Cosh(_imaginary), Math.Cos(_real) * Math.Sinh(_imaginary));
        }
        /// <summary>
        /// Gets the Cos of the Complex Number
        /// </summary>
        /// <returns>The Cos of the Complex Number</returns>
        public ComplexNumber Cos()
        {
            return new ComplexNumber(Math.Cos(_real) * Math.Cosh(_imaginary), -(Math.Sin(_real) * Math.Sinh(_imaginary)));
        }
        /// <summary>
        /// Gets the Tan of the Complex Number
        /// </summary>
        /// <returns>The Tan of the Complex Number</returns>
        public ComplexNumber Tan()
        {
            return Sin() / Cos();
        }
        /// <summary>
        /// Gets the hyperbolic Sin of the Complex Number
        /// </summary>
        /// <returns>The hyperbolic Sin of the Complex Number</returns>
        public ComplexNumber Sinh()
        {
            return new ComplexNumber(Math.Sinh(_real) * Math.Cos(_imaginary), Math.Cosh(_real) * Math.Sin(_imaginary));
        }
        /// <summary>
        /// Gets the hyperbolic Cos of the Complex Number
        /// </summary>
        /// <returns>The hyperbolic Cos of the Complex Number</returns>
        public ComplexNumber Cosh()
        {
            return new ComplexNumber(Math.Cosh(_real) * Math.Cos(_imaginary), Math.Sinh(_real) * Math.Sin(_imaginary));
        }
        /// <summary>
        /// Gets the hyperbolic Tan of the Complex Number
        /// </summary>
        /// <returns>The hyperbolic Tan of the Complex Number</returns>
        public ComplexNumber Tanh()
        {
            return Sinh() / Cosh();
        }
        /// <summary>
        /// Calculates the exponential of the Complex Number
        /// </summary>
        /// <returns></returns>
        public ComplexNumber Exp()
        {
            return new ComplexNumber(Math.Exp(_real) * Math.Cos(_imaginary), Math.Exp(_real) * Math.Sin(_imaginary));
        }

        /// <summary>
        /// Returns true if the Complex Numbers are the same
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is ComplexNumber b && 
                this == b;            
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _real.GetHashCode() + _imaginary.GetHashCode();
        }

        /// <summary>
        /// Returns true if the Complex Numbers are the same
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(ComplexNumber a, ComplexNumber b)
        {
            var realMatch = (double.IsNaN(a._real) && double.IsNaN(b._real)) ||
                (Math.Abs(a._real) - Math.Abs(b._real) < double.Epsilon);
            var imaginaryMatch = (double.IsNaN(a._imaginary) && double.IsNaN(b._imaginary)) ||
                (Math.Abs(a._imaginary) - Math.Abs(b._imaginary) < double.Epsilon);

            return realMatch && imaginaryMatch;
        }

        /// <summary>
        /// Returns true if the Complex Numbers are not the same
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(ComplexNumber a, ComplexNumber b)
        {
            var realMatch = (double.IsNaN(a._real) && double.IsNaN(b._real)) ||
                (Math.Abs(a._real) - Math.Abs(b._real) < double.Epsilon);
            var imaginaryMatch = (double.IsNaN(a._imaginary) && double.IsNaN(b._imaginary)) ||
                (Math.Abs(a._imaginary) - Math.Abs(b._imaginary) < double.Epsilon);

            return !realMatch || !imaginaryMatch;
        }

        /// <summary>
        /// Adds 2 Complex Numbers together
        /// </summary>
        /// <param name="a">The first Complex Number</param>
        /// <param name="b">The second Complex Number</param>
        /// <returns>The sum of the two Complex Numbers</returns>
        public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
        {
            double real = a._real + b._real;
            double imag = a._imaginary + b._imaginary;
            return new ComplexNumber(real, imag);
        }
        /// <summary>
        /// Subtracts two Complex Numbers
        /// </summary>
        /// <param name="a">The first Complex Number</param>
        /// <param name="b">The second Complex Number</param>
        /// <returns></returns>
        public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
        {
            return a + -b;
        }
        /// <summary>
        /// Multiplies two Complex Numbers together
        /// </summary>
        /// <param name="a">The first Complex Number</param>
        /// <param name="b">The second Complex Number</param>
        /// <returns>The product of the two Complex Numbers</returns>
        public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
        {
            double real = (a._real * b._real) - (a._imaginary * b._imaginary);
            double imag = (a._real * b._imaginary) + (a._imaginary * b._real);
            return new ComplexNumber(real, imag);
        }
        /// <summary>
        /// Divides two Complex Numbers
        /// </summary>
        /// <param name="a">The first Complex Number</param>
        /// <param name="b">The second Complex Number</param>
        /// <returns>The quotient of the two Complex Numbers</returns>
        public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
        {
            ComplexNumber conj = b.GetConjugate();
            ComplexNumber top = a * conj;
            ComplexNumber bottom = b * conj;
            if (bottom._real != 0.0 && bottom._imaginary == 0.0)
                return new ComplexNumber(top._real / bottom._real, top._imaginary / bottom._real);
            else
                return new ComplexNumber(double.NaN, double.NaN);
        }
        /// <summary>
        /// Divides a Complex Number by a double scalar
        /// </summary>
        /// <param name="a">The Complex Number</param>
        /// <param name="b">The scalar</param>
        /// <returns>The quotient of the two</returns>
        public static ComplexNumber operator /(ComplexNumber a, double b)
        {
            return new ComplexNumber(a._real / b, a._imaginary / b);
        }
        /// <summary>
        /// Negates a Complex Number
        /// </summary>
        /// <param name="a">The Complex Number</param>
        /// <returns>The negated Complex Number</returns>
        public static ComplexNumber operator -(ComplexNumber a)
        {
            return new ComplexNumber(-a._real, -a._imaginary);
        }
        /// <summary>
        /// Allows a double to be cast to a ComplexNumber
        /// </summary>
        /// <param name="d">The double to cast</param>
        public static implicit operator ComplexNumber(double d)
        {
            return new ComplexNumber(d, 0.0);
        }
        /// <summary>
        /// Allows a Complex Number to be cast to an int, only taking the real
        /// </summary>
        /// <param name="n">The Complex Number to be cast</param>
        public static explicit operator int(ComplexNumber n)
        {
            return (int)n._real;
        }

        /// <summary>
        /// Allows a ComplexNumber to be cast to a double
        /// </summary>
        /// <param name="value">The ComplexNumber to cast</param>
        public static explicit operator double(ComplexNumber value)
        {
            return value._real;
        }
    }
}
