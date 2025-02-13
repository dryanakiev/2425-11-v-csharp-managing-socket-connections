using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MySocketConnection.Server;

class Server
{
    public IPAddress ServerIpAddress { get; set; }
    public int ServerPort { get; set; }
    public byte[] Buffer { get; set; }
    public TcpListener ServerSocket { get; set; }
    public List<TcpClient> ClientSockets { get; set; } = new List<TcpClient>();

    public Server()
    {
        Buffer = new byte[1024];
        ServerIpAddress = IPAddress.Any;
        ServerPort = 9999;

        ServerSocket = new TcpListener(ServerIpAddress, ServerPort);
        
        ServerSocket.Start();
    }

    public async Task AcceptClients()
    {
        while (true)
        {
            if (ServerSocket.Pending())
            {
                TcpClient client = ServerSocket.AcceptTcpClient();
                
                ClientSockets.Add(client);

                if (client.Connected)
                {
                    GreetClient(client).Wait();
                }
            }
        }
    }
    
    public async Task GreetClient(TcpClient client)
    {
        Console.WriteLine("Client connected!");
            
        Buffer = Encoding.ASCII.GetBytes("Welcome to the server!");
            
        client.Client.Send(Buffer);
            
        Buffer = new byte[1024];
    }

    public async Task BroadcastToAllClients()
    {
        while (true)
        {
            int bytesRead = ServerSocket.Server.Receive(Buffer);

            string message = Encoding.ASCII.GetString(Buffer, 0, bytesRead);

            foreach (TcpClient client in ClientSockets)
            {
                client.Client.Send(Encoding.ASCII.GetBytes($"{client.Client.RemoteEndPoint}: {message}"));
            }

            Buffer = new byte[1024];
        }
    }
    
    
    static void Main(string[] args)
    {
        Server server = new Server();

        server.AcceptClients();
    }
}