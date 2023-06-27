namespace ElevatorLib
{
    public class ElevatorManager
    {
        public Guid Id { get; } = Guid.NewGuid();
        
        public int MinimumFloor { get; private set; } = -3;
        public int MaximumFloor { get; private set; } = 10;

        public int CurrentFloor { get; private set; } = 0;
        public int TargetFloor { get; private set; } = 0;



        public ElevatorState _currentState { get; private set; }

        public IdleState IdleState = new ();
        public MovingState MovingState = new ();
        public ClosingState ClosingState = new();
        public OpeningState OpeningState = new();
        public ErrorState ErrorState = new();

        public ElevatorManager()
        {
            _currentState = new IdleState();
            _currentState.EnterState(this);
        }

        public void Update()
        {
            _currentState.UpdateState(this);
        }

        public void ChangeState(ElevatorState elevatorState)
        {
            _currentState = elevatorState;
            _currentState.EnterState(this);
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
    }
}