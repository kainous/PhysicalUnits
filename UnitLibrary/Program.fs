open System

//I can either forbid addition by well-typing all things, or make multiplication 
type Quantity = { Value:float; Units: decimal list}
with
  static member (+) (a : Quantity, b : Quantity) =
    match a, b with
    | { Units = a' }, { Units = b' } when a' <> b' -> ArgumentException("Both quantities must be of the same units", "b") |> raise
    | _ -> { Value = a.Value + b.Value; Units = a.Units }

    


type IEquivalenceRelation<'T> =
  abstract Equals : 'T -> 'T -> bool

type ITotalOrder<'T> =
  abstract Relation : 'T -> 'T -> bool

type IMagma<'T> =
  abstract Combine : 'T -> 'T -> 'T

type IQuasigroup<'T> =
  inherit IMagma<'T>
  inherit ITotalOrder<'T>
  abstract Inverse : 'T -> 'T

//Rule includes associativity
type ISemigroup<'T> =
  inherit IMagma<'T>
  inherit ITotalOrder<'T>

type ILoop<'T> =
  inherit IQuasigroup<'T>
  abstract Identity : 'T

type IMonoid<'T> =
  inherit ISemigroup<'T>
  abstract Identity : 'T

type IGroup<'T> =
  inherit IMonoid<'T>
  inherit ILoop<'T>

type IAbelianGroup<'T> =
  inherit IGroup<'T>

type IRing<'T> =
  abstract Addition : IGroup<'T>
  abstract Multiplication : IGroup<'T>

type IExponentialRing<'T> =
  abstract Power : IMagma<'T>

type DoubleRing =
  interface IExponentialRing<double> with
    member this.Power =
      { new IMagma<double> with
          member this.Combine a b = a ** b }

    member this.Addition =
      { new IGroup<double> with
          member __.Combine a b = a + b
          member __.Identity with get() = 0.0
          member __.Inverse a = 1.0 / a 
          member __.Relation a b = (a = b),
        new ILoop<double> with
          member }

type 'T RingExpr =
| Add of 'T GroupExpr
| Mul of 'T GroupExpr
| Exp of 'T RingExpr
| Ln  of 'T RingExpr
| Sin of 'T RingExpr
| Cos of 'T RingExpr
| DiffVar
| Const of 'T
and 'T GroupExpr =
| Combine of 'T RingExpr * 'T RingExpr
| InverseElement of 'T RingExpr
| Identity

let (+) a b = Add(Combine(a, b))
let (-) a b = Add(Combine(a, Add(InverseElement b)))
let (~-) a  = Add(InverseElement a)
let (*) a b = Mul(Combine(a, b))
let tan a   = Mul(Combine((Sin a), Mul(InverseElement (Cos a))))

let b_0 = Const 3.2
let b_1 = Const 2.5
let b_2 = Const 1.2
let f = Exp (b_0 * Sin DiffVar) - b_1 + b_2 * (Sin DiffVar) * (tan DiffVar)

let rec diff = function
| Const _
| Add(Identity)
| Mul(Identity)         -> Add(Identity)
| Add(Combine(a, b))    -> (diff a) + (diff b)
| Add(InverseElement a) -> -(diff a)
| Mul(Combine(a, b))    -> (diff a) * b + a * (diff b)
| Mul(InverseElement f) -> -(diff f) * Mul(InverseElement(f * f))
| DiffVar               -> Mul(Identity)
| Cos a                 -> (diff a)*(-Sin a)
| Sin a                 -> (diff a)*( Cos a)
| Exp f                 -> (diff f)*( Exp f)
| Ln  f                 -> (diff f) * Mul(InverseElement(f))



[<EntryPoint>]
let main argv = 
  printfn "%A" argv
  0 // return an integer exit code
