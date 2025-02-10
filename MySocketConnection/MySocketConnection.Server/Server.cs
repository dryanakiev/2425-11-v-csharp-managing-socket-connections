using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;

namespace MySocketConnection.Server;

class Server
{
    static void Main(string[] args)
    {
        byte[] buffer = new byte[1024];
        
        IPAddress serverIp = IPAddress.Any;
        
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        serverSocket.Bind(new IPEndPoint(serverIp, 8080));
        
        serverSocket.Listen(10);
        
        Console.WriteLine($"Server started with IP address: {serverIp}.");
        
        Socket clientSocket = serverSocket.Accept();

        if (clientSocket.Connected)
        {
            Console.WriteLine("Client connected!");
            
            buffer = Encoding.ASCII.GetBytes("Welcome to the server!");
            
            clientSocket.Send(buffer);
            
            buffer = new byte[1024];
        }

        while (true)
        {
            int bytesRead = clientSocket.Receive(buffer);

            string messageReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            
            Console.WriteLine($"Client sent: {messageReceived}");
            
            clientSocket.Send(Encoding.ASCII.GetBytes(messageReceived),bytesRead,0);
            
            buffer = new byte[1024];
        }
    }
}