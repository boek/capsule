open System.IO

open Article
open Core.Config
open Server

[<EntryPoint>]
let main _ =
    let articleSource = FileSystemProvider.provider("src/articles")
    let articleFetcher = articleSource |> findArticle

    let staticDir = Path.Combine
                        (Directory.GetCurrentDirectory(), "src", "static")

    start { defaultConfig with StaticDir = Some staticDir } (app articleSource articleFetcher)

    0 // return an integer exit code
