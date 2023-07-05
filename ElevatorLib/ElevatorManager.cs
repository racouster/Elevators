using ElevatorLib.States;

namespace ElevatorLib
{
    public class ElevatorManager
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string? StatusMessage { get; private set; }

        public int MinimumFloor { get; private set; } = -3;
        public int MaximumFloor { get; private set; } = 10;

        public int CurrentFloor { get; private set; } = 0;
        public int TargetFloor { get; private set; } = 0;

        public int Occupants { get; private set; } = 0;
        public int OccupantLimit { get; private set; } = 0;

        public ElevatorState _currentState { get; private set; }
        public ElevatorState _previousState { get; private set; }

        public IdleState IdleState = new();
        public MovingState MovingState = new();
        public DoorsClosedState DoorsClosedState = new();
        public DoorsOpenState DoorsOpenState = new();
        public ErrorState ErrorState = new();

        private TextWriter _output = TextWriter.Null; //Console.Out;

        public ElevatorManager()
        {
            _previousState = new IdleState();
            _currentState = new IdleState();
            _currentState.OnEnterState(this);
            StatusMessage = "Initializing...";
        }

        public ElevatorManager(StreamWriter outputStream) : this()
        {
            _output = outputStream;
        }

        public ElevatorManager(int startingFloor) : this()
        {
            CurrentFloor = startingFloor;
            TargetFloor = startingFloor;
        }

        public ElevatorManager(int startingFloor, int minimumFloor, int maximumFloor) : this(startingFloor)
        {
            MinimumFloor = minimumFloor;
            MaximumFloor = maximumFloor;
        }

        public void Update()
        {
            _output.WriteLine(this.StatusMessage);
            _currentState.UpdateState(this);
        }

        public void ChangeState(ElevatorState elevatorState)
        {
            if (_currentState.CanProceedTo(this))
            {
                _currentState.OnLeaveState(this);
                _previousState = _currentState.Clone();
                _currentState = elevatorState;
                _currentState.OnEnterState(this);
            }
        }

        internal void MoveDown()
        {
            if (this.CurrentFloor > this.MinimumFloor)
            {
                this.CurrentFloor--;
            }
        }

        internal void MoveUp()
        {
            if (this.CurrentFloor < this.MaximumFloor)
            {
                this.CurrentFloor++;
            }
        }

        public void ChooseFloor(int targetFloor)
        {
            if (targetFloor < this.MinimumFloor)
            {
                this.TargetFloor = this.MinimumFloor;
            }
            else if (targetFloor > this.MaximumFloor)
            {
                this.TargetFloor = this.MaximumFloor;
            }
            else
            {
                this.TargetFloor = targetFloor;
            }

            this.ChangeState(this.MovingState);
        }

        public void SetStatusMessage(string newMessage)
        {
            this.StatusMessage = newMessage;
        }

        internal int AddOccupants(int newOccupants)
        {
            var availableSpace = OccupantLimit - Occupants;
            int remainder = availableSpace - newOccupants;

            Occupants += availableSpace - newOccupants;

            return remainder;
        }

        internal bool OpenDoor()
        {
            if (_currentState != IdleState)
            {
                return false;
            }

            ChangeState(DoorsOpenState);
            return true;
        }
    }
}