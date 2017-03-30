using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalUnits {
  public class Pressure : Measure<Pressure> {
    public static readonly IPhysicalUnit<Pressure> Pascals = RegisterBase("Pascals", "Pa");
    public static readonly IPhysicalUnit<Pressure> Psi = RegisterScaled("Pounds per square inch", 6894.757293168, "psi");
    public static readonly IPhysicalUnit<Pressure> Atm = RegisterScaled("Atmospheres", 101325.0, "atm");
    public static readonly IPhysicalUnit<Pressure> Kilopascals = RegisterScaled("Kilopascals", 1000.0, "kPa");
    public static readonly IPhysicalUnit<Pressure> Bar = RegisterScaled("Bar", 100000.0, "bar");

    public override IPhysicalUnit<Pressure> BaseUnit {
      get {
        return Pascals;
      }
    }
  }
}
