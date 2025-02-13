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
        ServerIpAddress = IPAddress.Loopback;
        ServerPort = 9999;
        Buffer = new byte[1024];
        
        ClientSocket.ConnectAsync(ServerIpAddress, ServerPort);

        if (ClientSocket.Connected)
        {
            ClientStream = ClientSocket.GetStream();
            
            Console.WriteLine($"Connected to server {ServerIpAddress}:{ServerPort} succesfully!");

            _ = Task.Run(() => ReceiveMessage());
        }
    }

    public async Task SendMessage()
    {
        string message = Console.ReadLine();
        
        Buffer = Encoding.ASCII.GetBytes(message);
        await ClientStream.WriteAsync(Buffer, 0, Buffer.Length);
    }

    public async Task ReceiveMessage()
    {
        while (true)
        {
            int bytesRead = await ClientStream.ReadAsync(Buffer, 0, Buffer.Length);

            string message = Encoding.ASCII.GetString(Buffer, 0, bytesRead);
            Console.WriteLine($"\n{message}\nYou: ");
        }
    }

    static async Task Main(string[] args)
    {
        Client client = new Client();
        while (true)
        {
            await client.ReceiveMessage();
            await client.SendMessage();
        }
    }
}