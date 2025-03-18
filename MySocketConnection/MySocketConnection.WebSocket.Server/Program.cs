namespace MySocketConnection.WebSocket.Server;

public class Program
{
    static void Main(string[] args)
    {
        WebServer server = new WebServer();
        server.Start();
    }
}