using ElevatorLib.States;

namespace ElevatorLib
{
    public interface IElevator
    {
        int CurrentFloorNumber { get; }
        ElevatorState CurrentState { get; }
        Guid Id { get; }
        int MaximumFloor { get; }
        int MinimumFloor { get; }
        int OccupantLimit { get; }
        int Occupants { get; }
        ElevatorState PreviousState { get; }
        List<string> StatusMessages { get; }
        int TargetFloor { get; }

        void ChangeState(ElevatorState elevatorState);
        void ChooseFloor(int targetFloor);
        void SetStatusMessage(string newMessage);
        public int AddOccupants(int removedOccupants);
        public int RemoveOccupants(int removedOccupants);
        void Update(IFloor floor);
    }
}