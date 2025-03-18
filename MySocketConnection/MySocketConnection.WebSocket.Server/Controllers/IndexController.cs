using System.Net;

namespace MySocketConnection.WebSocket.Server.Controllers;

public class IndexController
{
    public string HandleIndex(HttpListenerContext context)
    {
        return context.Request.HttpMethod switch
        {
            "GET" => OnGet(),
            "POST" => OnPost(context),
            _ => ""
        };
    }

    private string OnGet()
    {
        return "index.html";
    }

    private string OnPost(HttpListenerContext context)
    {
        return "index.html";
    }
}