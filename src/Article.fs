module Article

open System
open System.IO

type Article = { 
    Title : string
    Slug : string
    Created : DateTime
    Body : string
}

type ArticleProvider = unit -> Article list

let parseFileToArticle path = async {
    let content = File.ReadAllText(path)     
    let foo = Path.GetFileName(path) 
    printfn "path: %s" foo    

    return {
        Title = "Foo";
        Slug = Path.GetFileName(path);
        Created = DateTime.Now;
        Body = content.ToString() }
}    

let fileSystemArticleProvider articlesDirectory () =
    Directory.GetFiles articlesDirectory
    |> Seq.map parseFileToArticle
    |> Async.Parallel
    |> Async.RunSynchronously
    |> Seq.toList




let mockArticleProvider () =
    [ { Title = "MyTitle"; Slug = "foo"; Created = DateTime.Now; Body = "Article Body" } ]


let findArticle provider slug =
    provider() |> Seq.tryFind (fun a -> a.Slug = slug)
    
