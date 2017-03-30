using System.Collections.Generic;

namespace PhysicalUnits {
  public interface IPhysicalUnit<TMeasure> where TMeasure : IMeasure<TMeasure> {
    double ConvertToBase(double value);
    double ConvertFromBase(double value);
    string Name { get; }
    IReadOnlyList<string> Symbols { get; }
  }
}
