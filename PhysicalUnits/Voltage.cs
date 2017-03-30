using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalUnits {
  public sealed class Voltage : Measure<Voltage> {
    public static readonly IPhysicalUnit<Voltage> Volts = RegisterBase("Volts", "V");
    public static readonly IPhysicalUnit<Voltage> Millivolts = RegisterScaled("Millivolts", 0.001, "mV");
    public static readonly IPhysicalUnit<Voltage> Microvolts = RegisterScaled("Microvolts", 0.001, "μV", "uV");
    public static readonly IPhysicalUnit<Voltage> Kilovolts = RegisterScaled("Kilovolts", 1000.0, "kV");
    public static readonly IPhysicalUnit<Voltage> Megavolts = RegisterScaled("Megavolts", 1E6, "MV");

    public override IPhysicalUnit<Voltage> BaseUnit {
      get { 
        return Volts;
      }
    }
  }
}
