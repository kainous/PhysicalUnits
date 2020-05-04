using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace System.Measurements {
    public sealed partial class Dimension {
        private readonly IDictionary<char, Dimension> _bySymbol =
            new Dictionary<char, Dimension>();

        private readonly IDictionary<string, Dimension> _byTextID =
            new Dictionary<string, Dimension>(StringComparer.OrdinalIgnoreCase);

        public string TextID { get; }
        public char Symbol { get; }

        internal Dimension(string textID, char symbol) {
            symbol = char.ToUpper(symbol);
            if (!char.IsLetterOrDigit(symbol)) {
                throw new ArgumentException("Symbol must be a character", nameof(symbol));
            }

            if (string.IsNullOrWhiteSpace(textID)) {
                throw new ArgumentException("Text ID must contain data", nameof(TextID));
            }

            TextID = textID;
            Symbol = symbol;

            _bySymbol.Add(symbol, this);
            _byTextID.Add(textID, this);
        }
    }

    public abstract class Measurement<TMeasurement> where TMeasurement : Measurement<TMeasurement> {
        protected internal Measurement() { }
        public abstract string Name { get; }
        public abstract IReadOnlyDictionary<char, int> Dimensions { get; }
        public abstract PhysicalUnit<TMeasurement> BaseUnit { get; }
    }

    public abstract class PhysicalUnit<TMeasurement>
      where TMeasurement : Measurement<TMeasurement> {
        protected internal PhysicalUnit() { }
        public abstract string Name { get; }
        public abstract string Plural { get; }
        public abstract LinearFractionalTransformation ToBase { get; }
        public abstract LinearFractionalTransformation FromBase { get; }
    }

    public struct LinearFractionalTransformation : IEquatable<LinearFractionalTransformation> {
        public double A { get; }
        public double B { get; }
        public double C { get; }
        public double D { get; }

        public double Transform(double value) =>
            (A * value + B) / (C * value + D);

        public LinearFractionalTransformation(double a, double b, double c, double d) {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public static LinearFractionalTransformation Identity =>
            new LinearFractionalTransformation(1.0, 0.0, 0.0, 1.0);

        public LinearFractionalTransformation Inverse =>
            new LinearFractionalTransformation(D, -B, -C, A);       

        public static bool Equals(LinearFractionalTransformation first, LinearFractionalTransformation second, double epsilon = double.Epsilon) =>
            Math.Abs(first.A * second.C - second.A * first.C) <= epsilon
         && Math.Abs(first.A * second.D + first.B * second.C - second.A * first.D - second.B * first.C) <= epsilon
         && Math.Abs(first.B * second.D - second.B * first.D) <= epsilon;

        public static bool operator ==(LinearFractionalTransformation first, LinearFractionalTransformation second) => 
          ( first.A == second.A
         && first.B == second.B
         && first.C == second.C
         && first.D == second.D)
         || Equals(first, second);

        public static bool operator !=(LinearFractionalTransformation first, LinearFractionalTransformation second) =>
          ( first.A != second.A
         || first.B != second.B
         || first.C != second.C
         || first.D != second.D)
         && !Equals(first, second);

        public bool Equals(LinearFractionalTransformation other, double epsilon) => 
            Equals(this, other, epsilon);

        bool IEquatable<LinearFractionalTransformation>.Equals(LinearFractionalTransformation other) =>
            Equals(this, other);

        public bool Equals(object obj, double epsilon) =>
            obj is LinearFractionalTransformation other && Equals(other, epsilon);

        public override bool Equals(object obj) =>
            Equals(obj, double.Epsilon);

        public override int GetHashCode() => 
            HashCode.Combine(A, B, C, D);
    }
}
