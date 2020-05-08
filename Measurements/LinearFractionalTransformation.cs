using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace System.Measurements {
    public struct Matrix2x2 {
        private readonly double _a1_1;
        private readonly double _a1_2;
        private readonly double _a2_1;
        private readonly double _a2_2;

        public Matrix2x2(double a11, double a12, double a21, double a22) {
            _a1_1 = a11;
            _a1_2 = a12;
            _a2_1 = a21;
            _a2_2 = a22;
        }

        public double this[int i, int j] =>
            (i, j) switch
            {
                (0, 0) => _a1_1,
                (0, 1) => _a1_2,
                (1, 0) => _a2_1,
                (1, 1) => _a2_2,
                _ => throw new IndexOutOfRangeException("SquareMatrix2 can only handle indices between 0 and 1")
            };

        public static Matrix2x2 operator *(Matrix2x2 A, Matrix2x2 B) =>
            new Matrix2x2(
                A._a1_1 * B._a1_1 + A._a1_2 * B._a2_1,
                A._a1_1 * B._a1_2 + A._a1_2 * B._a2_2,
                A._a2_1 * B._a1_1 + A._a2_2 * B._a2_1,
                A._a2_1 * B._a1_2 + A._a2_2 * B._a2_2);

        public static Matrix2x2 Multiply(params Matrix2x2[] matrices) => 
            matrices.Aggregate((a, b) => a * b);

        public static Matrix2x2 Multiply(IEnumerable<Matrix2x2> matrices) =>
            matrices.Aggregate((a, b) => a * b);
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

            _normalized1 = new Lazy<LinearFractionalTransformation>(() => {
                // Inverse of the determinant of the matrix
                var iDet = 1.0 / (a * d - b * c);
                return new LinearFractionalTransformation(a * iDet, b * iDet, c * iDet, d * iDet);
            });
        }

        public LinearFractionalTransformation(Matrix2x2 matrix)
            : this(matrix[0, 0], matrix[0, 1], matrix[1, 0], matrix[1, 1]) {
        }

        public LinearFractionalTransformation(double a, double b)
            : this(a, b, 0.0, 1.0) {
        }

        public LinearFractionalTransformation(double a)
            : this(a, 0.0, 0.0, 1.0) {
        }

        public Matrix2x2 AsSquareMatrix2() =>
            new Matrix2x2(A, B, C, D);

        public static LinearFractionalTransformation Compose(params LinearFractionalTransformation[] transformations) => 
            new LinearFractionalTransformation(Matrix2x2.Multiply(transformations.Select(o => o.AsSquareMatrix2())));

        public LinearFractionalTransformation ComposeWith(LinearFractionalTransformation transformation) => 
            Compose(this, transformation);

        public static LinearFractionalTransformation Identity =>
            new LinearFractionalTransformation(1.0, 0.0, 0.0, 1.0);

        public LinearFractionalTransformation Inverse =>
            new LinearFractionalTransformation(D, -B, -C, A);

        // For now, this is normalized by determinant, not by fixpoints
        private readonly Lazy<LinearFractionalTransformation> _normalized1;
        public LinearFractionalTransformation Normalized => _normalized1.Value;

        public static bool Equals(LinearFractionalTransformation first, LinearFractionalTransformation second, double epsilon = double.Epsilon) =>
            Math.Abs(first.A * second.C - second.A * first.C) <= epsilon
         && Math.Abs(first.A * second.D + first.B * second.C - second.A * first.D - second.B * first.C) <= epsilon
         && Math.Abs(first.B * second.D - second.B * first.D) <= epsilon;

        public static bool operator ==(LinearFractionalTransformation first, LinearFractionalTransformation second) =>
          (first.A == second.A
         && first.B == second.B
         && first.C == second.C
         && first.D == second.D)
         || Equals(first, second);

        public static bool operator !=(LinearFractionalTransformation first, LinearFractionalTransformation second) =>
          (first.A != second.A
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
