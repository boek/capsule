open System
open System.IO
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http

let configureApp (app: IApplicationBuilder) =
    app.Run (fun ctx ->
        printfn "%A - %A - %A - %A" ctx.Request.Method ctx.Request.PathBase ctx.Request.Path ctx.Request.QueryString
        sprintf "Hello world" |> ctx.Response.WriteAsync  
)

[<EntryPoint>]
let main argv =
    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .Configure(Action<IApplicationBuilder> configureApp)
        .Build()
        .Run()

    0 // return an integer exit code

