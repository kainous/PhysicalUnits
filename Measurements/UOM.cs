using System.Collections.Generic;

namespace System.Measurements { 
    public partial class Dimension {
        public static Dimension Length = 
            new Dimension("Length", 'L');
        public static Dimension Time = 
            new Dimension("Time", 'T');
    }

    public partial class Position : Measurement<Position> {
        public override string Name => "Position";

        public override IReadOnlyDictionary<char, int> Dimensions => 
            new Dictionary<char, int> {
                ['L'] = 1,
        };

        public override PhysicalUnit<Position> BaseUnit =>
            Meters.Instance;

        public class Meters : PhysicalUnit<Position> {
            private Meters() {}
            public static Meters Instance => new Meters();
            public override string Name => "Meter";
            public override string Plural => "Meters";
            // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
            public override LinearFractionalTransformation ToBase =>
                LinearFractionalTransformation.Identity;
        }
        public class Inches : PhysicalUnit<Position> {
            private Inches() {}
            public static Inches Instance => new Inches();
            public override string Name => "Inch";
            public override string Plural => "Inches";
            // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
            public override LinearFractionalTransformation ToBase =>
                LinearFractionalTransformation.Identity * 0.025399999999999999;
        }
        public class Feet : PhysicalUnit<Position> {
            private Feet() {}
            public static Feet Instance => new Feet();
            public override string Name => "Foot";
            public override string Plural => "Feet";
            // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
            public override LinearFractionalTransformation ToBase =>
                LinearFractionalTransformation.Identity * 0.30480000000000002;
        }
    }
    public partial class Time : Measurement<Time> {
        public override string Name => "Time";

        public override IReadOnlyDictionary<char, int> Dimensions => 
            new Dictionary<char, int> {
                ['T'] = 1,
        };

        public override PhysicalUnit<Time> BaseUnit =>
            Seconds.Instance;

        public class Seconds : PhysicalUnit<Time> {
            private Seconds() {}
            public static Seconds Instance => new Seconds();
            public override string Name => "Second";
            public override string Plural => "Seconds";
            // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
            public override LinearFractionalTransformation ToBase =>
                LinearFractionalTransformation.Identity;
        }
        public class Minutes : PhysicalUnit<Time> {
            private Minutes() {}
            public static Minutes Instance => new Minutes();
            public override string Name => "Minute";
            public override string Plural => "Minutes";
            // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
            public override LinearFractionalTransformation ToBase =>
                LinearFractionalTransformation.Identity * 60;
        }
    }
    public partial class Velocity : Measurement<Velocity> {
        public override string Name => "Velocity";

        public override IReadOnlyDictionary<char, int> Dimensions => 
            new Dictionary<char, int> {
                ['L'] = 1,
                ['T'] = -1,
        };

        public override PhysicalUnit<Velocity> BaseUnit =>
            MetersPerSecond.Instance;

        public class MetersPerSecond : PhysicalUnit<Velocity> {
            private MetersPerSecond() {}
            public static MetersPerSecond Instance => new MetersPerSecond();
            public override string Name => "MeterPerSecond";
            public override string Plural => "MetersPerSecond";
            // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
            public override LinearFractionalTransformation ToBase =>
                LinearFractionalTransformation.Identity;
        }
        public class MilesPerHour : PhysicalUnit<Velocity> {
            private MilesPerHour() {}
            public static MilesPerHour Instance => new MilesPerHour();
            public override string Name => "MilePerHour";
            public override string Plural => "MilesPerHour";
            // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
            public override LinearFractionalTransformation ToBase =>
                LinearFractionalTransformation.Identity * 0.44703999999999999;
        }
    }
}