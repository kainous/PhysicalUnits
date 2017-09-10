using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalUnits {
  public class Fraction : Measure<Fraction> {
    public static readonly IPhysicalUnit<Fraction> Proportion = RegisterBase("Ratio");
    public static readonly IPhysicalUnit<Fraction> Percentage = RegisterScaled("Percentage", 0.01, "%");
    public static readonly IPhysicalUnit<Fraction> Permille = RegisterScaled("Permille", 0.001, "‰");
    public static readonly IPhysicalUnit<Fraction> PartsPerMillion = RegisterScaled("Parts per million", 1E-6, "ppm");
    public static readonly IPhysicalUnit<Fraction> PartsPerBillion = RegisterScaled("Parts per billion", 1E-9, "ppb");
    public static readonly IPhysicalUnit<Fraction> PartsPerTrillion = RegisterScaled("Parts per trillion", 1E-12, "ppt");
    public static readonly IPhysicalUnit<Fraction> Decibel = Register(new PhysicalUnit<Fraction>("Decibel", a => 10.0 * Math.Log10(a), a => Math.Pow(10.0, a * 0.1), "dB"));

    public override IPhysicalUnit<Fraction> BaseUnit {
      get {
        return Proportion;
      }
    }
  }
}
