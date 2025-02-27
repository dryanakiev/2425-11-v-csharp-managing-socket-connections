using System.Net;
using System.Text;
using System.Text.Json;
using MySocketConnection.WebSocket.Server.Models;

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
            
            string userIp = context.Request.RemoteEndPoint?.Address.ToString() ?? "Unknown IP";
            Console.WriteLine($"User {userIp} is visiting: {path}");

            if (path == "/")
            {
                string indexPath = Path.Combine(rootDirectory, "Pages/index.html");
                responseString = File.ReadAllText(indexPath);
                context.Response.ContentType = "text/html";
            }
            else if (path == "/about")
            {
                string aboutPath = Path.Combine(rootDirectory, "Pages/about.html");
                responseString = File.ReadAllText(aboutPath);
                context.Response.ContentType = "text/html";
            }
            else if (path == "/contact")
            {
                string contactPath = Path.Combine(rootDirectory, "Pages/contact.html");
                responseString = File.ReadAllText(contactPath);
                context.Response.ContentType = "text/html";
            }
            else if (path == "/register")
            {
                if (context.Request.HttpMethod == "GET")
                {
                    string registerPath = Path.Combine(rootDirectory, "Pages/register.html");
                    responseString = File.ReadAllText(registerPath);
                    context.Response.ContentType = "text/html";
                }
                else if (context.Request.HttpMethod == "POST")
                {
                    using (StreamReader reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        string formData = reader.ReadToEnd();
                        var parsedData = System.Web.HttpUtility.ParseQueryString(formData);
                        string email = parsedData["email"];
                        string password = parsedData["password"];
                        string passwordRepeat = parsedData["password-repeat"];

                        if (password == passwordRepeat)
                        {
                            WriteToJsonFile(new User()
                            {
                                Email = email,
                                Password = password,
                            });
                            
                            string returnPath = Path.Combine(rootDirectory, "Pages/index.html");
                            responseString = File.ReadAllText(returnPath);
                            context.Response.ContentType = "text/html";
                        }
                        else
                        {
                            string returnPath = Path.Combine(rootDirectory, "Pages/register.html");
                            responseString = File.ReadAllText(returnPath);
                            context.Response.ContentType = "text/html";
                        }
                        
                        
                    }
                }
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

        public async Task WriteToJsonFile(User user)
        {
            string fileName = "Users.json";
            if(!File.Exists(fileName)){
                await using FileStream createStream = File.Create(fileName);
                await JsonSerializer.SerializeAsync(createStream, user);
            }
            else
            {
                await using FileStream writeStream = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.None);
                await JsonSerializer.SerializeAsync(writeStream, user); // TODO: Fix object appending
            }
        }
        
        static void Main(string[] args)
        {
            WebServer server = new WebServer();
            server.Start();
        }
    }
}
