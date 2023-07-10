namespace ElevatorApi.Model
{
    public interface IUserPacket
    {
        public string Username { get; set; }
        public string? Message { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int Floor { get; set; }

    }
    public class UserPacket : IUserPacket
    {
        public string Username { get; set; }
        public string? Message { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int Floor { get; set; }

    }
}


