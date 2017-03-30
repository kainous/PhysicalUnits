using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalUnits {
  public abstract class Measure<TSelf> : IMeasure<TSelf> where TSelf : Measure<TSelf>, new() {
    private readonly IList<IPhysicalUnit<TSelf>> _units =
      new List<IPhysicalUnit<TSelf>>();

    public abstract IPhysicalUnit<TSelf> BaseUnit { get; }
    public IEnumerable<IPhysicalUnit<TSelf>> Units {
      get {
        return _units;
      }
    }

    private static TSelf _instance = new TSelf();

    public static TSelf Instance {
      get {
        return _instance;
      }
    }

    PhysicalQuantity<TSelf> IMeasure<TSelf>.Origin {
      get {
        return new PhysicalQuantity<TSelf>(0.0, BaseUnit);
      }
    }

    public static PhysicalQuantity<TSelf> Zero {
      get {
        return ((IMeasure<TSelf>)Instance).Origin;
      }
    }

    protected static IPhysicalUnit<TSelf> Register(IPhysicalUnit<TSelf> unit) {
      _instance._units.Add(unit);
      return unit;
    }

    protected static IPhysicalUnit<TSelf> RegisterBase(string name, params string[] symbols) {
      return PhysicalUnit.BaseUnit<TSelf>(name, symbols);
    }

    protected static IPhysicalUnit<TSelf> RegisterScaled(string name, double scale, params string[] symbols) {
      return PhysicalUnit.ScaledUnit<TSelf>(name, scale, symbols);
    }

    protected static IPhysicalUnit<TSelf> RegisterLinear(string name, double scale, double offset, params string[] symbols) {
      return PhysicalUnit.LinearUnit<TSelf>(name, scale, offset, symbols);
    }

    protected Measure() {
    }
  }
}
