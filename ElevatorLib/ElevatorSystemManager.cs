using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using ElevatorLib.States;

[assembly: InternalsVisibleTo("Elevator.Test")]
namespace ElevatorLib
{
    public class ElevatorSystemManager
    {
        public ImmutableList<ElevatorManager> Elevators { get; private set; }
        public int ElevatorCount { get; private set; }
        
        // TODO: Use state machine for system running/paused/stopped states.
        public bool IsRunning { get; private set; } = false;

        public ElevatorSystemManager(int elevatorCount)
        {
            ElevatorCount = elevatorCount;

            Elevators = Enumerable.Range(0, ElevatorCount)
                .Select<int, ElevatorManager>(_ => new ElevatorManager())
                .ToImmutableList();
        }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void RequestElevator(int floor)
        {
            var idleElevator = Elevators
                    .Where(e => e._currentState.GetType().IsAssignableFrom(typeof(IdleState)))
                    // Just get the first Idle one for now...
                    .FirstOrDefault();

            if (idleElevator is not null)
            {
                idleElevator.ChooseFloor(floor);
            }

            //TODO: Add queue for elevators that are busy
        }

        public class Elevator : IElevator
        {
            public int CurrentFloor { get ; set; }
            public string StatusMessage { get ; set; }
        }

        public IEnumerable<IElevator> GetElevatorSystemState()
        { 
            var elevatorSnapshot = new List<IElevator>();
            return Elevators.Select(e => new Elevator() { CurrentFloor = e.CurrentFloor, StatusMessage = e.StatusMessage });
        }

        public void Update(TimeSpan timeDelta)
        {
            if (IsRunning)
            {
                // TODO: Will be used in draw to determine how much to move the elevator
                double gameTimeElapsed = timeDelta.TotalMilliseconds / 1000;

                foreach (var elevator in Elevators)
                {
                    elevator.Update();
                }
            }
        }
    }
}
