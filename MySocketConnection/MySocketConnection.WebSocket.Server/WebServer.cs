using System.Net;
using MySocketConnection.WebSocket.Server.Controllers;

namespace MySocketConnection.WebSocket.Server
{
    public class WebServer
    {
        public HttpListener Listener { get; }
        public string UrlPrefix { get; }
        private readonly WebController _controller;

        public WebServer()
        {
            UrlPrefix = "http://localhost:9999/";
            Listener = new HttpListener();
            Listener.Prefixes.Add(UrlPrefix);
            Listener.Start();
            _controller = new WebController();
            Console.WriteLine($"HTTP Server started on {UrlPrefix}");
        }

        public void Start()
        {
            while (true)
            {
                HttpListenerContext context = Listener.GetContext();
                _controller.HandleRequest(context);
            }
        }
    }
}
