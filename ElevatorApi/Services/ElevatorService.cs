using ElevatorLib;

namespace ElevatorApi.Services
{
    public class ElevatorService : IElevatorService
    {
        //TODO: use: https://learn.microsoft.com/en-us/dotnet/api/system.threading.timer?view=net-7.0&redirectedfrom=MSDN

        public ElevatorService()
        {
            // Create elevator manager
            _elevatorSystem = new ElevatorSystemManager(elevatorCount: 3, -3, 10);
            _elevatorSystem.Start();
        }

        public ElevatorSystemManager _elevatorSystem { get; }

        public void RequestElevator(int floor)
        {
            _elevatorSystem.RequestElevator(floor);
        }

        public IEnumerable<IElevator> GetElevatorSystemState()
        {
            return _elevatorSystem.GetElevatorSystemState();
        }

        public void RegisterUpdateCallback(EventHandler<CustomEventArgs> callback)
        {
            _elevatorSystem.RegisterUpdateCallback(callback);
        }
    }
}
