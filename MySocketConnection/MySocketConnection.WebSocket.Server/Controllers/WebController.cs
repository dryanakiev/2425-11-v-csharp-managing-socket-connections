using System.Net;
using System.Text;

namespace MySocketConnection.WebSocket.Server.Controllers
{
    public class WebController
    {
        private readonly string _rootDirectory;
        private readonly RegisterController _registerController;
        private readonly IndexController _indexController;
        private readonly AboutController _aboutController;
        private readonly ContactController _contactController;

        public WebController()
        {
            _rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            _registerController = new RegisterController();
            _indexController = new IndexController();
            _aboutController = new AboutController();
            _contactController = new ContactController();
        }

        public void HandleRequest(HttpListenerContext context)
        {
            string path = context.Request.Url.AbsolutePath;
            
            string responseString = " ";
            
            string userIp = context.Request.RemoteEndPoint?.Address.ToString() ?? "Unknown IP";
            
            Console.WriteLine($"User {userIp} is visiting: {path}");

            switch (path)
            {
                case "/":
                    responseString = GetPage(_indexController.HandleIndex(context));
                    break;
                case "/about":
                    responseString = GetPage(_aboutController.HandleAbout(context));
                    break;
                case "/contact":
                    responseString = GetPage(_contactController.HandleContact(context));
                    break;
                case "/register":
                    responseString = GetPage(_registerController.HandleRegister(context));
                    break;
                default:
                {
                    if (path.StartsWith("/css/"))
                        responseString = GetCssFile(path);
                    else
                        responseString = "<html><body><img src=\"https://i0.wp.com/wasmormon.org/wp-content/uploads/2023/04/image.png?resize=498%2C256&ssl=1\"></img></body></html>";
                    break;
                }
            }

            SendResponse(context, responseString);
        }

        private string GetPage(string fileName)
        {
            string filePath = Path.Combine(_rootDirectory, "Pages", fileName);
            
            return File.Exists(filePath) ? File.ReadAllText(filePath) : "404 - Page Not Found";
        }

        private string GetCssFile(string path)
        {
            string cssFilePath = Path.Combine(_rootDirectory, path.TrimStart('/'));
            
            return File.Exists(cssFilePath) ? File.ReadAllText(cssFilePath) : "";
        }

        private void SendResponse(HttpListenerContext context, string responseString)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }
    }
}
