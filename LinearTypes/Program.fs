type Expr =
| Lambda   of Expr * Expr
| Apply    of Expr * Expr
| Bang     of Expr
| CaseBang of Expr * Expr * Expr
| Tuple    of Expr * Expr
| CaseTuple of Expr * Expr * Expr * Expr * Expr

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    0 // return an integer exit code
