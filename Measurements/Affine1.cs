using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Numerics;
using System.Net.NetworkInformation;
using System.Measurements;

namespace System.Measurements {
    public interface ILinear1<TMeasurement>
      where TMeasurement : struct, IMeasurement<TMeasurement> {
        public double X { get; }
        IPhysicalUnit<TMeasurement> GetUnit();
    }

    public interface IAffine1<TMeasurement>
      : ILinear1<TMeasurement>
      where TMeasurement : struct, IMeasurement<TMeasurement> {
    }

    public interface IDiff1<TMeasurement>
      : ILinear1<TMeasurement>
      where TMeasurement : struct, IMeasurement<TMeasurement> {
    }    

    public static class Linear {
        public static Diff1<TUnit, TMeasurement> ConvertTo<TUnit, TMeasurement>(this IDiff1<TMeasurement> diff)
          where TMeasurement : struct, IMeasurement<TMeasurement>
          where TUnit : struct, IPhysicalUnit<TUnit, TMeasurement> =>
            new Diff1<TUnit, TMeasurement>(diff.GetUnit().GetConversion(default(TUnit)).Transform(diff.X));

        public static Affine1<TUnit, TMeasurement> ConvertTo<TUnit, TMeasurement>(this IAffine1<TMeasurement> affine)
          where TMeasurement : struct, IMeasurement<TMeasurement>
          where TUnit : struct, IPhysicalUnit<TUnit, TMeasurement> =>
            new Affine1<TUnit, TMeasurement>(affine.GetUnit().GetConversion(default(TUnit)).Transform(affine.X));

        public static Affine1<TTargetUnit, TMeasurement>[] Convert<TTargetUnit, TSourceUnit, TMeasurement>(params Affine1<TSourceUnit, TMeasurement>[] affines)
          where TMeasurement : struct, IMeasurement<TMeasurement>
          where TSourceUnit : struct, IPhysicalUnit<TSourceUnit, TMeasurement>
          where TTargetUnit : struct, IPhysicalUnit<TTargetUnit, TMeasurement> {
            var transform = default(TSourceUnit).GetConversion(default(TTargetUnit));
            var result = new Affine1<TTargetUnit, TMeasurement>[affines.Length];
            Parallel.For(0, affines.Length - 1, i =>
                result[i] = new Affine1<TTargetUnit, TMeasurement>(transform.Transform(affines[i].X)));
            return result;
        }
    }

    public struct Affine1<TUnit, TMeasurement>
      : IAffine1<TMeasurement>
      where TMeasurement : struct, IMeasurement<TMeasurement>
      where TUnit : struct, IPhysicalUnit<TUnit, TMeasurement> {
        public double X { get; }
        private readonly Lazy<Diff1<TUnit, TMeasurement>> _magnitude;
        private readonly Lazy<Diff1<TUnit, TMeasurement>> _squareMagnitude;

        public static Affine1<TUnit, TMeasurement> Zero => default;
        public IPhysicalUnit<TMeasurement> GetUnit() => default(TUnit);

        public Affine1(double x) {
            X = x;

            _squareMagnitude = new Lazy<Diff1<TUnit, TMeasurement>>(() => new Diff1<TUnit, TMeasurement>(x * x));
            _magnitude = new Lazy<Diff1<TUnit, TMeasurement>>(() => new Diff1<TUnit, TMeasurement>(x < 0 ? -x : x));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Affine1<TTargetUnits, TMeasurement> ConvertTo<TTargetUnits>()
          where TTargetUnits : struct, IPhysicalUnit<TTargetUnits, TMeasurement> =>
            new Affine1<TTargetUnits, TMeasurement>(default(TUnit).GetConversion(default(TTargetUnits)).Transform(X));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Diff1<TUnit, TMeasurement> operator -(Affine1<TUnit, TMeasurement> first, Affine1<TUnit, TMeasurement> second) =>
            new Diff1<TUnit, TMeasurement>(first.X - second.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Diff1<TUnit, TMeasurement> operator -(Affine1<TUnit, TMeasurement> first, IAffine1<TMeasurement> second) =>
            new Diff1<TUnit, TMeasurement>(first.X - second.GetUnit().GetConversion(first.GetUnit()).Transform(second.X));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine1<TUnit, TMeasurement> operator -(Affine1<TUnit, TMeasurement> first, Diff1<TUnit, TMeasurement> second) =>
            new Affine1<TUnit, TMeasurement>(first.X - second.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine1<TUnit, TMeasurement> operator -(Affine1<TUnit, TMeasurement> first, IDiff1<TMeasurement> second) =>
            new Affine1<TUnit, TMeasurement>(first.X - second.GetUnit().GetConversion(first.GetUnit()).Transform(second.X));



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Diff1<TUnit, TMeasurement> operator +(Affine1<TUnit, TMeasurement> first, Diff1<TUnit, TMeasurement> second) =>
            new Diff1<TUnit, TMeasurement>(first.X + second.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Diff1<TUnit, TMeasurement> operator +(Affine1<TUnit, TMeasurement> first, IDiff1<TMeasurement> second) =>
            new Diff1<TUnit, TMeasurement>(first.X + second.GetUnit().GetConversion(first.GetUnit()).Transform(second.X));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Diff1<TUnit, TMeasurement> operator +(Diff1<TUnit, TMeasurement> first, Affine1<TUnit, TMeasurement> second) =>
            new Diff1<TUnit, TMeasurement>(first.X + second.X);        

        public Diff1<TUnit, TMeasurement> Norm => _magnitude.Value;
    }

    //A difference space, components of which are vectors created by subtraction of one affine point by another
    public struct Diff1<TUnit, TMeasurement>
      : IDiff1<TMeasurement>
      where TMeasurement : struct, IMeasurement<TMeasurement>
      where TUnit : struct, IPhysicalUnit<TUnit, TMeasurement> {
        public double X { get; }
        public IPhysicalUnit<TMeasurement> GetUnit() => default(TUnit);

        public Diff1(double x) {
            X = x;
            _squareMagnitude = new Lazy<Diff1<TUnit, TMeasurement>>(() => new Diff1<TUnit, TMeasurement>(x * x));
            _magnitude = new Lazy<Diff1<TUnit, TMeasurement>>(() => new Diff1<TUnit, TMeasurement>(x < 0 ? -x : x));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Diff1<TTargetUnits, TMeasurement> ConvertTo<TTargetUnits>(IPhysicalUnit<TTargetUnits, TMeasurement> targetUnits)
          where TTargetUnits : struct, IPhysicalUnit<TTargetUnits, TMeasurement> =>
            new Diff1<TTargetUnits, TMeasurement>(GetUnit().GetConversion(targetUnits).Transform(X));

        public static Diff1<TUnit, TMeasurement> operator +(Diff1<TUnit, TMeasurement> first, Diff1<TUnit, TMeasurement> second) =>
            new Diff1<TUnit, TMeasurement>(first.X + second.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Diff1<TUnit, TMeasurement> operator +(Diff1<TUnit, TMeasurement> first, IDiff1<TMeasurement> second) =>
            new Diff1<TUnit, TMeasurement>(first.X + second.ConvertTo<TUnit, TMeasurement>().X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Diff1<TUnit, TMeasurement> operator -(Diff1<TUnit, TMeasurement> first, Diff1<TUnit, TMeasurement> second) =>
            new Diff1<TUnit, TMeasurement>(first.X - second.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Diff1<TUnit, TMeasurement> operator -(Diff1<TUnit, TMeasurement> first, IDiff1<TMeasurement> second) =>
            new Diff1<TUnit, TMeasurement>(first.X - second.ConvertTo<TUnit, TMeasurement>().X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Diff1<TUnit, TMeasurement> operator +(Diff1<TUnit, TMeasurement> first, Affine1<TUnit, TMeasurement> second) =>
            new Diff1<TUnit, TMeasurement>(first.X + second.X);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Diff1<TUnit, TMeasurement> operator +(Diff1<TUnit, TMeasurement> first, IAffine1<TMeasurement> second) =>
            new Diff1<TUnit, TMeasurement>(first.X + second.ConvertTo<TUnit, TMeasurement>().X);

        public static Diff1<TUnit, TMeasurement> operator *(double scalar, Diff1<TUnit, TMeasurement> vector) =>
            new Diff1<TUnit, TMeasurement>(scalar * vector.X);

        public static Diff1<TUnit, TMeasurement> operator *(Diff1<TUnit, TMeasurement> vector, double scalar) =>
            new Diff1<TUnit, TMeasurement>(vector.X * scalar);

        public static Diff1<TUnit, TMeasurement> operator /(Diff1<TUnit, TMeasurement> vector, double scalar) =>
            new Diff1<TUnit, TMeasurement>(vector.X / scalar);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double Square(double value) => value * value;

        public static Diff1<TUnit, TMeasurement> operator &(Diff1<TUnit, TMeasurement> first, Diff1<TUnit, TMeasurement> second) =>
            new Diff1<TUnit, TMeasurement>(Square(second.X - first.X));

        private readonly Lazy<Diff1<TUnit, TMeasurement>> _magnitude;
        public Diff1<TUnit, TMeasurement> Magnitude => _magnitude.Value;

        private readonly Lazy<Diff1<TUnit, TMeasurement>> _squareMagnitude;
        public Diff1<TUnit, TMeasurement> SquareMagnitude => _squareMagnitude.Value;
    }
}
