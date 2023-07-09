namespace ElevatorLib.States
{
    public abstract class ElevatorState
    {
        public abstract void EnterState(ElevatorManager elevator);
        public abstract void UpdateState(ElevatorManager elevator);
        public virtual bool CanProceedTo(ElevatorState targetState) => true;
        public abstract void LeaveState(ElevatorManager elevator);
        public virtual ElevatorState Clone() => (ElevatorState)this.MemberwiseClone();

    }
}