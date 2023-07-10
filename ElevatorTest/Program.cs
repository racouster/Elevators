//Console.WriteLine("How many elevators would you like to simulate?");
//var requestedElevatorCount = Console.ReadLine();
//var messages = new List<string>();
//Console.Clear();

//if (int.TryParse(requestedElevatorCount, out int elevatorCount))
//{
//    messages.Add($"Starting {elevatorCount} elevators...");
//}
//else
//{
//    messages.Add($"Starting 1 elevator...");
//}


var game = new Game();
await game.StartAsync();

Console.Clear();
Console.WriteLine("Goodbye...");