module Server

open Suave
open Suave.Filters
open Suave.Successful
open Suave.Operators
open Suave.RequestErrors

open Core
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

let start config app =
    let (Host host, Port port) = (config.Host, config.Port)
    let binding = HttpBinding.createSimple HTTP host port
    let config = { defaultConfig with
                    bindings = [ binding ]
                    homeFolder = config.StaticDir
                 }

    startWebServer config app