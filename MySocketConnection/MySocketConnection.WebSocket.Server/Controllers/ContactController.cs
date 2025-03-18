using System.Net;

namespace MySocketConnection.WebSocket.Server.Controllers;

public class ContactController
{
    public string HandleContact(HttpListenerContext context)
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
        return "contact.html";
    }

    private string OnPost(HttpListenerContext context)
    {
        return "contact.html";
    }
}