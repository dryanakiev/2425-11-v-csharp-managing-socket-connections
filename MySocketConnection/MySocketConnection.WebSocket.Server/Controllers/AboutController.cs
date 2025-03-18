using System.Net;

namespace MySocketConnection.WebSocket.Server.Controllers;

public class AboutController
{
    public string HandleAbout(HttpListenerContext context)
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
        return "about.html";
    }

    private string OnPost(HttpListenerContext context)
    {
        return "about.html";
    }
}