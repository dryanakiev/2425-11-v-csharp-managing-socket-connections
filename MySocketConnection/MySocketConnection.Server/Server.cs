using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MySocketConnection.Server;

class Server
{
    static void Main(string[] args)
    {
        IPAddress serverIp = IPAddress.Any;
        
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        serverSocket.Bind(new IPEndPoint(serverIp, 8080));
        
        serverSocket.Listen(10);
        
        Socket clientSocket = serverSocket.Accept();
        
        byte[] buffer = new byte[1024];

        while (true)
        {
            int bytesRead = clientSocket.Receive(buffer);

            string messageReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            
            Console.WriteLine($"Client sent: {messageReceived}");
        }
    }
}