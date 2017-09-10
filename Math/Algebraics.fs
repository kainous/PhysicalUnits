module System.Math.Algebraics

open System.Math.Categorical
open System.Runtime.CompilerServices
open Microsoft.FSharp.Quotations

type IMagma<'T> =
  inherit IMorphism<'T, IEndomorphism<'T>>

//Implies a magma that is associative
type ISemigroup<'T> =
  inherit IMagma<'T>

type IUnitalMagma<'T> =
  inherit IMagma<'T>
  abstract IdentityElement:'T

type IMonoid<'T> =
  inherit IUnitalMagma<'T>
  inherit ISemigroup<'T>

type IQuasigroup<'T> =
  inherit IMagma<'T>
  abstract InverseElement:IMorphism<'T, 'T option>

type ILoop<'T> =
  inherit IQuasigroup<'T>
  inherit IUnitalMagma<'T>

type IGroup<'T> =
  inherit ILoop<'T>
  inherit IMonoid<'T>

type ITransformationGroup<'T, 'TElement> =
  abstract Compose : IGroup<'T>
  abstract Transform : IMorphism<'T, IEndomorphism<'TElement>>

type IAbelianGroup<'T> =
  inherit IGroup<'T>

//Requires that multiplication distributes over addition
type IField<'T> =
  abstract Addition     :IAbelianGroup<'T>
  abstract Multiplication:IGroup<'T>

type IModule<'T, 'TField, 'TElement when 'TField :> IField<'TElement>> =
  abstract ScalarMultiplication : IMorphism<'TElement, IEndomorphism<'T>>

type IExponentialField<'T> =
  inherit IField<'T>
  abstract Exponentiation:IMagma<'T>

type IVectorSpace<'T, 'TField, 'TElement when 'TField :> IField<'TElement>> =
  abstract ScalarField:'TField
  abstract Module:IModule<'T, 'TField, 'TElement>
  abstract Group:IAbelianGroup<'T>

type private TransformationGroup<'T, 'TElement>(transform:IMorphism<'T, IEndomorphism<'TElement>>, compose:IGroup<'T>) =
  interface ITransformationGroup<'T, 'TElement> with
    member __.Transform = transform
    member __.Compose   = compose

type private Magma<'T>(operation : Expr<'T -> 'T -> 'T>) =
  interface IMagma<'T> with
    member __.Map = <@ fun x -> Morphisms.Endomorphism <@ (%operation) x @> @>

type private Group<'T>(operation:IMagma<_>, negation, identity) =
  interface IGroup<'T> with
    member __.InverseElement = negation
  interface IMagma<'T> with
    member __.Map = operation.Map
  interface IUnitalMagma<'T> with
    member __.IdentityElement = identity

type private AbelianGroup<'T>(transformation:IMagma<_>, negation, identity) =
  interface IAbelianGroup<'T> with
    member __.InverseElement = negation
  interface IMagma<'T> with
    member __.Map = transformation.Map
  interface IUnitalMagma<'T> with
    member __.IdentityElement = identity

type private Field<'T>(addition, multiplication) =
  interface IField<'T> with
    member __.Addition       = addition
    member __.Multiplication = multiplication

type private ExponentialField<'T>(addition, multiplication, exponentiation) =
  interface IExponentialField<'T> with
    member __.Addition       = addition
    member __.Multiplication = multiplication
    member __.Exponentiation = exponentiation   

type private VectorSpace<'T, 'TField, 'TElement when 'TField :> IField<'TElement>>(scalarField, vectorAddition, scalarMultiplication) =
  interface IVectorSpace<'T, 'TField, 'TElement> with
    member __.ScalarField = scalarField
    member __.Module = scalarMultiplication
    member __.Group = vectorAddition    

type private Module<'T, 'TField, 'TElement when 'TField :> IField<'TElement>>(scalarMultiplication) =
  interface IModule<'T, 'TField, 'TElement> with
    member __.ScalarMultiplication = Morphisms.Morphism <@ fun s -> Morphisms.Endomorphism <@ fun t -> (%scalarMultiplication) s t @> @>

[<Extension>]
type Groups() =
  [<Extension>]
  static member TransformationGroup(transform, compose) = 
    (transform, compose) 
    |> TransformationGroup
    :> ITransformationGroup<_, _>

  static member TransformationGroup(transform:Expr<'T -> 'TElement -> 'TElement>, compose:Expr<'T -> 'T -> 'T>, inverse:Expr<'T -> 'T option>, identity:'T) =
    ( Morphisms.Morphism <@ fun t -> <@ (%transform) t @> |> Morphisms.Endomorphism  @>
    , Groups.Group(compose, inverse, identity))
    |> Groups.TransformationGroup

  static member Magma operation =
    operation
    |> Magma
    :> IMagma<_>

  [<Extension>]
  static member Group(operation:IMagma<_>, negation, identity) = 
    Group(operation, negation, identity) 
    :> IGroup<_>
  
  [<Extension>]
  static member Group(operation:Expr<'T -> 'T -> 'T>, negation : Expr<'T -> 'T option>, identity : 'T) =
    Groups.Group
      ( Groups.Magma operation
      , Morphisms.Morphism negation
      , identity )

  [<Extension>]
  static member AbelianGroup(operation, negation, identity) = 
    AbelianGroup<_>(operation, negation, identity) 
    :> IAbelianGroup<_>
  
  [<Extension>]
  static member AbelianGroup(operation:Expr<'T -> 'T -> 'T>, negation : Expr<'T -> 'T option>, identity : 'T) =
    Groups.AbelianGroup
      ( Groups.Magma operation
      , Morphisms.Morphism negation
      , identity )

  [<Extension>]
  static member Field(addition, multiplication) =
    Field(addition, multiplication)
    :> IField<_>

  [<Extension>]
  static member Field(zero:'T, one:'T, addition:Expr<'T -> 'T -> 'T>, negation, multiplication:Expr<'T -> 'T -> 'T>, inverseElement) =
    Field
      ( Groups.AbelianGroup(addition, negation, zero)
      , Groups.Group(multiplication, inverseElement, one))
    :> IField<_>

  [<Extension>]
  static member ExponentialField(addition, multiplication, exponentiation) =
    ExponentialField(addition, multiplication, exponentiation)
    :> IExponentialField<_>

  [<Extension>]
  static member ExponentialField(zero:'T, one:'T, addition:Expr<'T -> 'T -> 'T>, negation:Expr<'T -> 'T option>, multiplication:Expr<'T -> 'T -> 'T>, inverseElement, exponentiation:Expr<'T -> 'T -> 'T>) =
    ExponentialField
      ( Groups.AbelianGroup(addition, negation, zero)
      , Groups.Group(multiplication, inverseElement, one)
      , Groups.Magma exponentiation )
    :> IExponentialField<_>

  [<Extension>]
  static member VectorSpace(scalarField, vectorAddition, scalarMultiplication) =
    VectorSpace(scalarField, vectorAddition, scalarMultiplication)
    :> IVectorSpace<_, _, _>

  [<Extension>]
  static member Module scalarMultiplication =
    Module(scalarMultiplication)
    :> IModule<_, _, _>

  [<Extension>]
  static member VectorSpace(zero, addition, negation:Expr<_ -> _ option>, field, scalarMultiplication) =
    VectorSpace
      ( field
      , Groups.AbelianGroup(addition, negation, zero)
      , Groups.Module scalarMultiplication )
    :> IVectorSpace<_, _, _>

type AutomorphismGroup<'T>() =
  let magma = Groups.Magma <@ fun (a:IAutomorphism<'T>) b -> Morphisms.Compose(a, b) @>
  let inverse = Morphisms.Morphism <@ fun (a:IAutomorphism<'T>) -> Morphisms.Automorphism (a.Inverse, a.Forward) |> Some @>
  let identity = Morphisms.Automorphism(<@ id @>, <@ id @>)
  
  interface IGroup<IAutomorphism<'T>> with
    member __.InverseElement = inverse
  interface IMagma<IAutomorphism<'T>> with
    member __.Map = magma.Map
  interface IUnitalMagma<IAutomorphism<'T>> with
    member __.IdentityElement = identity