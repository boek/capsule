
open Suave

[<EntryPoint>]
let main argv =
    let binding = HttpBinding.createSimple HTTP "0.0.0.0" 8081
    let config = { defaultConfig with bindings = [ binding ] }
    startWebServer config (Successful.OK "hello world")

    0 // return an integer exit code
