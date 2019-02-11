module ArticleTests

open Article
open Xunit
open System

[<Fact>]
let ``Test that we can parse an article`` () =
    let mockFile = String.concat("\n") <| [
        "{"
        """"title": "Title","""
        """"slug": "Slug","""
        """"author": "Author","""
        """"date": "12/08/2012","""
        """"tags": ["tag"]"""
        "}"
        ""
        "Hello World!"
        "Hello World!"
        ""
        ""
        "Hello World!"
    ]

    let expected = {
        Meta = {
            Title = "Title"
            Slug = "Slug"
            Author = "Author"
            Date = DateTime.Parse("12/08/2012")
            Tags = ["tag"]
        }
        Body = "Hello World!\nHello World!\n\n\nHello World!" }

    let actual = mockFile |> Article.parse
    Assert.True(actual.Value = expected)
