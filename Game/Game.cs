

using GameSystem;

public class Game : IGame
{
    private readonly List<int> SleepTimes = new List<int>() { 256, 128, 64, 32, 16, 8, 4, 2, 1 };
    private int SelectedSleepIndex = 0;
    private int SleepTime = 64;
    private bool IsRunning = false;
    private DateTime PreviousGameTime = DateTime.Now;
    private UserInterface UserInterface;
    private readonly ElevatorSystemManager ElevatorSystem;
public int ElevatorCount { get; }
    public int MinimumFloor { get; } = -3;
    public int MaximumFloor { get; } = 10;

    // TODO: Come up with a better title
    public Game(
        string title = "Simuvator",
        int elevatorCount = 1)
    {
        ElevatorCount = elevatorCount;
        ElevatorSystem = new ElevatorSystemManager(ElevatorCount, MinimumFloor, MaximumFloor, 10, 100);
        UserInterface = new UserInterface(this, title);
    }


    // TODO: Do I need DI here?
    public async Task StartAsync()
    {
        IsRunning = true;
        ElevatorSystem.Start();
        await RunAsync();
    }

    private async Task RunAsync()
    {
        if (ElevatorSystem is null)
        {
            throw new Exception("Elevator system is not initialized.");
        }

        // Main loop
        while (IsRunning)
        {
            // TODO: Calculate movement distance by time elapsed instead.

            // Calculate the time elapsed since the last game loop cycle, Borrowed from https://www.codementor.io/@dewetvanthomas/tutorial-game-loop-for-c-128ovxgrig
            TimeSpan GameTime = DateTime.Now - PreviousGameTime;

            // Slow down game speed
            //sleep less when processing takes longer. // Math.Min(SleepTime - GameTime.Milliseconds,
            await Task.Delay(SleepTime);

            // Update the previous game time
            PreviousGameTime += GameTime;

            // Run menu update commands
            UserInterface.Run();
        }
    }

    public int DecreaseGameSpeed()
    {
        if (SelectedSleepIndex <= SleepTimes.Count - 1)
        {
            SelectedSleepIndex = 0;
        }
        else
        {
            SelectedSleepIndex--;
        }

        SleepTime = SleepTimes[SelectedSleepIndex];
        return SleepTime;
    }
    
    public int IncreaseGameSpeed()
    {
        if (SelectedSleepIndex >= SleepTimes.Count - 1)
        {
            SelectedSleepIndex = 0;
        }
        else
        {
            SelectedSleepIndex++;
        }

        SleepTime = SleepTimes[SelectedSleepIndex];
        return SleepTime;
    }

    public int CallElevator(int selectedFloor)
    {
        return ElevatorSystem?.CallElevator(selectedFloor) ?? 0;
    }

    public int EmbarkOccupants(int targetFloor, IElevator elevator, int count)
    {
        var remainder = elevator.RemoveOccupants(count);
        var floorOccupants = ElevatorSystem?.AddFloorOccupants(targetFloor, remainder) ?? 0;
        return floorOccupants;
    }

    public int DisembarkOccupants(int targetFloor, IElevator elevator, int count)
    {
        var remainder = elevator.AddOccupants(count);
        var floorOccupants = ElevatorSystem?.RemoveFloorOccupants(targetFloor, remainder) ?? 0;
        return floorOccupants;
    }

    public int SetFloorOccupants(int targetFloor, int count)
    {
        return ElevatorSystem?.SetFloorOccupants(targetFloor, count) ?? 0;
    }

    public int AddFloorOccupants(int targetFloor, int count)
    {
        return ElevatorSystem?.AddFloorOccupants(targetFloor, count) ?? 0;
    }

    public int RemoveFloorOccupants(int targetFloor, int count)
    {
        return ElevatorSystem?.RemoveFloorOccupants(targetFloor, count) ?? 0;
    }

    public IEnumerable<IElevator> GetElevators() {
        return ElevatorSystem.GetElevatorSystemState();
    }

    public List<IFloor> GetFloors()
    {
        return ElevatorSystem.Floors;
    }

    public void Quit()
    {
        ElevatorSystem?.Stop();
        IsRunning = false;
        Environment.Exit(0);
    }

    public int GetPopulation(int selectedFloor)
    { 
        var clampedFloor = Math.Clamp(selectedFloor, MinimumFloor, MaximumFloor);
        return ElevatorSystem.Floors.FirstOrDefault(f => f.Id == clampedFloor - MinimumFloor)?.Occupants ?? 0;
    }
}
