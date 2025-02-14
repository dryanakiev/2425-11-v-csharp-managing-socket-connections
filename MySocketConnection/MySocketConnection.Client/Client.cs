using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MySocketConnection.Client;

class Client
{
    public IPAddress ServerIpAddress { get; set; }
    public int ServerPort { get; set; }
    public TcpClient ClientSocket { get; set; }
    public byte[] Buffer { get; set; }
    public NetworkStream ClientStream { get; set; }

    public Client()
    {
        ServerIpAddress = IPAddress.Parse("127.0.0.1");
        ServerPort = 9999;
        Buffer = new byte[1024];
        ClientSocket = new TcpClient();
        ClientSocket.Connect(ServerIpAddress, ServerPort);

        if (ClientSocket.Connected)
        {
            ClientStream = ClientSocket.GetStream();
            
            Console.WriteLine($"Connected to server {ServerIpAddress}:{ServerPort} succesfully!");
        }
    }

    public async Task HandleServer()
    {
        while(true)
        {
            await ReadMessage();
            await SendMessage();
        }
        
        // TODO: Make both methods work individually
    }

    private async Task SendMessage()
    {
        Console.Write("Enter something: ");
        string message = Console.ReadLine();

        Buffer = Encoding.ASCII.GetBytes(message);
        await ClientStream.WriteAsync(Buffer, 0, Buffer.Length);

        Buffer = new byte[1024];
        await ClientStream.FlushAsync();
    }

    private async Task ReadMessage()
    {
        string message;
        
        int bytesRead = await ClientStream.ReadAsync(Buffer, 0, Buffer.Length);

        if (bytesRead != 0)
        {
            message = Encoding.ASCII.GetString(Buffer, 0, bytesRead);
            Console.WriteLine($"Server: {message}");
        }
    }

    static async Task Main(string[] args)
    {
        Client client = new Client();
        
        await client.HandleServer();
    }
}