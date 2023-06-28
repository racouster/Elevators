// See https://aka.ms/new-console-template for more information
using ElevatorLib;
using System.Text;

Console.Title = "Elevator";
Console.WriteLine("How many elevators would you like to simulate?");

var sleepTimes = new List<int>() { 256, 128, 64, 32, 16, 8, 4, 2, 1 };
var selectedSleepTime = 0;
var sleepTime = sleepTimes[selectedSleepTime];

var message = "";
var keepRunning = false;
var _previousGameTime = DateTime.Now;

var requestedElevatorCount = Console.ReadLine();

ElevatorSystemManager elevatorSystem;

if (int.TryParse(requestedElevatorCount, out int elevatorCount))
{
    keepRunning = true;
    message = $"Starting {elevatorCount} elevators...";
    elevatorSystem = new ElevatorSystemManager(elevatorCount);
}
else
{
    message = $"Starting 1 elevator...";
    elevatorSystem = new ElevatorSystemManager(1);
}

Console.WriteLine("Input keys: `ESC` - Quit, `1 .. 9` - Choose a floor, `+` - Cycle processign speed");

elevatorSystem.Start();

while (keepRunning)
{
    // Borrowed from https://www.codementor.io/@dewetvanthomas/tutorial-game-loop-for-c-128ovxgrig
    // Calculate the time elapsed since the last game loop cycle
    TimeSpan GameTime = DateTime.Now - _previousGameTime;
    // Update the current previous game time
    _previousGameTime += GameTime;

    // Update Game at 3.90625 fps
    await Task.Delay(sleepTime);

    elevatorSystem.Update(GameTime);
    var input = GetInput();
    var inputKey = input.Key;

    if (inputKey == ConsoleKey.Escape)
    {
        Console.WriteLine("Exiting...");
        elevatorSystem.Stop();
        keepRunning = false;
    }

    // TODO: Use better input handling for numbers
    //int.TryParse("", out int selectedFloor)
    if (int.TryParse(input.KeyChar.ToString(), out int floor))
    {
        elevatorSystem.RequestElevator(floor);
    }

    if (inputKey == ConsoleKey.Add)
    {
        if (selectedSleepTime >= sleepTimes.Count - 1)
        {
            selectedSleepTime = 0;
        }
        else
        {
            selectedSleepTime++;
        }

        sleepTime = sleepTimes[selectedSleepTime];
    }

    // TODO: Draw map, elevators, screen and player
    //DrawScreen();
}

// TODO: Create Visual representation of the elevator system
void DrawScreen()
{
    var width = Console.BufferWidth;
    var height = Console.BufferHeight;

    var a = new StreamWriter(Console.OpenStandardOutput(), encoding: ASCIIEncoding.ASCII, width * height);
    a.AutoFlush = false;

    for (int i = 0; i < height; i++)
    {
        a.WriteLine();
    }

    a.Flush();
}

static ConsoleKeyInfo GetInput()
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey(true);
        return key;
    }

    return new ConsoleKeyInfo((char)ConsoleKey.NoName,ConsoleKey.NoName, false, false, false);
}