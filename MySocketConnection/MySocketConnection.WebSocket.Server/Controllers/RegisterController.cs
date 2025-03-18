using System.Net;
using System.Text;
using System.Text.Json;
using MySocketConnection.WebSocket.Server.Models;

namespace MySocketConnection.WebSocket.Server.Controllers
{
    public class RegisterController
    {
        public string HandleRegister(HttpListenerContext context)
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
            return "register.html";
        }

        private string OnPost(HttpListenerContext context)
        {
            using StreamReader reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
            
            string formData = reader.ReadToEnd();
            
            var parsedData = System.Web.HttpUtility.ParseQueryString(formData);
            
            string email = parsedData["email"];
            string password = parsedData["password"];
            string passwordRepeat = parsedData["password-repeat"];

            if (password == passwordRepeat)
            {
                WriteToJsonFile(new User { Email = email, Password = password });
                return "index.html";
            }
            else
            {
                return "register.html";
            }
        }

        private async void WriteToJsonFile(User user)
        {
            string fileName = "Users.json";
            if (!File.Exists(fileName))
            {
                await using FileStream createStream = File.Create(fileName);
                await JsonSerializer.SerializeAsync(createStream, user);
            }
            else
            {
                await using FileStream writeStream = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.None);
                await JsonSerializer.SerializeAsync(writeStream, user); 
            }
        }
    }
}