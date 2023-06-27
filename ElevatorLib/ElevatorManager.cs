namespace ElevatorLib
{
    public class Elevator
    {
        Guid Id { get; set; } = Guid.NewGuid();
        int CurrentFloor { get; set; } = -1;
        int TargetFloor { get; set; } = -1;

        //int MaxFloor { get; set; } = 100;
        //int MinFloor { get; set; } = -3;
        //int Speed { get; set; } = 0;
        //int MaxWeight { get; set; } = 1000;
        //int CurrentWeight { get; set; } = 0;
    }

    public class ElevatorManager
    {
        ElevatorState _currentState { get; set; }

        public IdleState IdleState { get; set; } = new IdleState();
        public MovingState MovingState { get; set; } = new MovingState();
        public ErrorState ErrorState { get; set; } = new ErrorState();

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
            elevatorState.EnterState(this);
        }
    }

    public abstract class ElevatorState
    {
        public abstract void EnterState(ElevatorManager elevator);
        public abstract void UpdateState(ElevatorManager elevator);
        public abstract void OnEnterElevator(ElevatorManager elevator);
    }

    public class IdleState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {

        }
        public override void UpdateState(ElevatorManager elevator)
        {
        }
        public override void OnEnterElevator(ElevatorManager elevator)
        {
        }
    }

    public class MovingState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
        }
        public override void UpdateState(ElevatorManager elevator)
        {
            elevator.ChangeState(elevator.IdleState);
        }
        public override void OnEnterElevator(ElevatorManager elevator)
        {
        }
    }

    public class ErrorState : ElevatorState
    {
        public override void EnterState(ElevatorManager elevator)
        {
        }
        public override void UpdateState(ElevatorManager elevator)
        {
        }
        public override void OnEnterElevator(ElevatorManager elevator)
        {
        }
    }
}