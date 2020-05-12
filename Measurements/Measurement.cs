using System.Collections.Generic;

namespace System.Measurements {
    public interface IMeasurement<TMeasurement>
      : IEquatable<TMeasurement>
      where TMeasurement : struct, IMeasurement<TMeasurement> {
        string Name { get; }
        IReadOnlyDictionary<char, int> Dimensions { get; }
        IPhysicalUnit<TMeasurement> GetBaseUnit();
        bool IEquatable<TMeasurement>.Equals(TMeasurement other) =>
            Name == other.Name;
    }

    //public struct Measurement<TMeasurement> : IMeasurement where TMeasurement : struct, Measurement<TMeasurement> {
    //    protected internal Measurement() { }
    //    public string Name { get; }
    //    public IReadOnlyDictionary<char, int> Dimensions { get; }
    //    public PhysicalUnit<TMeasurement> BaseUnit { get; }
    //}
}
