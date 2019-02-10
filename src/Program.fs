
open Suave
open Suave.Filters
open Suave.Successful
open Suave.Operators
open Suave.RequestErrors

open Templates
open Article

let rootRoute = path "/"
let articleRoute = pathScan "/article/%s" 
let pageRoute = pathScan "/%s" 

let renderArticle article =
    let template = sprintf """
    title: %s <br />
    created at: %s <br />
    body: %s <br />
    """

    template article.Title (article.Created.ToString()) article.Body

let articleAction slug =
    let response = findArticle (fileSystemArticleProvider "src/articles") slug
                    |> Option.map renderArticle
                    |> Option.map OK

    defaultArg response (NOT_FOUND "404")

let app =
    choose [
        GET >=> choose [
            rootRoute >=> OK(layout({ Title = "Hello World!"; Content = index }))
            articleRoute articleAction
            pageRoute ((sprintf "Page: %s") >> OK)
        ] 
        path ""
        NOT_FOUND "404"
    ]

[<EntryPoint>]
let main argv =
    let binding = HttpBinding.createSimple HTTP "0.0.0.0" 8081
    let config = { defaultConfig with bindings = [ binding ] }
    startWebServer config app

    0 // return an integer exit code
