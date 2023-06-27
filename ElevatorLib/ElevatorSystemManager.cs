using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorLib
{
    public class ElevatorSystemManager
    {
        private List<ElevatorManager> Elevators { get; set; } = new List<ElevatorManager>();
        private int ElevatorCount { get; }

        public ElevatorSystemManager(int elevatorCount)
        {
            ElevatorCount = elevatorCount;

            Elevators = Enumerable.Range(0, ElevatorCount)
                .Select<int, ElevatorManager>(_ => new ElevatorManager())
                .ToList();
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
            
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

            //TODO: What if there are no idle elevators?
        }

        public void Update(TimeSpan timeDelta)
        {
            double gameTimeElapsed = timeDelta.TotalMilliseconds / 1000;

            foreach (var elevator in Elevators)
            {
                elevator.Update();
            }
        }
    }
}
