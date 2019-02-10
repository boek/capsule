module Templates


type LayoutViewModel = {
    Title : string
    Content : string
}

let layout viewModel =
    let template = sprintf """
    <!doctype html>
    <html>
      <head>
        <link rel="stylesheet" type="text/css" href="/main.css">
        <link rel="alternate" type="application/atom+xml" title="%s - feed" href="/index.xml" />
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/> 
        <title>%s</title>
      </head>
      <body>
        <header class="cf">
          <h3><a href="/">Jeff Boek</a></h3>
          <nav>
            <!--<a href="/about/">About</a> |-->
            <a href="/archives">Archives</a> |
            <a href="http://twitter.com/jeffboek">Twitter</a>
          </nav>
        </header> 
        <section>
          %s
        </section>
        <footer>
          powered by <a href="#Capsule">capsule</a>
        </footer>
      </body>
    </html>
    """
    template viewModel.Title viewModel.Title viewModel.Content


let index =
    let template = """
    <section id="articles">      
        <article class="post">
          <header>
            <h1><a href="#">Article Title</a></h1>
            <span class="date">Article Date</span>
          </header>

          <section class="content">
            Summary
          </section>
          <div class="more"><a href="#">read on &raquo;</a></div>
        </article>      
    </section>

    <section id="archives">
        ...      
    </section>
    """
    template


type PostViewModel = { Title : string; Date : string; Content : string }
let article viewModel =
    let template = sprintf """
    <article class="post">
      <header>
        <h1>%s</h1>
        <span class="date">%s</span>
      </header>

      <section class="content">
        %s
      </section>      
    </article>
    """
    
    layout {
        Title = viewModel.Title
        Content = (template viewModel.Title viewModel.Date viewModel.Content)
    }
