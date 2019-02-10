module Article
open System.Text.RegularExpressions

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
        "title": "Hello World!",
        "slug": "foo",
        "author": "Jeff Boek",
        "date": "12/08/2012",
        "tags": ["misc"]
    }
     """>

    let parse input =
        let value = MetaParser.Parse input
        { Title = value.Title;
            Author = value.Author;
            Date = value.Date;
            Slug = value.Slug;
            Tags = (value.Tags |> Array.toList) }


let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None

type Article = { 
    Meta : Meta
    Body : string
}

type ArticleProvider = unit -> Article list
type ArticleParser = string -> Article option


module FileSystemProvider =
    open System
    open System.IO

    let parse path = async {
        return match (File.ReadAllText(path).Split("\n\n", 2)) with
               | [|meta; body|] -> Some({ Meta = Meta.parse meta; Body = body.ToString() })
               | _ -> None
    }

    let provider articlesDirectory () =
        Directory.GetFiles articlesDirectory
        |> Seq.map parse        
        |> Async.Parallel
        |> Async.RunSynchronously
        |> Seq.toList
        |> Seq.choose id

let findArticle provider slug =
    provider() |> Seq.tryFind (fun a -> a.Meta.Slug = slug)
    
