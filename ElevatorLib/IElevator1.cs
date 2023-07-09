namespace ElevatorLib
{
    public interface IElevator1
    {
        public int CurrentFloor { get; }
        public string? StatusMessage { get; }
        public int Occupants { get; }

        public void ChooseFloor(int floor);

    }
}