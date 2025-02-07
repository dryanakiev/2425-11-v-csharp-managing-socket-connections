using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MySocketConnection.Client;

class Client
{
    static void Main(string[] args)
    {
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 8080));
        
        byte[] buffer = new byte[1024];

        while (true)
        {
            string input = Console.ReadLine();
            
            buffer = Encoding.ASCII.GetBytes(input);
            
            clientSocket.Send(buffer);
        }
    }
}