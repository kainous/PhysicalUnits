using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalUnits {
  public sealed class Current : Measure<Current> {
    public static readonly IPhysicalUnit<Current> Amps = RegisterBase("Amps", "A");
    public static readonly IPhysicalUnit<Current> Milliamps = RegisterScaled("Milliamps", 1000.0, "mA");
    public static readonly IPhysicalUnit<Current> Microamps = RegisterScaled("Microamps", 1000.0, "μA", "uA");

    public override IPhysicalUnit<Current> BaseUnit {
      get {
        return Amps;
      }
    }
  }
}
