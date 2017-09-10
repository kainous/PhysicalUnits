module Transforms

open System.Math.Algebraics

type MobiusTransform(a:float, b:float, c:float, d:float) =
  let compose = <@ fun (p:MobiusTransform) (q:MobiusTransform) ->
    MobiusTransform
      ( p.A * q.A + p.B * q.C
      , p.A * q.B + p.B * q.D
      , p.C * q.A + p.D * q.C
      , p.C * q.B + p.D * q.D ) @>

  let inverse = <@ fun (p:MobiusTransform) -> 
    MobiusTransform(p.D, -p.B, -p.C, p.A) 
    |> Some @>
 
  let identity = MobiusTransform(1.0, 0.0, 0.0, 1.0)
  
  let group = 
    Groups.TransformationGroup
      ( <@ fun (m:MobiusTransform) (x:float) -> (m.A * x + m.B) / (m.C * x + m.D) @>
      , compose
      , inverse
      , identity)
  
  member __.A = a
  member __.B = b
  member __.C = c
  member __.D = d

  static member Compose(p:MobiusTransform, q:MobiusTransform) =
    MobiusTransform
      ( p.A * q.A + p.B * q.C
      , p.A * q.B + p.B * q.D
      , p.C * q.A + p.D * q.C
      , p.C * q.B + p.D * q.D )


  interface ITransformationGroup<MobiusTransform, float> with
    member __.Transform = group.Transform
    member __.Compose   = group.Compose