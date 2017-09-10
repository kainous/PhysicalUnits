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
    public static readonly IPhysicalUnit<Pressure> Bar = RegisterScaled("Bar", 1E5, "bar");
    public static readonly IPhysicalUnit<Pressure> Millibar = RegisterScaled("Millibar", 1E2, "bar");
    public static readonly IPhysicalUnit<Pressure> Kilopascals = RegisterScaled("Kilopascals", 1E3, "kPa");
    public static readonly IPhysicalUnit<Pressure> Megapascals = RegisterScaled("Megapascals", 1E6, "MPa");
    public static readonly IPhysicalUnit<Pressure> Hectopascals = RegisterScaled("Hectopascals", 1E5, "hPa");
    public static readonly IPhysicalUnit<Pressure> InchesOfMercury = RegisterScaled("Inches of mercury", 3386.389, "inHg");
    public static readonly IPhysicalUnit<Pressure> MillimetersOfMercury = RegisterScaled("Millimeters of mercury", 133.322368421, "mmHg");
    public static readonly IPhysicalUnit<Pressure> InchesOfWater = RegisterScaled("Inches of water", 248.84, "inH2O");
    public static readonly IPhysicalUnit<Pressure> CentimetersOfWater = RegisterScaled("Centimeters of water", 98.0665, "cmH2O");
    public static readonly IPhysicalUnit<Pressure> Torr = RegisterScaled("Torr", 133.322368421, "torr");

    public override IPhysicalUnit<Pressure> BaseUnit {
      get {
        return Pascals;
      }
    }
  }
}
