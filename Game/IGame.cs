public interface IGame
{
    int ElevatorCount { get; }
    int MaximumFloor { get; }
    int MinimumFloor { get; }

    int CallElevator(int selectedFloor);
    int IncreaseGameSpeed();
    int DecreaseGameSpeed();
    void Quit();
    int SetFloorOccupants(int targetFloor, int count);
    int AddFloorOccupants(int targetFloor, int count);
    int RemoveFloorOccupants(int targetFloor, int count);
    int EmbarkOccupants(int targetFloor, IElevator elevator, int count);
    int DisembarkOccupants(int targetFloor, IElevator elevator, int count);
    Task StartAsync();
    IEnumerable<IElevator> GetElevators();
    List<IFloor> GetFloors();
    int GetPopulation(int selectedFloor);
}