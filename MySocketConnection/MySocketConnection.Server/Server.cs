using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MySocketConnection.Server;

class Server
{
    public IPAddress ServerIpAddress { get; set; }
    public int ServerPort { get; set; }
    public TcpListener ServerSocket { get; set; }
    public List<TcpClient> ClientSockets { get; set; } = new List<TcpClient>();

    public Server()
    {
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
        client.Client.Send(Encoding.ASCII.GetBytes("Welcome to the server!"));
    }

    public async Task HandleClient(TcpClient sender)
    {
        NetworkStream stream = sender.GetStream();
        byte[] buffer = new byte[1024];

        while (true)
        {
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            
            if (bytesRead == 0) // Client disconnected
            {
                break;
            } 

            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received from {sender.Client.RemoteEndPoint}: {message}");

            List<Task> sendTasks = new List<Task>();
            foreach (TcpClient client in ClientSockets)
            {
                sendTasks.Add(client.GetStream().WriteAsync(buffer, 0, bytesRead));
            }

            await Task.WhenAll(sendTasks);
        }

        sender.Close();
        ClientSockets.Remove(sender);
    }

    
    static async Task Main(string[] args)
    {
        Server server = new Server();
        
        await server.AcceptClients();
    }
}