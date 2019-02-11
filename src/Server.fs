module Server

open Suave
open Suave.Filters
open Suave.Successful
open Suave.Operators
open Suave.RequestErrors

open Article
open Templates

let mapArticleToViewModel article =
    { Title = article.Meta.Title ; Date = article.Meta.Date.ToString() ; Content = article.Body }

module Routes =
    let rootRoute = path "/"
    let articleRoute = pathScan "/article/%s"


module Actions =
    let articleAction articleFinder slug =
        articleFinder slug
        |> Option.map (mapArticleToViewModel >> Templates.article)
        |> Option.map OK
        |> Option.orDefault (fun () -> NOT_FOUND "404")


let app articleFinder =
    choose [
        pathRegex @"/(.*)\.(css|png|gif|jpg|js|map|ico|svg)" >=> Files.browseHome
        GET >=> choose [
            Routes.rootRoute >=> OK(layout({ Title = "Hello World!"; Content = index }))
            Routes.articleRoute (Actions.articleAction articleFinder)
        ]
        NOT_FOUND "404"
    ]

let start homeFolder app =
    let binding = HttpBinding.createSimple HTTP "0.0.0.0" 8081
    let config = { defaultConfig with
                    bindings = [ binding ]
                    homeFolder = Some homeFolder
                 }
    startWebServer config app