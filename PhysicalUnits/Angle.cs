using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalUnits {
  public sealed class Angle : Measure<Angle> {
    public static readonly IPhysicalUnit<Angle> Turns = RegisterBase("Turn", "turn");
    public static readonly IPhysicalUnit<Angle> Radians = RegisterScaled("Radians", 0.5 / Math.PI, "rad");

    public override IPhysicalUnit<Angle> BaseUnit {
      get {
        return Turns;
      }
    }
  }
}
