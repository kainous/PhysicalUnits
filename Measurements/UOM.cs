using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace System.Measurements {
    public partial class Dimension {
        public static Dimension Length = 
            new Dimension("Length", 'L');
        public static Dimension Time = 
            new Dimension("Time", 'T');
    }

    public partial struct Position : IMeasurement<Position> {
        public string Name => "Position";

        public IReadOnlyDictionary<char, int> Dimensions => 
            new Dictionary<char, int> {
                ['L'] = 1,
        };

        public Meters GetBaseUnit() =>
            default;

        IPhysicalUnit<Position> IMeasurement<Position>.GetBaseUnit() =>
            default(Meters);

        public static Meters Meters() => default;
        public static Affine1<Meters> Meters(double value) => new Affine1<Meters>(value, Meters.Instance);
            
        public static Inches Inches() => default;
        public static Affine1<Inches> Inches(double value) => new Affine1<Inches>(value, Inches.Instance);
            
        public static Feet Feet() => default;
        public static Affine1<Feet> Feet(double value) => new Affine1<Feet>(value, Feet.Instance);
            
    }

    public struct Meters : IPhysicalUnit<Position> {
        private Meters() {}
        public static Meters Instance => new Meters();
        public string Name => "Meter";
        public string Plural => "Meters";
        public IMeasurement<Position> Measurement => Position.Instance;
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public LinearFractionalTransformation ToBase =>
            LinearFractionalTransformation.Identity;

        public LinearFractionalTransformation FromBase =>
            LinearFractionalTransformation.Identity;
    }
    public struct Inches : IPhysicalUnit<Position> {
        private Inches() {}
        public static Inches Instance => new Inches();
        public string Name => "Inch";
        public string Plural => "Inches";
        public IMeasurement<Position> Measurement => Position.Instance;
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public LinearFractionalTransformation ToBase =>
            new LinearFractionalTransformation(0.025399999999999999);

        public LinearFractionalTransformation FromBase =>
            new LinearFractionalTransformation(39.370078740157481);
    }
    public struct Feet : IPhysicalUnit<Position> {
        private Feet() {}
        public static Feet Instance => new Feet();
        public string Name => "Foot";
        public string Plural => "Feet";
        public IMeasurement<Position> Measurement => Position.Instance;
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public LinearFractionalTransformation ToBase =>
            new LinearFractionalTransformation(0.30480000000000002);

        public LinearFractionalTransformation FromBase =>
            new LinearFractionalTransformation(3.280839895013123);
    }
    public partial struct Time : IMeasurement<Time> {
        public string Name => "Time";

        public IReadOnlyDictionary<char, int> Dimensions => 
            new Dictionary<char, int> {
                ['T'] = 1,
        };

        public Seconds GetBaseUnit() =>
            default;

        IPhysicalUnit<Time> IMeasurement<Time>.GetBaseUnit() =>
            default(Seconds);

        public static Seconds Seconds() => default;
        public static Affine1<Seconds> Seconds(double value) => new Affine1<Seconds>(value, Seconds.Instance);
            
        public static Minutes Minutes() => default;
        public static Affine1<Minutes> Minutes(double value) => new Affine1<Minutes>(value, Minutes.Instance);
            
    }

    public struct Seconds : IPhysicalUnit<Time> {
        private Seconds() {}
        public static Seconds Instance => new Seconds();
        public string Name => "Second";
        public string Plural => "Seconds";
        public IMeasurement<Time> Measurement => Time.Instance;
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public LinearFractionalTransformation ToBase =>
            LinearFractionalTransformation.Identity;

        public LinearFractionalTransformation FromBase =>
            LinearFractionalTransformation.Identity;
    }
    public struct Minutes : IPhysicalUnit<Time> {
        private Minutes() {}
        public static Minutes Instance => new Minutes();
        public string Name => "Minute";
        public string Plural => "Minutes";
        public IMeasurement<Time> Measurement => Time.Instance;
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public LinearFractionalTransformation ToBase =>
            new LinearFractionalTransformation(60);

        public LinearFractionalTransformation FromBase =>
            new LinearFractionalTransformation(0.016666666666666666);
    }
    public partial struct Velocity : IMeasurement<Velocity> {
        public string Name => "Velocity";

        public IReadOnlyDictionary<char, int> Dimensions => 
            new Dictionary<char, int> {
                ['L'] = 1,
                ['T'] = -1,
        };

        public MetersPerSecond GetBaseUnit() =>
            default;

        IPhysicalUnit<Velocity> IMeasurement<Velocity>.GetBaseUnit() =>
            default(MetersPerSecond);

        public static MetersPerSecond MetersPerSecond() => default;
        public static Affine1<MetersPerSecond> MetersPerSecond(double value) => new Affine1<MetersPerSecond>(value, MetersPerSecond.Instance);
            
        public static MilesPerHour MilesPerHour() => default;
        public static Affine1<MilesPerHour> MilesPerHour(double value) => new Affine1<MilesPerHour>(value, MilesPerHour.Instance);
            
    }

    public struct MetersPerSecond : IPhysicalUnit<Velocity> {
        private MetersPerSecond() {}
        public static MetersPerSecond Instance => new MetersPerSecond();
        public string Name => "MeterPerSecond";
        public string Plural => "MetersPerSecond";
        public IMeasurement<Velocity> Measurement => Velocity.Instance;
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public LinearFractionalTransformation ToBase =>
            LinearFractionalTransformation.Identity;

        public LinearFractionalTransformation FromBase =>
            LinearFractionalTransformation.Identity;
    }
    public struct MilesPerHour : IPhysicalUnit<Velocity> {
        private MilesPerHour() {}
        public static MilesPerHour Instance => new MilesPerHour();
        public string Name => "MilePerHour";
        public string Plural => "MilesPerHour";
        public IMeasurement<Velocity> Measurement => Velocity.Instance;
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public LinearFractionalTransformation ToBase =>
            new LinearFractionalTransformation(0.44703999999999999);

        public LinearFractionalTransformation FromBase =>
            new LinearFractionalTransformation(2.2369362920544025);
    }
}