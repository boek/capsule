
open Suave
open Suave.Filters
open Suave.Successful
open Suave.Operators
open Suave.RequestErrors


let app =
    choose [
        path "/" >=> (OK "/")
        NOT_FOUND "404"
    ]

[<EntryPoint>]
let main argv =
    let binding = HttpBinding.createSimple HTTP "0.0.0.0" 8081
    let config = { defaultConfig with bindings = [ binding ] }
    startWebServer config app

    0 // return an integer exit code
