module Article

open System.Text.RegularExpressions
let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None

type Meta = {
    Title : string
    Author : string
    Slug : string
    Date : System.DateTime
    Tags : string list
}

module Meta =
    open FSharp.Data

    type MetaParser = JsonProvider<"""
    {
        "title": "Title",
        "slug": "Slug",
        "author": "Author",
        "date": "12/08/2012",
        "tags": ["tag"]
    }
     """>

    let transformToMeta (meta : MetaParser.Root) = { Title = meta.Title;
            Author = meta.Author;
            Date = meta.Date;
            Slug = meta.Slug;
            Tags = (meta.Tags |> Array.toList) }


    let parse =  MetaParser.Parse >> transformToMeta

type Article = {
    Meta : Meta
    Body : string
}

module Article =
    let parse (input : string) =
        match input.Split("\n\n") with
        | [|meta; body|] -> Some({ Meta = Meta.parse meta; Body = body.ToString() })
        | _ -> None

module FileSystemProvider =
    open System.IO
    open Article

    let openFile path = async {
        return File.ReadAllText(path)
    }

    let provider articlesDirectory () =
        Directory.GetFiles articlesDirectory
        |> Seq.map (openFile >> Async.map parse)
        |> Async.Parallel
        |> Async.RunSynchronously
        |> Seq.toList
        |> Seq.choose id

let findArticle provider slug =
    provider() |> Seq.tryFind (fun a -> a.Meta.Slug = slug)

