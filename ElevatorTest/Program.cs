// See https://aka.ms/new-console-template for more information
using ElevatorLib;
using System.Text;

Console.Title = "Elevator";
Console.WriteLine("How many elevators would you like to simulate?");

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
} else
{
    message = $"Starting 1 elevator...";
    elevatorSystem = new ElevatorSystemManager(1);
}

elevatorSystem.Start();

while (keepRunning)
{
    // Borrowed from https://www.codementor.io/@dewetvanthomas/tutorial-game-loop-for-c-128ovxgrig
    // Calculate the time elapsed since the last game loop cycle
    TimeSpan GameTime = DateTime.Now - _previousGameTime;
    // Update the current previous game time
    _previousGameTime = _previousGameTime + GameTime;
    
    // Update Game at 7.5fps
    await Task.Delay(128);

    elevatorSystem.Update(GameTime);
    var input = GetInput();

    if (input == ConsoleKey.Escape)
    {
        Console.WriteLine("Exiting...");
        elevatorSystem.Stop();
        keepRunning = false;
    }

    //int.TryParse("", out int selectedFloor)
    if (input == ConsoleKey.D1)
    {
        elevatorSystem.RequestElevator(1);
    }
    if (input == ConsoleKey.D2)
    {
        elevatorSystem.RequestElevator(2);
    }

    if (input == ConsoleKey.D0)
    {
        elevatorSystem.RequestElevator(0);
    }

    //DrawScreen();
}

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

static ConsoleKey GetInput()
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey(true);
        return key.Key;
    }

    return ConsoleKey.NoName;
}