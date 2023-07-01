using ElevatorLib;

namespace ElevatorApi.Services
{
    public interface IElevatorService
    {
        public void RequestElevator(int floor);

        public IEnumerable<IElevator> GetElevatorSystemState();
        public void RegisterUpdateCallback(EventHandler<CustomEventArgs> callback);
    }
}
