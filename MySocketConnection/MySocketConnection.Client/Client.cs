using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MySocketConnection.Client;

class Client
{
    static void Main(string[] args)
    {
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, 9999));
        
        byte[] buffer = new byte[1024];

        while (true)
        {
            int bytesRead = clientSocket.Receive(buffer);

            string messageReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            
            Console.WriteLine($"Server sent: {messageReceived}");
            
            string input = Console.ReadLine();
            
            buffer = Encoding.ASCII.GetBytes(input);
            
            clientSocket.Send(buffer);
            
            buffer = new byte[1024];
        }
    }
}