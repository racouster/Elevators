namespace ElevatorLib
{
    public partial class ElevatorSystemManager
    {
        private class Elevator : IElevator
        {
            public int CurrentFloor { get; set; }
            public string? StatusMessage { get; set; }
            public int Occupants { get; set; }
        }
    }
}
