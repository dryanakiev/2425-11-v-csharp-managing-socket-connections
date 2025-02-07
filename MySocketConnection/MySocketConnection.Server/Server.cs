using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MySocketConnection.Server;

class Server
{
    static void Main(string[] args)
    {
        IPAddress serverIp = IPAddress.Parse("");
        
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        byte[] buffer = new byte[1024];

        while (true)
        {
            int bytesRead = serverSocket.Receive(buffer);

            Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, bytesRead));
        }
    }
}