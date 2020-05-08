using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace System.Measurements {
    public struct Affine1<TMeasurement>
      where TMeasurement : Measurement<TMeasurement> {
        public PhysicalUnit<TMeasurement> UnitOfMeasure { get; }

        private readonly IReadOnlyList<double> _items;      

        public double X => _items[0];

        internal double GetAt(int index) =>
            _items[index];

        public Affine1(
          double x,
          PhysicalUnit<TMeasurement> unitOfMeasure) {
            UnitOfMeasure = unitOfMeasure;
            _items = new ReadOnlyCollection<double>(new double[] {
                x,
            });
        }

        internal Affine1(double[] values, PhysicalUnit<TMeasurement> unitOfMeasure) {
            _items = new ReadOnlyCollection<double>(values);
            UnitOfMeasure = unitOfMeasure;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Affine1<TMeasurement> Map(Func<double, double> transform, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[_items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = transform(_items[i]);
            }
            return new Affine1<TMeasurement>(arr, targetUnits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Affine1<TMeasurement> Map2(Affine1<TMeasurement> first, Diff1<TMeasurement> second, Func<double, double, double> mapping, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[first.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = mapping(first._items[i], second._items[i]);
            }
            return new Affine1<TMeasurement>(arr, targetUnits);
        }

        public Affine1<TMeasurement> ConvertTo(PhysicalUnit<TMeasurement> targetUnits) =>
            Map(UnitOfMeasure.ToBase.ComposeWith(targetUnits.FromBase).Transform, targetUnits);
        
        public static Diff1<TMeasurement> operator -(Affine1<TMeasurement> first, Affine1<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        public static Affine1<TMeasurement> operator +(Affine1<TMeasurement> first, Diff1<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a + b, first.UnitOfMeasure)
            : first + second.ConvertTo(first.UnitOfMeasure);

        public static Affine1<TMeasurement> operator -(Affine1<TMeasurement> first, Diff1<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);
    }

    //A differential space, which is a vector space created by the affine space
    public struct Diff1<TMeasurement>
      where TMeasurement : Measurement<TMeasurement> {
        public PhysicalUnit<TMeasurement> UnitOfMeasure { get; }

        private readonly IReadOnlyList<double> _items;      

        public double X => _items[0];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal double GetAt(int index) =>
            _items[index];

        public Diff1(
          double x,
          PhysicalUnit<TMeasurement> unitOfMeasure) 
          : this(new[] {
                x,
          
          }, unitOfMeasure){          
        }

        internal Diff1(double[] values, PhysicalUnit<TMeasurement> unitOfMeasure) {
            _items = new ReadOnlyCollection<double>(values);
            UnitOfMeasure = unitOfMeasure;

            var squareMagnitude = new Lazy<Diff1<TMeasurement>>(() =>
                new Diff1<TMeasurement>(new[] {values.Select(x => x * x).Sum()}, unitOfMeasure));
                            
            _squareMagnitude = squareMagnitude;

            _magnitude = new Lazy<Diff1<TMeasurement>>(() =>
                new Diff1<TMeasurement>(new[] {Math.Sqrt(squareMagnitude.Value.GetAt(0))}, unitOfMeasure));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Affine1<TMeasurement> Map(Func<double, double> transform, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[_items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = transform(_items[i]);
            }
            return new Affine1<TMeasurement>(arr, targetUnits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Affine1<TMeasurement> Map2(Affine1<TMeasurement> first, Diff1<TMeasurement> second, Func<double, double, double> mapping, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[first._items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = mapping(first._items[i], second.GetAt(i));
            }
            return new Affine1<TMeasurement>(arr, targetUnits);
        }

        public Affine1<TMeasurement> ConvertTo(PhysicalUnit<TMeasurement> targetUnits) =>
            Map(UnitOfMeasure.ToBase.ComposeWith(targetUnits.FromBase).Transform, targetUnits);
        
        public static Diff1<TMeasurement> operator -(Affine1<TMeasurement> first, Affine1<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        public static Affine1<TMeasurement> operator +(Affine1<TMeasurement> first, Diff1<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a + b, first.UnitOfMeasure)
            : first + second.ConvertTo(first.UnitOfMeasure);

        public static Affine1<TMeasurement> operator -(Affine1<TMeasurement> first, Diff1<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        private readonly Lazy<Diff1<TMeasurement>> _magnitude;
        public Diff1<TMeasurement> Magnitude => _magnitude.Value;

        private readonly Lazy<Diff1<TMeasurement>> _squareMagnitude;
        public Diff1<TMeasurement> SquareMagnitude => _squareMagnitude.Value;
    }
    public struct Affine2<TMeasurement>
      where TMeasurement : Measurement<TMeasurement> {
        public PhysicalUnit<TMeasurement> UnitOfMeasure { get; }

        private readonly IReadOnlyList<double> _items;      

        public double X => _items[0];
        public double Y => _items[1];

        internal double GetAt(int index) =>
            _items[index];

        public Affine2(
          double x,
          double y,
          PhysicalUnit<TMeasurement> unitOfMeasure) {
            UnitOfMeasure = unitOfMeasure;
            _items = new ReadOnlyCollection<double>(new double[] {
                x,
                y,
            });
        }

        internal Affine2(double[] values, PhysicalUnit<TMeasurement> unitOfMeasure) {
            _items = new ReadOnlyCollection<double>(values);
            UnitOfMeasure = unitOfMeasure;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Affine2<TMeasurement> Map(Func<double, double> transform, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[_items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = transform(_items[i]);
            }
            return new Affine2<TMeasurement>(arr, targetUnits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Affine2<TMeasurement> Map2(Affine2<TMeasurement> first, Diff2<TMeasurement> second, Func<double, double, double> mapping, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[first.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = mapping(first._items[i], second._items[i]);
            }
            return new Affine2<TMeasurement>(arr, targetUnits);
        }

        public Affine2<TMeasurement> ConvertTo(PhysicalUnit<TMeasurement> targetUnits) =>
            Map(UnitOfMeasure.ToBase.ComposeWith(targetUnits.FromBase).Transform, targetUnits);
        
        public static Diff2<TMeasurement> operator -(Affine2<TMeasurement> first, Affine2<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        public static Affine2<TMeasurement> operator +(Affine2<TMeasurement> first, Diff2<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a + b, first.UnitOfMeasure)
            : first + second.ConvertTo(first.UnitOfMeasure);

        public static Affine2<TMeasurement> operator -(Affine2<TMeasurement> first, Diff2<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);
    }

    //A differential space, which is a vector space created by the affine space
    public struct Diff2<TMeasurement>
      where TMeasurement : Measurement<TMeasurement> {
        public PhysicalUnit<TMeasurement> UnitOfMeasure { get; }

        private readonly IReadOnlyList<double> _items;      

        public double X => _items[0];
        public double Y => _items[1];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal double GetAt(int index) =>
            _items[index];

        public Diff2(
          double x,
          double y,
          PhysicalUnit<TMeasurement> unitOfMeasure) 
          : this(new[] {
                x,
                y,
          
          }, unitOfMeasure){          
        }

        internal Diff2(double[] values, PhysicalUnit<TMeasurement> unitOfMeasure) {
            _items = new ReadOnlyCollection<double>(values);
            UnitOfMeasure = unitOfMeasure;

            var squareMagnitude = new Lazy<Diff1<TMeasurement>>(() =>
                new Diff1<TMeasurement>(new[] {values.Select(x => x * x).Sum()}, unitOfMeasure));
                            
            _squareMagnitude = squareMagnitude;

            _magnitude = new Lazy<Diff1<TMeasurement>>(() =>
                new Diff1<TMeasurement>(new[] {Math.Sqrt(squareMagnitude.Value.GetAt(0))}, unitOfMeasure));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Affine2<TMeasurement> Map(Func<double, double> transform, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[_items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = transform(_items[i]);
            }
            return new Affine2<TMeasurement>(arr, targetUnits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Affine2<TMeasurement> Map2(Affine2<TMeasurement> first, Diff2<TMeasurement> second, Func<double, double, double> mapping, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[first._items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = mapping(first._items[i], second.GetAt(i));
            }
            return new Affine2<TMeasurement>(arr, targetUnits);
        }

        public Affine2<TMeasurement> ConvertTo(PhysicalUnit<TMeasurement> targetUnits) =>
            Map(UnitOfMeasure.ToBase.ComposeWith(targetUnits.FromBase).Transform, targetUnits);
        
        public static Diff2<TMeasurement> operator -(Affine2<TMeasurement> first, Affine2<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        public static Affine2<TMeasurement> operator +(Affine2<TMeasurement> first, Diff2<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a + b, first.UnitOfMeasure)
            : first + second.ConvertTo(first.UnitOfMeasure);

        public static Affine2<TMeasurement> operator -(Affine2<TMeasurement> first, Diff2<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        private readonly Lazy<Diff1<TMeasurement>> _magnitude;
        public Diff1<TMeasurement> Magnitude => _magnitude.Value;

        private readonly Lazy<Diff1<TMeasurement>> _squareMagnitude;
        public Diff1<TMeasurement> SquareMagnitude => _squareMagnitude.Value;
    }
    public struct Affine3<TMeasurement>
      where TMeasurement : Measurement<TMeasurement> {
        public PhysicalUnit<TMeasurement> UnitOfMeasure { get; }

        private readonly IReadOnlyList<double> _items;      

        public double X => _items[0];
        public double Y => _items[1];
        public double Z => _items[2];

        internal double GetAt(int index) =>
            _items[index];

        public Affine3(
          double x,
          double y,
          double z,
          PhysicalUnit<TMeasurement> unitOfMeasure) {
            UnitOfMeasure = unitOfMeasure;
            _items = new ReadOnlyCollection<double>(new double[] {
                x,
                y,
                z,
            });
        }

        internal Affine3(double[] values, PhysicalUnit<TMeasurement> unitOfMeasure) {
            _items = new ReadOnlyCollection<double>(values);
            UnitOfMeasure = unitOfMeasure;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Affine3<TMeasurement> Map(Func<double, double> transform, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[_items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = transform(_items[i]);
            }
            return new Affine3<TMeasurement>(arr, targetUnits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Affine3<TMeasurement> Map2(Affine3<TMeasurement> first, Diff3<TMeasurement> second, Func<double, double, double> mapping, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[first.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = mapping(first._items[i], second._items[i]);
            }
            return new Affine3<TMeasurement>(arr, targetUnits);
        }

        public Affine3<TMeasurement> ConvertTo(PhysicalUnit<TMeasurement> targetUnits) =>
            Map(UnitOfMeasure.ToBase.ComposeWith(targetUnits.FromBase).Transform, targetUnits);
        
        public static Diff3<TMeasurement> operator -(Affine3<TMeasurement> first, Affine3<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        public static Affine3<TMeasurement> operator +(Affine3<TMeasurement> first, Diff3<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a + b, first.UnitOfMeasure)
            : first + second.ConvertTo(first.UnitOfMeasure);

        public static Affine3<TMeasurement> operator -(Affine3<TMeasurement> first, Diff3<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);
    }

    //A differential space, which is a vector space created by the affine space
    public struct Diff3<TMeasurement>
      where TMeasurement : Measurement<TMeasurement> {
        public PhysicalUnit<TMeasurement> UnitOfMeasure { get; }

        private readonly IReadOnlyList<double> _items;      

        public double X => _items[0];
        public double Y => _items[1];
        public double Z => _items[2];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal double GetAt(int index) =>
            _items[index];

        public Diff3(
          double x,
          double y,
          double z,
          PhysicalUnit<TMeasurement> unitOfMeasure) 
          : this(new[] {
                x,
                y,
                z,
          
          }, unitOfMeasure){          
        }

        internal Diff3(double[] values, PhysicalUnit<TMeasurement> unitOfMeasure) {
            _items = new ReadOnlyCollection<double>(values);
            UnitOfMeasure = unitOfMeasure;

            var squareMagnitude = new Lazy<Diff1<TMeasurement>>(() =>
                new Diff1<TMeasurement>(new[] {values.Select(x => x * x).Sum()}, unitOfMeasure));
                            
            _squareMagnitude = squareMagnitude;

            _magnitude = new Lazy<Diff1<TMeasurement>>(() =>
                new Diff1<TMeasurement>(new[] {Math.Sqrt(squareMagnitude.Value.GetAt(0))}, unitOfMeasure));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Affine3<TMeasurement> Map(Func<double, double> transform, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[_items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = transform(_items[i]);
            }
            return new Affine3<TMeasurement>(arr, targetUnits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Affine3<TMeasurement> Map2(Affine3<TMeasurement> first, Diff3<TMeasurement> second, Func<double, double, double> mapping, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[first._items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = mapping(first._items[i], second.GetAt(i));
            }
            return new Affine3<TMeasurement>(arr, targetUnits);
        }

        public Affine3<TMeasurement> ConvertTo(PhysicalUnit<TMeasurement> targetUnits) =>
            Map(UnitOfMeasure.ToBase.ComposeWith(targetUnits.FromBase).Transform, targetUnits);
        
        public static Diff3<TMeasurement> operator -(Affine3<TMeasurement> first, Affine3<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        public static Affine3<TMeasurement> operator +(Affine3<TMeasurement> first, Diff3<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a + b, first.UnitOfMeasure)
            : first + second.ConvertTo(first.UnitOfMeasure);

        public static Affine3<TMeasurement> operator -(Affine3<TMeasurement> first, Diff3<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        private readonly Lazy<Diff1<TMeasurement>> _magnitude;
        public Diff1<TMeasurement> Magnitude => _magnitude.Value;

        private readonly Lazy<Diff1<TMeasurement>> _squareMagnitude;
        public Diff1<TMeasurement> SquareMagnitude => _squareMagnitude.Value;
    }
    public struct Affine4<TMeasurement>
      where TMeasurement : Measurement<TMeasurement> {
        public PhysicalUnit<TMeasurement> UnitOfMeasure { get; }

        private readonly IReadOnlyList<double> _items;      

        public double X => _items[0];
        public double Y => _items[1];
        public double Z => _items[2];
        public double W => _items[3];

        internal double GetAt(int index) =>
            _items[index];

        public Affine4(
          double x,
          double y,
          double z,
          double w,
          PhysicalUnit<TMeasurement> unitOfMeasure) {
            UnitOfMeasure = unitOfMeasure;
            _items = new ReadOnlyCollection<double>(new double[] {
                x,
                y,
                z,
                w,
            });
        }

        internal Affine4(double[] values, PhysicalUnit<TMeasurement> unitOfMeasure) {
            _items = new ReadOnlyCollection<double>(values);
            UnitOfMeasure = unitOfMeasure;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Affine4<TMeasurement> Map(Func<double, double> transform, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[_items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = transform(_items[i]);
            }
            return new Affine4<TMeasurement>(arr, targetUnits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Affine4<TMeasurement> Map2(Affine4<TMeasurement> first, Diff4<TMeasurement> second, Func<double, double, double> mapping, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[first.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = mapping(first._items[i], second._items[i]);
            }
            return new Affine4<TMeasurement>(arr, targetUnits);
        }

        public Affine4<TMeasurement> ConvertTo(PhysicalUnit<TMeasurement> targetUnits) =>
            Map(UnitOfMeasure.ToBase.ComposeWith(targetUnits.FromBase).Transform, targetUnits);
        
        public static Diff4<TMeasurement> operator -(Affine4<TMeasurement> first, Affine4<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        public static Affine4<TMeasurement> operator +(Affine4<TMeasurement> first, Diff4<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a + b, first.UnitOfMeasure)
            : first + second.ConvertTo(first.UnitOfMeasure);

        public static Affine4<TMeasurement> operator -(Affine4<TMeasurement> first, Diff4<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);
    }

    //A differential space, which is a vector space created by the affine space
    public struct Diff4<TMeasurement>
      where TMeasurement : Measurement<TMeasurement> {
        public PhysicalUnit<TMeasurement> UnitOfMeasure { get; }

        private readonly IReadOnlyList<double> _items;      

        public double X => _items[0];
        public double Y => _items[1];
        public double Z => _items[2];
        public double W => _items[3];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal double GetAt(int index) =>
            _items[index];

        public Diff4(
          double x,
          double y,
          double z,
          double w,
          PhysicalUnit<TMeasurement> unitOfMeasure) 
          : this(new[] {
                x,
                y,
                z,
                w,
          
          }, unitOfMeasure){          
        }

        internal Diff4(double[] values, PhysicalUnit<TMeasurement> unitOfMeasure) {
            _items = new ReadOnlyCollection<double>(values);
            UnitOfMeasure = unitOfMeasure;

            var squareMagnitude = new Lazy<Diff1<TMeasurement>>(() =>
                new Diff1<TMeasurement>(new[] {values.Select(x => x * x).Sum()}, unitOfMeasure));
                            
            _squareMagnitude = squareMagnitude;

            _magnitude = new Lazy<Diff1<TMeasurement>>(() =>
                new Diff1<TMeasurement>(new[] {Math.Sqrt(squareMagnitude.Value.GetAt(0))}, unitOfMeasure));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Affine4<TMeasurement> Map(Func<double, double> transform, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[_items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = transform(_items[i]);
            }
            return new Affine4<TMeasurement>(arr, targetUnits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Affine4<TMeasurement> Map2(Affine4<TMeasurement> first, Diff4<TMeasurement> second, Func<double, double, double> mapping, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[first._items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = mapping(first._items[i], second.GetAt(i));
            }
            return new Affine4<TMeasurement>(arr, targetUnits);
        }

        public Affine4<TMeasurement> ConvertTo(PhysicalUnit<TMeasurement> targetUnits) =>
            Map(UnitOfMeasure.ToBase.ComposeWith(targetUnits.FromBase).Transform, targetUnits);
        
        public static Diff4<TMeasurement> operator -(Affine4<TMeasurement> first, Affine4<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        public static Affine4<TMeasurement> operator +(Affine4<TMeasurement> first, Diff4<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a + b, first.UnitOfMeasure)
            : first + second.ConvertTo(first.UnitOfMeasure);

        public static Affine4<TMeasurement> operator -(Affine4<TMeasurement> first, Diff4<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        private readonly Lazy<Diff1<TMeasurement>> _magnitude;
        public Diff1<TMeasurement> Magnitude => _magnitude.Value;

        private readonly Lazy<Diff1<TMeasurement>> _squareMagnitude;
        public Diff1<TMeasurement> SquareMagnitude => _squareMagnitude.Value;
    }
    public struct Affine5<TMeasurement>
      where TMeasurement : Measurement<TMeasurement> {
        public PhysicalUnit<TMeasurement> UnitOfMeasure { get; }

        private readonly IReadOnlyList<double> _items;      

        public double X0 => _items[0];
        public double X1 => _items[1];
        public double X2 => _items[2];
        public double X3 => _items[3];
        public double X4 => _items[4];

        internal double GetAt(int index) =>
            _items[index];

        public Affine5(
          double x0,
          double x1,
          double x2,
          double x3,
          double x4,
          PhysicalUnit<TMeasurement> unitOfMeasure) {
            UnitOfMeasure = unitOfMeasure;
            _items = new ReadOnlyCollection<double>(new double[] {
                x0,
                x1,
                x2,
                x3,
                x4,
            });
        }

        internal Affine5(double[] values, PhysicalUnit<TMeasurement> unitOfMeasure) {
            _items = new ReadOnlyCollection<double>(values);
            UnitOfMeasure = unitOfMeasure;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Affine5<TMeasurement> Map(Func<double, double> transform, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[_items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = transform(_items[i]);
            }
            return new Affine5<TMeasurement>(arr, targetUnits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Affine5<TMeasurement> Map2(Affine5<TMeasurement> first, Diff5<TMeasurement> second, Func<double, double, double> mapping, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[first.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = mapping(first._items[i], second._items[i]);
            }
            return new Affine5<TMeasurement>(arr, targetUnits);
        }

        public Affine5<TMeasurement> ConvertTo(PhysicalUnit<TMeasurement> targetUnits) =>
            Map(UnitOfMeasure.ToBase.ComposeWith(targetUnits.FromBase).Transform, targetUnits);
        
        public static Diff5<TMeasurement> operator -(Affine5<TMeasurement> first, Affine5<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        public static Affine5<TMeasurement> operator +(Affine5<TMeasurement> first, Diff5<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a + b, first.UnitOfMeasure)
            : first + second.ConvertTo(first.UnitOfMeasure);

        public static Affine5<TMeasurement> operator -(Affine5<TMeasurement> first, Diff5<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);
    }

    //A differential space, which is a vector space created by the affine space
    public struct Diff5<TMeasurement>
      where TMeasurement : Measurement<TMeasurement> {
        public PhysicalUnit<TMeasurement> UnitOfMeasure { get; }

        private readonly IReadOnlyList<double> _items;      

        public double X0 => _items[0];
        public double X1 => _items[1];
        public double X2 => _items[2];
        public double X3 => _items[3];
        public double X4 => _items[4];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal double GetAt(int index) =>
            _items[index];

        public Diff5(
          double x0,
          double x1,
          double x2,
          double x3,
          double x4,
          PhysicalUnit<TMeasurement> unitOfMeasure) 
          : this(new[] {
                x0,
                x1,
                x2,
                x3,
                x4,
          
          }, unitOfMeasure){          
        }

        internal Diff5(double[] values, PhysicalUnit<TMeasurement> unitOfMeasure) {
            _items = new ReadOnlyCollection<double>(values);
            UnitOfMeasure = unitOfMeasure;

            var squareMagnitude = new Lazy<Diff1<TMeasurement>>(() =>
                new Diff1<TMeasurement>(new[] {values.Select(x => x * x).Sum()}, unitOfMeasure));
                            
            _squareMagnitude = squareMagnitude;

            _magnitude = new Lazy<Diff1<TMeasurement>>(() =>
                new Diff1<TMeasurement>(new[] {Math.Sqrt(squareMagnitude.Value.GetAt(0))}, unitOfMeasure));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Affine5<TMeasurement> Map(Func<double, double> transform, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[_items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = transform(_items[i]);
            }
            return new Affine5<TMeasurement>(arr, targetUnits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Affine5<TMeasurement> Map2(Affine5<TMeasurement> first, Diff5<TMeasurement> second, Func<double, double, double> mapping, PhysicalUnit<TMeasurement> targetUnits) {
            var arr = new double[first._items.Count];
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = mapping(first._items[i], second.GetAt(i));
            }
            return new Affine5<TMeasurement>(arr, targetUnits);
        }

        public Affine5<TMeasurement> ConvertTo(PhysicalUnit<TMeasurement> targetUnits) =>
            Map(UnitOfMeasure.ToBase.ComposeWith(targetUnits.FromBase).Transform, targetUnits);
        
        public static Diff5<TMeasurement> operator -(Affine5<TMeasurement> first, Affine5<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        public static Affine5<TMeasurement> operator +(Affine5<TMeasurement> first, Diff5<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a + b, first.UnitOfMeasure)
            : first + second.ConvertTo(first.UnitOfMeasure);

        public static Affine5<TMeasurement> operator -(Affine5<TMeasurement> first, Diff5<TMeasurement> second) =>
            first.UnitOfMeasure == second.UnitOfMeasure
            ? Map2((a, b) => a - b, first.UnitOfMeasure)
            : first - second.ConvertTo(first.UnitOfMeasure);

        private readonly Lazy<Diff1<TMeasurement>> _magnitude;
        public Diff1<TMeasurement> Magnitude => _magnitude.Value;

        private readonly Lazy<Diff1<TMeasurement>> _squareMagnitude;
        public Diff1<TMeasurement> SquareMagnitude => _squareMagnitude.Value;
    }

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
            _Meters.Instance;

        public static PhysicalUnit<Position> Meters() => _Meters.Instance;
        public static Affine1<Position> Meters(double value) => new Affine1<Position>(value, _Meters.Instance);
            
        public static PhysicalUnit<Position> Inches() => _Inches.Instance;
        public static Affine1<Position> Inches(double value) => new Affine1<Position>(value, _Inches.Instance);
            
        public static PhysicalUnit<Position> Feet() => _Feet.Instance;
        public static Affine1<Position> Feet(double value) => new Affine1<Position>(value, _Feet.Instance);
            
    }

    internal class _Meters : PhysicalUnit<Position> {
        private _Meters() {}
        public static _Meters Instance => new _Meters();
        public override string Name => "Meter";
        public override string Plural => "Meters";
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public override LinearFractionalTransformation ToBase =>
            LinearFractionalTransformation.Identity;

        public override LinearFractionalTransformation FromBase =>
            LinearFractionalTransformation.Identity;
    }
    internal class _Inches : PhysicalUnit<Position> {
        private _Inches() {}
        public static _Inches Instance => new _Inches();
        public override string Name => "Inch";
        public override string Plural => "Inches";
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public override LinearFractionalTransformation ToBase =>
            new LinearFractionalTransformation(0.025399999999999999);

        public override LinearFractionalTransformation FromBase =>
            new LinearFractionalTransformation(39.370078740157481);
    }
    internal class _Feet : PhysicalUnit<Position> {
        private _Feet() {}
        public static _Feet Instance => new _Feet();
        public override string Name => "Foot";
        public override string Plural => "Feet";
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public override LinearFractionalTransformation ToBase =>
            new LinearFractionalTransformation(0.30480000000000002);

        public override LinearFractionalTransformation FromBase =>
            new LinearFractionalTransformation(3.280839895013123);
    }
    public partial class Time : Measurement<Time> {
        public override string Name => "Time";

        public override IReadOnlyDictionary<char, int> Dimensions => 
            new Dictionary<char, int> {
                ['T'] = 1,
        };

        public override PhysicalUnit<Time> BaseUnit =>
            _Seconds.Instance;

        public static PhysicalUnit<Time> Seconds() => _Seconds.Instance;
        public static Affine1<Time> Seconds(double value) => new Affine1<Time>(value, _Seconds.Instance);
            
        public static PhysicalUnit<Time> Minutes() => _Minutes.Instance;
        public static Affine1<Time> Minutes(double value) => new Affine1<Time>(value, _Minutes.Instance);
            
    }

    internal class _Seconds : PhysicalUnit<Time> {
        private _Seconds() {}
        public static _Seconds Instance => new _Seconds();
        public override string Name => "Second";
        public override string Plural => "Seconds";
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public override LinearFractionalTransformation ToBase =>
            LinearFractionalTransformation.Identity;

        public override LinearFractionalTransformation FromBase =>
            LinearFractionalTransformation.Identity;
    }
    internal class _Minutes : PhysicalUnit<Time> {
        private _Minutes() {}
        public static _Minutes Instance => new _Minutes();
        public override string Name => "Minute";
        public override string Plural => "Minutes";
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public override LinearFractionalTransformation ToBase =>
            new LinearFractionalTransformation(60);

        public override LinearFractionalTransformation FromBase =>
            new LinearFractionalTransformation(0.016666666666666666);
    }
    public partial class Velocity : Measurement<Velocity> {
        public override string Name => "Velocity";

        public override IReadOnlyDictionary<char, int> Dimensions => 
            new Dictionary<char, int> {
                ['L'] = 1,
                ['T'] = -1,
        };

        public override PhysicalUnit<Velocity> BaseUnit =>
            _MetersPerSecond.Instance;

        public static PhysicalUnit<Velocity> MetersPerSecond() => _MetersPerSecond.Instance;
        public static Affine1<Velocity> MetersPerSecond(double value) => new Affine1<Velocity>(value, _MetersPerSecond.Instance);
            
        public static PhysicalUnit<Velocity> MilesPerHour() => _MilesPerHour.Instance;
        public static Affine1<Velocity> MilesPerHour(double value) => new Affine1<Velocity>(value, _MilesPerHour.Instance);
            
    }

    internal class _MetersPerSecond : PhysicalUnit<Velocity> {
        private _MetersPerSecond() {}
        public static _MetersPerSecond Instance => new _MetersPerSecond();
        public override string Name => "MeterPerSecond";
        public override string Plural => "MetersPerSecond";
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public override LinearFractionalTransformation ToBase =>
            LinearFractionalTransformation.Identity;

        public override LinearFractionalTransformation FromBase =>
            LinearFractionalTransformation.Identity;
    }
    internal class _MilesPerHour : PhysicalUnit<Velocity> {
        private _MilesPerHour() {}
        public static _MilesPerHour Instance => new _MilesPerHour();
        public override string Name => "MilePerHour";
        public override string Plural => "MilesPerHour";
        // Round-trip doubles are being used, and so may not look exactly like the values found in JSON
        public override LinearFractionalTransformation ToBase =>
            new LinearFractionalTransformation(0.44703999999999999);

        public override LinearFractionalTransformation FromBase =>
            new LinearFractionalTransformation(2.2369362920544025);
    }
}