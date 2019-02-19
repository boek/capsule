module Repository

open Domain.Article

type Repository = {
    All : unit -> seq<Article>
    FindBySlug : string -> Article option
}

module FileRepository =
    let create articleDatasource =
        let articles = articleDatasource
        let all() = articles
        let findBySlug slug =
            articles |> Seq.tryFind (fun article -> article.Meta.Slug = slug)

        {
            All = all
            FindBySlug = findBySlug
        }