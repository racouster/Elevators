namespace ElevatorLib
{
    public abstract class ElevatorState
    {
        public abstract void EnterState(ElevatorManager elevator);
        public abstract void UpdateState(ElevatorManager elevator);
        public abstract void OnFloorSelected(ElevatorManager elevator, int targetFloor);
    }
}