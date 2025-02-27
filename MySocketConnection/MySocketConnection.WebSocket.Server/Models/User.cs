namespace MySocketConnection.WebSocket.Server.Models;

public class User
{
    public int Id { get; set; } = new Random().Next(1,1000);
    public string Email { get; set; }
    public string Password { get; set; }
}