using ElevatorApi.Model;
using ElevatorApi.Services;
using ElevatorLib;
using Microsoft.AspNetCore.SignalR;

namespace ElevatorApi.Hubs
{
    // TODO: Read: https://learn.microsoft.com/en-us/aspnet/signalr/overview/performance/signalr-performance
    // TODO: use streaming or MessagePack https://learn.microsoft.com/en-us/aspnet/core/signalr/messagepackhubprotocol?view=aspnetcore-7.0

    public class PlayerHub : Hub
    {
        private readonly IElevatorService _elevatorService;
        private readonly IHubContext<PlayerHub> _hubContext;
        private readonly TextWriter _outputStream = TextWriter.Null;

        public PlayerHub(IElevatorService elevatorService, IHubContext<PlayerHub> hubContext)
        {
            _elevatorService = elevatorService;
            _hubContext = hubContext;
        }

        public async Task UpdatePosition(UserPacket packet) 
        {
            _outputStream.WriteLine("UpdatePosition");
            // TODO: Read: https://learn.microsoft.com/en-us/aspnet/signalr/overview/getting-started/tutorial-high-frequency-realtime-with-signalr
            // this can be used for DDOS?
            await Clients.All.SendAsync("PositionUpdated", packet);
        }

        public async Task SendMessage(string userId, string message)
        {
            _outputStream.WriteLine("SendMessage");
            await Clients.All.SendAsync("MessageReceived", userId, message);
        }

        public async Task RequestElevator(int floor)
        {
            _outputStream.WriteLine($"RequestElevator: {floor}");
            _elevatorService.RequestElevator(floor);
        }

        public async Task<IEnumerable<IElevator>> GetElevatorSystemState()
        {
            _outputStream.WriteLine($"GetElevatorSystemState");
            return _elevatorService.GetElevatorSystemState();
        }

        private void RegisterUpdateElevatorsEvent()
        {
            _outputStream.WriteLine($"UpdateElevators");

            _elevatorService.RegisterUpdateCallback(async (sender, eventArgs) => 
            {
                if (eventArgs?.Elevators is not null)
                {
                    await _hubContext.Clients.All.SendAsync("UpdateElevators", eventArgs.Elevators);
                }
            });
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            _outputStream.WriteLine($"OnConnectedAsync");
            RegisterUpdateElevatorsEvent();
        }
    }
}