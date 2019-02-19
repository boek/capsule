module Api

open Suave
open Suave.Filters
open Suave.Operators
open Suave.RequestErrors

open Domain.Article
open Repository

module ArticlesController =
    open Templates

    let mapToBrief article =
        { Title = article.Meta.Title; Date = article.Meta.Date.ToString(); Summary = article.Body }

    let mapToArticleViewModel article =
        { Title = article.Meta.Title ; Date = article.Meta.Date.ToString() ; Content = article.Body }

    let index repository =
        repository.All()
        |> Seq.map mapToBrief
        |> Seq.toList
        |> IndexViewModel.create
        |> Templates.index

    let article repository slug =
        repository.FindBySlug(slug)
        |> Option.map (mapToArticleViewModel >> Templates.article)
        |> Option.orDefault (fun () -> NOT_FOUND "404")



let api repository =
    GET >=> choose [
        pathRegex @"/(.*)\.(css|png|gif|jpg|js|map|ico|svg)" >=> Files.browseHome
        path "/" >=> (ArticlesController.index repository)
        pathScan "/article/%s" (ArticlesController.article repository)
        NOT_FOUND "404"
    ]