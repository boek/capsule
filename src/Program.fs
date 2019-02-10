
open Suave
open Suave.Filters
open Suave.Successful
open Suave.Operators
open Suave.RequestErrors

open Templates
open Article
open Article.FileSystemProvider

let rootRoute = path "/"
let articleRoute = pathScan "/article/%s" 
let pageRoute = pathScan "/%s" 

let renderArticle article =
    Templates.article { Title = article.Meta.Title ; Date = article.Meta.Date.ToString() ; Content = article.Body }

let articleAction slug =
    let response = findArticle (provider "src/articles") slug
                    |> Option.map renderArticle
                    |> Option.map OK

    defaultArg response (NOT_FOUND "404")

let app =
    choose [
        pathRegex @"/(.*)\.(css|png|gif|jpg|js|map|ico|svg)" >=> Files.browseHome
        GET >=> choose [            
            rootRoute >=> OK(layout({ Title = "Hello World!"; Content = index }))
            articleRoute articleAction            
        ]         
        NOT_FOUND "404"
    ]

open System.IO
let myHomeFolder = Path.Combine(Directory.GetCurrentDirectory(), "src", "static")


[<EntryPoint>]
let main argv =
    let binding = HttpBinding.createSimple HTTP "0.0.0.0" 8081
    printfn "%s" myHomeFolder
    let config = { defaultConfig with
                    bindings = [ binding ]
                    homeFolder = Some myHomeFolder
                 }
    startWebServer config app

    0 // return an integer exit code
