using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalUnits {
  public class PhysicalUnit<TMeasure> : IPhysicalUnit<TMeasure> 
    where TMeasure : IMeasure<TMeasure> {

    private readonly string _name;
    private readonly IReadOnlyList<string> _symbols;
    private readonly Func<double, double> _convertToBase;
    private readonly Func<double, double> _convertFromBase;
    double IPhysicalUnit<TMeasure>.ConvertToBase(double value) {
      return _convertToBase(value);
    }

    double IPhysicalUnit<TMeasure>.ConvertFromBase(double value) {
      return _convertFromBase(value);
    }

    public PhysicalUnit(string name, Func<double, double> convertToBase, Func<double, double> convertFromBase, params string[] symbols) {
      _name = name;
      _symbols = symbols;
      _convertFromBase = convertFromBase;
      _convertToBase = convertToBase;
    }

    public string Name {
      get {
        return _name;
      }
    }

    public IReadOnlyList<string> Symbols {
      get {
        return _symbols;
      }
    }
  }

  public static class PhysicalUnit {
    public static IPhysicalUnit<TMeasure> BaseUnit<TMeasure>(string name, params string[] symbols)
      where TMeasure : IMeasure<TMeasure> {
      return new PhysicalUnit<TMeasure>(name, a => a, a => a, symbols);
    }

    public static IPhysicalUnit<TMeasure> ScaledUnit<TMeasure>(string name, double scale, params string[] symbols)
      where TMeasure : IMeasure<TMeasure> {
      return new PhysicalUnit<TMeasure>(name, a => a * scale, a => a / scale, symbols);
    }

    internal static IPhysicalUnit<TMeasure> LinearUnit<TMeasure>(string name, double scale, double offset, params string[] symbols)
      where TMeasure : IMeasure<TMeasure> {
      return new PhysicalUnit<TMeasure>(name, a => a * scale + offset, a => (a - offset) / scale, symbols);
    }
  }
}
