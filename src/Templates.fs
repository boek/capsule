module Templates
open Suave.DotLiquid
open DotLiquid


type LayoutViewModel = {
    title : string
    content : string
}

setTemplatesDir "./src/templates"

type ArticleBrief = { Title : string ; Date : string; Summary : string}
type IndexViewModel = { Articles : ArticleBrief list }
let index = page "index.liquid" None

type ArticleViewModel = { Title : string; Date : string; Content : string }
let article (model : ArticleViewModel) = page "article.liquid" model
