using System.ComponentModel;

namespace PhysicalUnits {
  [ImmutableObject(true)]
  public sealed class PhysicalQuantity<TMeasure> where TMeasure : IMeasure<TMeasure> {
    private readonly double _value;
    private readonly IPhysicalUnit<TMeasure> _unit;

    internal PhysicalQuantity(double value, IPhysicalUnit<TMeasure> unit) {
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

    public static PhysicalDifference<TMeasure> operator -(PhysicalQuantity<TMeasure> a, PhysicalQuantity<TMeasure> b) {
      var result = a.Unit == b.Unit
               ? a.Value - b.Value
               : b.Unit.ConvertToBase(a.Unit.ConvertToBase(a.Value)) - b.Value;

      return new PhysicalDifference<TMeasure>(result, b.Unit);
    }
  }

  public static class PhysicalQuantity {
    public static PhysicalQuantity<TMeasure> ConvertTo<TMeasure>(
      this PhysicalQuantity<TMeasure> quantity,
      IPhysicalUnit<TMeasure> newUnit)

      where TMeasure : IMeasure<TMeasure> {

      var newValue = newUnit.ConvertFromBase(quantity.Unit.ConvertToBase(quantity.Value));
      return Create(newValue, newUnit);
    }

    public static PhysicalQuantity<TMeasure> Create<TMeasure>(double value, IPhysicalUnit<TMeasure> unit)
      where TMeasure : IMeasure<TMeasure> {

      return new PhysicalQuantity<TMeasure>(value, unit);
    }
  }
}
