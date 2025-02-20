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
            UrlPrefix = "http://localhost:9999/";
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
            string rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            
            string userIP = context.Request.RemoteEndPoint?.Address.ToString() ?? "Unknown IP";
            Console.WriteLine($"User {userIP} is visiting: {path}");

            if (path == "/")
            {
                string indexPath = Path.Combine(rootDirectory, "index.html");
                responseString = File.ReadAllText(indexPath);
                context.Response.ContentType = "text/html";
            }
            else if (path == "/about")
            {
                string aboutPath = Path.Combine(rootDirectory, "about.html");
                responseString = File.ReadAllText(aboutPath);
                context.Response.ContentType = "text/html";
            }
            else if (path == "/contact")
            {
                string contactPath = Path.Combine(rootDirectory, "contact.html");
                responseString = File.ReadAllText(contactPath);
                context.Response.ContentType = "text/html";
            }
            else if (path.StartsWith("/css/"))
            {
                string cssFilePath = Path.Combine(rootDirectory, path.TrimStart('/'));
                responseString = File.ReadAllText(cssFilePath);
                context.Response.ContentType = "text/css";
            }
            else
            {
                responseString = "<html><body><img src=\"https://i0.wp.com/wasmormon.org/wp-content/uploads/2023/04/image.png?resize=498%2C256&ssl=1\"></img></body></html>";
                context.Response.ContentType = "text/html";
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
