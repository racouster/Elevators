using System.Collections.Immutable;

namespace ElevatorLib
{
    public interface IElevatorSystemManager
    {
        int ElevatorCount { get; }
        List<IElevator> Elevators { get; }
        List<IFloor> Floors { get; }
        int MaximumFloor { get; }
        int MinimumFloor { get; }
        int WaitTime { get; }

        void AddElevator();
        int AddFloorOccupants(int floorNumber, int count);
        int RemoveFloorOccupants(int floorNumber, int count);

        IEnumerable<IElevator> GetElevatorSystemState();
        IEnumerable<IFloor> GetFloors();
        void RegisterUpdateCallback(EventHandler<CustomEventArgs> callback);
        void RemoveElevator();
        int CallElevator(int floor);
        void Start();
        void Stop();
        IEnumerable<IElevator> Update(TimeSpan timeDelta);
    }
}