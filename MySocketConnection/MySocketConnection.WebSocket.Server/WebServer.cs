using System.Net;
using System.Text;

namespace MySocketConnection.WebSocket.Server
{
    public class WebServer
    {
        public HttpListener Listener { get; }
        public string UrlPrefix { get; }

        public WebServer()
        {
            UrlPrefix = "http://localhost:5000/";
            Listener = new HttpListener();
            Listener.Prefixes.Add(UrlPrefix);
            Listener.Start();
            Console.WriteLine($"HTTP Server started on {UrlPrefix}");
        }

        public void Start()
        {
            while (true)
            {
                HttpListenerContext context = Listener.GetContext();
                ProcessRequest(context);
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            string path = context.Request.Url.AbsolutePath;
            string responseString = " ";

            if (path == "/")
            {
                responseString = "<html><body><h2>Welcome to the Web Server!</h2></body></html>";
            }
            else
            {
                responseString = "<html><body><h2>404 - Not Found</h2></body></html>";
            }



            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }

        static void Main(string[] args)
        {
            WebServer server = new WebServer();
            server.Start();
        }
    }
}
