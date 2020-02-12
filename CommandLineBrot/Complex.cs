using System;
using System.Collections.Generic;
using System.Text;

namespace CommandLineBrot
{
    public struct Complex
    {
        public double Real { get; set; }

        public double Imaginary { get; set; }

        public double SquaredMagnitude => ((Real * Real) + (Imaginary * Imaginary));

        public Complex(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public static Complex operator +(Complex left, Complex right) => new Complex(left.Real + right.Real, left.Imaginary + right.Imaginary);

        public static Complex operator *(Complex left, Complex right) => new Complex((left.Real * right.Real) - (left.Imaginary * right.Imaginary), (left.Real * right.Imaginary) + (left.Imaginary * right.Real));
    }
}
