open System.IO

open MarkdownSharp
open Suave
open Suave.DotLiquid

open Api
open Domain
open Repository


[<EntryPoint>]
let main _ =
    let markdown = Markdown()
    let app =
        FileDatasource.listFilesAsync "src/articles"
        |> Async.map (Seq.map FileParser.parse
                        >> Seq.choose id
                        >> Seq.map (fun x -> { x with Body = markdown.Transform x.Body }))
        |> Async.map FileRepository.create
        |> Async.map api
        |> Async.RunSynchronously

    let staticDir = Path.Combine
                        (Directory.GetCurrentDirectory(), "src", "static")

    let binding = HttpBinding.createSimple HTTP "0.0.0.0" 8088
    setTemplatesDir "./src/templates"

    let config = { defaultConfig with
                    bindings = [ binding ]
                    homeFolder = Some staticDir
                 }

    startWebServer config app

    0 // return an integer exit code
