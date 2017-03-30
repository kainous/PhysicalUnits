using System.Collections.Generic;

namespace PhysicalUnits {
  public interface IMeasure<TSelf> where TSelf : IMeasure<TSelf> {
    IPhysicalUnit<TSelf> BaseUnit { get; }
    PhysicalQuantity<TSelf> Origin { get; }
    IEnumerable<IPhysicalUnit<TSelf>> Units { get; }
  }
}
