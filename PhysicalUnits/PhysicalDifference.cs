namespace PhysicalUnits {
  public sealed class PhysicalDifference<TMeasure> where TMeasure : IMeasure<TMeasure> {
    private readonly double _value;
    private readonly IPhysicalUnit<TMeasure> _unit;

    internal PhysicalDifference(double value, IPhysicalUnit<TMeasure> unit) {
      _value = value;
      _unit = unit;
    }

    public double Value {
      get {
        return _value;
      }
    }

    public IPhysicalUnit<TMeasure> Unit {
      get {
        return _unit;
      }
    }

    public static PhysicalDifference<TMeasure> operator -(PhysicalDifference<TMeasure> a, PhysicalDifference<TMeasure> b) {
      var result = a.Unit == b.Unit
               ? a.Value - b.Value
               : b.Unit.ConvertToBase(a.Unit.ConvertToBase(a.Value)) - b.Value;

      return new PhysicalDifference<TMeasure>(result, b.Unit);
    }

    public static PhysicalDifference<TMeasure> operator +(PhysicalDifference<TMeasure> a, PhysicalDifference<TMeasure> b) {
      var result = a.Unit == b.Unit
               ? a.Value + b.Value
               : b.Unit.ConvertToBase(a.Unit.ConvertToBase(a.Value)) + b.Value;

      return new PhysicalDifference<TMeasure>(result, b.Unit);
    }

    public static PhysicalQuantity<TMeasure> operator +(PhysicalQuantity<TMeasure> a, PhysicalDifference<TMeasure> b) {
      var result = a.Unit == b.Unit
               ? a.Value + b.Value
               : b.Unit.ConvertToBase(a.Unit.ConvertToBase(a.Value)) + b.Value;

      return new PhysicalQuantity<TMeasure>(result, b.Unit);
    }

    public static PhysicalQuantity<TMeasure> operator +(PhysicalDifference<TMeasure> a, PhysicalQuantity<TMeasure> b) {
      var result = a.Unit == b.Unit
               ? a.Value + b.Value
               : b.Unit.ConvertToBase(a.Unit.ConvertToBase(a.Value)) + b.Value;

      return new PhysicalQuantity<TMeasure>(result, b.Unit);
    }
  }
}
