open System.IO

open Domain
open Server

[<EntryPoint>]
let main _ =
    let articlesDatasource = FileDatasource.listFilesAsync "src/articles"
    let repository = ArticleRepository.all articlesDatasource FileParser.parse
    let fromSlug slug = async {
        let! repository = repository
        return repository
               |> Seq.tryFind (fun x -> x.Meta.Slug = slug)
    }

    let staticDir = Path.Combine
                        (Directory.GetCurrentDirectory(), "src", "static")

    let articleSource () = repository |> Async.RunSynchronously
    let articleFetcher slug = fromSlug slug |> Async.RunSynchronously

    start staticDir (app articleSource articleFetcher)

    0 // return an integer exit code
