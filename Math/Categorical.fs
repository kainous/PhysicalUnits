module System.Math.Categorical

open System.Collections.Generic
open System.Runtime.CompilerServices
open Microsoft.FSharp.Quotations

type IMorphism<'A, 'B> =
  abstract Map:Expr<'A -> 'B>

type IEndomorphism<'T> =
  inherit IMorphism<'T, 'T>

type IIsomorphism<'A, 'B> =
  abstract Forward:IMorphism<'A, 'B>
  abstract Inverse:IMorphism<'B, 'A>

type IAutomorphism<'T> =
  inherit IIsomorphism<'T, 'T>
  abstract Forward:IEndomorphism<'T>
  abstract Inverse:IEndomorphism<'T>

type IFixable<'T> =
  inherit IAutomorphism<'T>
  abstract FixedPoint:'T

//These are functions that, applied twice (or more), give the same value
type IIdempotent<'T> =
  inherit IFixable<'T>

//These are functions where Forward and Inverse are the same function
type IInvolution<'T> =
  inherit IAutomorphism<'T>

type private AutomorphismBase<'T>(forward, inverse) =
  interface IAutomorphism<'T> with
    member __.Forward = forward
    member __.Inverse = inverse
  interface IIsomorphism<'T, 'T> with
    member __.Forward = forward :> IMorphism<_, _>
    member __.Inverse = inverse :> IMorphism<_, _>

[<Extension>]
type Morphisms() =
  [<Extension>]
  static member Morphism f =
    { new IMorphism<_, _> with
        member __.Map = f }
  
  [<Extension>]
  static member Endomorphism f =
    { new IEndomorphism<_> with
        member __.Map = f }

  [<Extension>]
  static member inline Isomorphism(forward, inverse) =
    { new IIsomorphism<_,_> with 
        member __.Forward = forward
        member __.Inverse = inverse }

  [<Extension>]
  static member inline Isomorphism(forward, inverse) =
    Morphisms.Isomorphism(Morphisms.Morphism forward, Morphisms.Morphism inverse)

  [<Extension>]
  static member inline Automorphism(forward, inverse) = AutomorphismBase(forward, inverse) :> IAutomorphism<_>
  
  [<Extension>]
  static member inline Automorphism(forward, inverse) = AutomorphismBase(Morphisms.Endomorphism forward, Morphisms.Endomorphism inverse) :> IAutomorphism<_>

  //[<Extension>]
  //static member inline CachedMorphism(morphism:IMorphism<'TInput, _>, comparer:IEqualityComparer<'TInput>) =    
  //  let mutable cachedInput  = None
  //  let mutable cachedOutput = None

  //  Morphisms.Morphism <| fun input ->
  //    match cachedInput, cachedOutput with
  //    | _, None
  //    | None, _ -> 
  //      let output = morphism.Map input
  //      cachedOutput <- Some output
  //      output
  //    | Some a, _ when (a, input) |> comparer.Equals |> not -> 
  //      let output = morphism.Map input
  //      cachedOutput <- Some output
  //      output
  //    | _, Some b -> b
  
  //[<Extension>]
  //static member inline CachedMorphism(morphism:IMorphism<_,_>) = Morphisms.CachedMorphism(morphism, EqualityComparer<_>.Default)
  
  //[<Extension>]
  //static member inline CachedMorphism(f:_ -> _, comparer)      = Morphisms.CachedMorphism(Morphisms.Morphism f, comparer)
  
  //[<Extension>]
  //static member inline CachedMorphism f                        = Morphisms.CachedMorphism(Morphisms.Morphism f, EqualityComparer<_>.Default)

  [<Extension>]
  static member inline Compose(a:IMorphism<_, _>, b:IMorphism<_, _>) =
    Morphisms.Morphism <@ %a.Map >> %b.Map @>

  [<Extension>]
  static member inline Compose(a:IEndomorphism<_>, b:IEndomorphism<_>) =
    Morphisms.Endomorphism <@ %a.Map >> %b.Map @>

  [<Extension>]
  static member inline Compose(a:IIsomorphism<_, _>, b:IIsomorphism<_, _>) =
    Morphisms.Isomorphism(Morphisms.Compose(a.Forward, b.Forward), Morphisms.Compose(b.Inverse, a.Inverse))

  [<Extension>]
  static member inline Compose(a:IAutomorphism<_>, b:IAutomorphism<_>) =
    Morphisms.Automorphism(Morphisms.Compose(a.Forward, b.Forward), Morphisms.Compose(b.Inverse, a.Inverse))

