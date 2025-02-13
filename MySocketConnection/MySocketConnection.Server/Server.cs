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
                TcpClient client = await ServerSocket.AcceptTcpClientAsync();

                if (client.Connected)
                {
                    ClientSockets.Add(client);
                    
                    Console.WriteLine($"{client.Client.RemoteEndPoint} connected!");
                    
                    GreetClient(client);
                    
                    _ = Task.Run(() => HandleClient(client));
                }
            }
        }
    }
    
    public void GreetClient(TcpClient client)
    {
        Buffer = Encoding.ASCII.GetBytes("Welcome to the server!");
            
        client.Client.Send(Buffer);
            
        Buffer = new byte[1024];
    }

    public async Task HandleClient(TcpClient sender)
    {
        NetworkStream stream = sender.GetStream();
        
        Buffer = new byte[1024];

        while (true)
        {
            await stream.ReadAsync(Buffer, 0, Buffer.Length);
            
            string message = Encoding.ASCII.GetString(Buffer, 0, Buffer.Length);
            Console.WriteLine($"Received from {sender.Client.RemoteEndPoint}: {message}");
            await BroadcastToAllClients(message, sender);
        }
    }

    public async Task BroadcastToAllClients(string message, TcpClient sender)
    {
        List<Task> sendTasks = new List<Task>();
        foreach (TcpClient client in ClientSockets)
        {
            if (client != sender)
            {
                sendTasks.Add(client.GetStream().WriteAsync(Buffer, 0, Buffer.Length));
            }
        }

        await Task.WhenAll(sendTasks);
    }
    
    
    static async Task Main(string[] args)
    {
        Server server = new Server();
        
        await server.AcceptClients();
    }
}