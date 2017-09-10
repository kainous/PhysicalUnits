using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicalUnits {
  class Quaternion {
    public double W { get; }
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public Quaternion(double w, double x, double y, double z) {
      W = w;
      X = x;
      Y = y;
      Z = z;
    }

    public static Quaternion operator +(Quaternion a, Quaternion b) {
      return new Quaternion(
        a.W + b.W, 
        a.X + b.X, 
        a.Y + b.Y, 
        a.Z + b.Z);
    }

    public static Quaternion operator *(Quaternion a, Quaternion b) {
      return new Quaternion(
        a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,

        );
    }
  }
}
