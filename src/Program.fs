open System.IO

open Article
open Server

[<EntryPoint>]
let main _ =
    let homeFolder = Path.Combine(Directory.GetCurrentDirectory(), "src", "static")
    let articleSource = FileSystemProvider.provider("src/articles") |> findArticle
    start homeFolder (app articleSource)

    0 // return an integer exit code
