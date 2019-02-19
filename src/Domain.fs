module Domain

module Config =
    type Port = Port of int
    type Host = Host of string

    type Config = {
        StaticDir : string option
        Host : Host
        Port : Port
    }

    let defaultConfig = {
        StaticDir = None
        Host = Host "0.0.0.0"
        Port = Port 8081
    }

module Article =
    type Meta = {
        Title : string
        Author : string
        Slug : string
        Date : System.DateTime
        Tags : string list
    }

    type Article = {
        Meta : Meta
        Body : string
    }

module File =
    open System.IO

    type File = { FileName : string ; Body : string }

    let loadAsync file = async {
        return { FileName = file ; Body =  File.ReadAllText(file) }
    }

module JsonParser =
    open FSharp.Data
    open Article

    type private MetaParser = JsonProvider<"""
    {
        "title": "Title",
        "slug": "Slug",
        "author": "Author",
        "date": "12/08/2012",
        "tags": ["tag"]
    }
    """>

    let private metaMapper (parsedMeta : MetaParser.Root) =
        { Title = parsedMeta.Title;
          Author = parsedMeta.Author;
          Date = parsedMeta.Date;
          Slug = parsedMeta.Slug;
          Tags = (parsedMeta.Tags |> Array.toList) }

    let parseMeta = MetaParser.Parse >> metaMapper

module FileParser =
    open Article
    open File
    open JsonParser

    let parse { FileName = _ ; Body = body } =
        match body.Split("\n\n", 2) with
        | [|meta; body|] -> Some({ Meta = JsonParser.parseMeta meta; Body = body })
        | _ -> None


module FileDatasource =
    open File
    open System.IO

    let listFilesAsync path = async {
        return! path
        |> Directory.GetFiles
        |> Array.map loadAsync
        |> Async.Parallel
        |> Async.map Array.toSeq
    }