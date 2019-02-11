open System.IO

open Article
open Core.Config
open Server

[<EntryPoint>]
let main _ =
    let articleSource = FileSystemProvider
                            .provider("src/articles") |> findArticle

    let staticDir = Path.Combine
                        (Directory.GetCurrentDirectory(), "src", "static")

    start { defaultConfig with StaticDir = Some staticDir } (app articleSource)

    0 // return an integer exit code
