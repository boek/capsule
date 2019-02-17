module Server

open Suave
open Suave.Filters
open Suave.Operators
open Suave.RequestErrors
open Suave.DotLiquid

open Domain.Article
open Templates

let mapArticleToViewModel article =
    { Title = article.Meta.Title ; Date = article.Meta.Date.ToString() ; Content = article.Body }

module Routes =
    let rootRoute = path "/"
    let articleRoute = pathScan "/article/%s"


type ArticleDatasource = (unit -> seq<Article>)
module Actions =
    let index (articleDatasource : ArticleDatasource) =
        articleDatasource()
        |> Seq.map (fun x -> { Title = x.Meta.Title; Date = x.Meta.Date.ToString(); Summary = x.Body })
        |> Seq.toList
        |> (fun x -> { Articles = x })
        |> Templates.index


    let articleAction articleFetcher slug =
        articleFetcher slug
        |> Option.map (mapArticleToViewModel >> Templates.article)
        |> Option.orDefault (fun () -> NOT_FOUND "404")

let app (articleDatasource : ArticleDatasource) articleFetcher =
    choose [
        pathRegex @"/(.*)\.(css|png|gif|jpg|js|map|ico|svg)" >=> Files.browseHome
        GET >=> choose [
            Routes.rootRoute >=> (Actions.index articleDatasource)
            Routes.articleRoute (Actions.articleAction articleFetcher)
        ]
        NOT_FOUND "404"
    ]

let start homeFolder app =
    let binding = HttpBinding.createSimple HTTP "0.0.0.0" 8088
    setTemplatesDir "./src/templates"

    let config = { defaultConfig with
                    bindings = [ binding ]
                    homeFolder = Some homeFolder
                 }

    startWebServer config app