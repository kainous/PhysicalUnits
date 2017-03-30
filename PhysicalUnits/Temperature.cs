using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalUnits {
  public class Temperature : Measure<Temperature> {
    public static readonly IPhysicalUnit<Temperature> Kelvin = RegisterBase("Kelvin", "K");
    public static readonly IPhysicalUnit<Temperature> Millikelvin = RegisterScaled("Millikelvin", 1000.0, "mK");
    public static readonly IPhysicalUnit<Temperature> Rankin = RegisterScaled("Rankin", 1.8, "R");
    public static readonly IPhysicalUnit<Temperature> Celsius = RegisterLinear("Celsius", 1.0, 273.15, "°C");
    public static readonly IPhysicalUnit<Temperature> Fahrenheit = RegisterLinear("Fahrenheit", 1.8, 459.67, "°F");

    public override IPhysicalUnit<Temperature> BaseUnit {
      get {
        return Kelvin;
      }
    }
  }
}
