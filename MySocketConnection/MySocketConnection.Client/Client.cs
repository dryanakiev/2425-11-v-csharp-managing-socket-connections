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
            string message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message)) continue;

            byte[] sendBuffer = Encoding.ASCII.GetBytes(message);
            await ClientStream.WriteAsync(sendBuffer, 0, sendBuffer.Length);
        }
    }

    private async Task ReadMessage()
    {
        byte[] readBuffer = new byte[1024];
        try
        {
            while (true)
            {
                int bytesRead = await ClientStream.ReadAsync(readBuffer, 0, readBuffer.Length);
                if (bytesRead == 0)
                {
                    Console.WriteLine("Server disconnected.");
                    break;
                }

                string message = Encoding.ASCII.GetString(readBuffer, 0, bytesRead);
            
                // Ensure server messages appear on a new line
                Console.WriteLine($"\nServer: {message}");
                Console.Write("Enter something: "); // Reprint prompt
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static async Task Main(string[] args)
    {
        Client client = new Client();
        
        await client.HandleServer();
    }
}