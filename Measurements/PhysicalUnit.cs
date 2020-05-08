using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;

namespace System.Measurements {
    public abstract class PhysicalUnit<TMeasurement>
      where TMeasurement : Measurement<TMeasurement> {
        protected internal PhysicalUnit() { }
        public abstract string Name { get; }
        public abstract string Plural { get; }
        public abstract LinearFractionalTransformation ToBase { get; }
        public abstract LinearFractionalTransformation FromBase { get; }
    }
}
