using ElevatorLib.States;

namespace ElevatorLib
{
    public class ElevatorManager : IElevator
    {
        public Guid Id { get; } = Guid.NewGuid();
        public List<string> StatusMessages { get; private set; } = new();

        public int MinimumFloor { get; private set; } = -3;
        public int MaximumFloor { get; private set; } = 10;

        public int CurrentFloorNumber { get; private set; }
        public IFloor CurrentFloor { get; private set; }
        public int TargetFloor { get; private set; }

        public int Occupants { get; private set; }
        public int OccupantLimit { get; private set; }

        public ElevatorState CurrentState { get; private set; }
        public ElevatorState PreviousState { get; private set; }
        public Queue<ElevatorState> StateQueue { get; private set; } = new();

        public IdleState IdleState = new();
        public MovingState MovingState = new();
        public DoorsClosedState DoorsClosedState = new();
        public DoorsOpenState DoorsOpenState = new();
        public ErrorState ErrorState = new();

        private TextWriter _output = new DebugWriter(); // TextWriter.Null;

        public ElevatorManager(int startingFloor, int occupantLimit, int minimumFloor, int maximumFloor)
        {
            PreviousState = new IdleState();
            CurrentState = new IdleState();
            CurrentState.EnterState(this);
            StatusMessages.Add("Initializing...");

            CurrentFloorNumber = startingFloor;
            TargetFloor = startingFloor;
            MinimumFloor = minimumFloor;
            MaximumFloor = maximumFloor;
            OccupantLimit = occupantLimit;
            Occupants = 0;
        }

        public void Update(IFloor floor)
        {
            _output.WriteLine(string.Join('\n', StatusMessages));
            StatusMessages.Clear();
            CurrentFloor = floor;
            CurrentState.UpdateState(this);
            
            StateQueue.TryDequeue(out ElevatorState? nextstate);
            if (nextstate is not null)
            {
                CurrentState.LeaveState(this);
                PreviousState = CurrentState.Clone();
                CurrentState = nextstate;
                CurrentState.EnterState(this);
            }
        }

        public void ChangeState(ElevatorState newElevatorState)
        {
            StateQueue.Enqueue(newElevatorState);
        }

        internal void MoveDown()
        {
            if (this.CurrentFloorNumber > this.MinimumFloor)
            {
                this.CurrentFloorNumber--;
            }
        }

        internal void MoveUp()
        {
            if (this.CurrentFloorNumber < this.MaximumFloor)
            {
                this.CurrentFloorNumber++;
            }
        }

        public void ChooseFloor(int targetFloor)
        {
            if (targetFloor < MinimumFloor)
            {
                TargetFloor = MinimumFloor;
            }
            else if (targetFloor > MaximumFloor)
            {
                TargetFloor = MaximumFloor;
            }
            else
            {
                TargetFloor = targetFloor;
            }

            ChangeState(MovingState);
        }

        public void SetStatusMessage(string newMessage)
        {
            this.StatusMessages.Add(newMessage);
        }

        public int AddOccupants(int change)
        {
            Occupants += Math.Abs(change);
            Occupants = Math.Min(Occupants, OccupantLimit);
            return Occupants;
        }

        public int RemoveOccupants(int change)
        {
            Occupants -= Math.Abs(change);
            Occupants = Math.Max(0, Occupants);
            return Occupants;
        }

        internal bool OpenDoor()
        {
            if (CurrentState != IdleState)
            {
                return false;
            }

            ChangeState(DoorsOpenState);
            return true;
        }
    }
}