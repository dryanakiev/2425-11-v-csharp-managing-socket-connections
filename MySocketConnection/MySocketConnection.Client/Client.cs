using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MySocketConnection.Client;

class Client
{
    public IPAddress ServerIpAddress { get; set; }
    public int ServerPort { get; set; }
    public TcpClient ClientSocket { get; set; }
    public NetworkStream ClientStream { get; set; }

    public Client()
    {
        ServerIpAddress = IPAddress.Parse("127.0.0.1");
        ServerPort = 9999;
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
        var readTask = Task.Run(ReadMessage);
        var sendTask = Task.Run(SendMessage);

        await Task.WhenAll(readTask, sendTask);
    }

    private async Task SendMessage()
    {
        while (true)
        {
            Console.Write("Enter something: ");
            string message = Console.ReadLine();

            byte[] sendBuffer = Encoding.ASCII.GetBytes(message);
            await ClientStream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
            await ClientStream.FlushAsync();
        }
    }

    private async Task ReadMessage()
    {
        byte[] readBuffer = new byte[1024];
        while (true)
        {
            int bytesRead = await ClientStream.ReadAsync(readBuffer, 0, readBuffer.Length);
            if (bytesRead != 0)
            {
                string message = Encoding.ASCII.GetString(readBuffer, 0, bytesRead);
                Console.WriteLine($"\nServer: {message}");
            }
        }
    }

    static async Task Main(string[] args)
    {
        Client client = new Client();
        
        await client.HandleServer();
    }
}