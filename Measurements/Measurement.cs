using System.Collections.Generic;

namespace System.Measurements {
    public abstract class Measurement<TMeasurement> where TMeasurement : Measurement<TMeasurement> {
        protected internal Measurement() { }
        public abstract string Name { get; }
        public abstract IReadOnlyDictionary<char, int> Dimensions { get; }
        public abstract PhysicalUnit<TMeasurement> BaseUnit { get; }
    }
}
