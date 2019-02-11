module Templates

open Suave.DotLiquid

type ArticleBrief = { Title : string ; Date : string; Summary : string}
type IndexViewModel = { Articles : ArticleBrief list }
let index (model : IndexViewModel) = page "index.liquid" model

type ArticleViewModel = { Title : string; Date : string; Content : string }
let article (model : ArticleViewModel) = page "article.liquid" model
