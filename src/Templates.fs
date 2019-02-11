module Templates
open Suave.DotLiquid
open DotLiquid


type LayoutViewModel = {
    title : string
    content : string
}

setTemplatesDir "./src/templates"

let index = page "index.liquid" None

type PostViewModel = { Title : string; Date : string; Content : string }
let article (model : PostViewModel) = page "article.liquid" model
