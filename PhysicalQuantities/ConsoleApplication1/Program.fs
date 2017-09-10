open System

[<AbstractClass>]
type Measure<'TMeasure when 'TMeasure :> Measure<'TMeasure>>() =
  abstract Name:string
  abstract Zero:AffineQuantity<'TMeasure>

and Unit<'TMeasure>(name:string, symbols:string list, convertToBase:float -> float, convertFromBase:float -> float) = 
  member __.ConvertToBase   = convertToBase
  member __.ConvertFromBase = convertFromBase
  member __.Name            = name
  member __.Symbols         = symbols
  
  member this.Equals (other:Unit<'TMeasure>) =
      this.Name = other.Name

  interface IEquatable<Unit<'TMeasure>> with
    member this.Equals other = this.Equals other

  override this.Equals other =
    match other with
    | :? Unit<'TMeasure> as u -> this.Name = u.Name
    | _ -> false

  override this.GetHashCode() = this.Name.GetHashCode()

  static member op_Equality (a:Unit<'TMeasure>, b:Unit<'TMeasure>) = a.Name = b.Name
  static member op_Inequality (a:Unit<'TMeasure>, b:Unit<'TMeasure>) = a.Name <> b.Name

and FieldQuantity<'TMeasure when 'TMeasure :> Measure<'TMeasure>>(value:float, units:Unit<'TMeasure> ) = 
  static let combine (a:FieldQuantity<'TMeasure>) (b:FieldQuantity<'TMeasure>) f =
    let scalar = 
      b.Units
      :> Unit<'TMeasure>
      |> fun u -> u.ConvertToBase b.Value
      |> a.Units.ConvertFromBase
    FieldQuantity(f a.Value scalar, a.Units)
  
  member __.Value = value
  member __.Units = units

  static member ( ~- ) (a:FieldQuantity<'TMeasure>) = FieldQuantity<'TMeasure>(-a.Value, a.Units)
  static member ( ~+ ) (a:FieldQuantity<'TMeasure>) = FieldQuantity<'TMeasure>( a.Value, a.Units)
  static member (+) (a:FieldQuantity<'TMeasure>, b:FieldQuantity<'TMeasure>) = combine a b (+)
  static member (-) (a:FieldQuantity<'TMeasure>, b:FieldQuantity<'TMeasure>) = combine a b (-)

and AffineQuantity<'TMeasure when 'TMeasure :> Measure<'TMeasure>>(value:float, units:Unit<'TMeasure>) =
  member this.Value = value
  member this.Units = units

  static member (-) (a:AffineQuantity<'TMeasure>, b:AffineQuantity<'TMeasure>) = 
    let scalar = 
      b.Value
      |> b.Units.ConvertToBase
      |> a.Units.ConvertFromBase
    FieldQuantity(a.Value - scalar, a.Units)

  static member (-) (a:AffineQuantity<'TMeasure>, b:FieldQuantity<'TMeasure>) = 
    let scalar = 
      b.Value
      |> b.Units.ConvertToBase
      |> a.Units.ConvertFromBase
    AffineQuantity(a.Value - scalar, a.Units)

  static member (+) (a:AffineQuantity<'TMeasure>, b:FieldQuantity<'TMeasure>) = 
    let scalar = 
      b.Value
      |> b.Units.ConvertToBase
      |> a.Units.ConvertFromBase
    AffineQuantity(a.Value + scalar, a.Units)

  static member (+) (a:FieldQuantity<'TMeasure>, b:AffineQuantity<'TMeasure>) = 
    let scalar = 
      b.Value
      |> b.Units.ConvertToBase
      |> a.Units.ConvertFromBase
    AffineQuantity(a.Value + scalar, a.Units)

let LinearUnit name symbols scalar offset =
  Unit<'TMeasure>(name, symbols, (fun x -> x * scalar + offset), fun x -> (x - offset) / scalar)

let ScalarUnit name symbols scalar = LinearUnit name symbols scalar 0.0
let BaseUnit name symbols = Unit<'TMeasure>(name, symbols, id, id)

type Pressure() =
  inherit Measure<Pressure>()

  override __.Name = "Pressure"
  override __.Zero = AffineQuantity(0.0, Pressure.Pascals)

  static member Pascals    :Unit<Pressure> = BaseUnit   "Pascals"     ["Pa"]
  static member Kilopascals:Unit<Pressure> = ScalarUnit "Kilopascals" ["kPa"] 1000.0

type Temperature() =
  inherit Measure<Temperature>()

  override __.Name = "Temperature"
  override __.Zero = AffineQuantity(0.0, Temperature.Kelvin)

  static member Kelvin    :Unit<Temperature> = BaseUnit   "Kelvin"     ["K"]
  static member Celsius   :Unit<Temperature> = LinearUnit "Celsius"    ["°C"] 1.0 -273.15
  static member Fahrenheit:Unit<Temperature> = LinearUnit "Fahrenheit" ["°F"] 1.8 -459.67

type MassFlowRate() =
  inherit Measure<MassFlowRate>()

  override __.Name = "Mass flow rate"
  override __.Zero = AffineQuantity(0.0, MassFlowRate.KilogramsPerSecond)

  static member KilogramsPerSecond:Unit<MassFlowRate> = BaseUnit "Kilograms per second" ["kg/s"]
  
type VolumetricFlowRate() =
  inherit Measure<VolumetricFlowRate>()

  override __.Name = "Volumetric flow rate"
  override __.Zero = AffineQuantity(0.0, VolumetricFlowRate.CubicMetersPerSecond)

  static member CubicMetersPerSecond:Unit<VolumetricFlowRate> = BaseUnit   "Cubic meters per second" ["m³/s"]
  static member USGallonsPerMinute  :Unit<VolumetricFlowRate> = ScalarUnit "US Gallons per minute"   ["USgpm"] 15850.372483753
  static member CubicFeetPerSecond  :Unit<VolumetricFlowRate> = ScalarUnit "Cubic feet per second"   ["ft³/s"] 35.314666721489

type Fractional() =
  inherit Measure<Fractional>()

  override __.Name = "Fractional"
  override __.Zero = AffineQuantity(0.0, Fractional.Fraction)

  static member Fraction:Unit<Fractional> = BaseUnit "Fraction" []
  static member Percent:Unit<Fractional> = ScalarUnit "Percent" ["%"] 100.0
  static member Permille:Unit<Fractional> = ScalarUnit "Permille" ["‰"] 1000.0
  static member PartsPerMillion:Unit<Fractional> = ScalarUnit "Parts per million" ["ppm"] 1E6

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    0 // return an integer exit code
