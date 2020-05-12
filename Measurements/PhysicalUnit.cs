using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;

namespace System.Measurements {
    public interface IPhysicalUnit<TMeasurement>
      where TMeasurement : struct, IMeasurement<TMeasurement> {
        public string Name { get; }
        public string Plural { get; }
        public LinearFractionalTransformation ToBase => FromBase.Inverse;
        public LinearFractionalTransformation FromBase => ToBase.Inverse;
        public TMeasurement Measurement { get; }
    }

    public interface IPhysicalUnit<TUnit, TMeasurement>
      : IPhysicalUnit<TMeasurement>
      , IEquatable<TUnit>
      where TMeasurement : struct, IMeasurement<TMeasurement>
      where TUnit : struct, IPhysicalUnit<TUnit, TMeasurement> {
        bool IEquatable<TUnit>.Equals(TUnit other) =>
            Name == other.Name;
    }

    public static class PhysicalUnit {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LinearFractionalTransformation GetConversion<TMeasurement>(this IPhysicalUnit<TMeasurement> from, IPhysicalUnit<TMeasurement> to)
          where TMeasurement : struct, IMeasurement<TMeasurement> =>
            from.ToBase.ComposeWith(to.FromBase);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IPhysicalUnit<TMeasurement> GetBaseUnit<TMeasurement>(this IPhysicalUnit<TMeasurement> units)
          where TMeasurement : struct, IMeasurement<TMeasurement> =>
            units.Measurement.GetBaseUnit();
    }
}
