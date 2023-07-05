namespace ElevatorLib.States
{
    public abstract class ElevatorState
    {
        public abstract void OnEnterState(ElevatorManager elevator);
        public abstract void UpdateState(ElevatorManager elevator);
        public virtual bool CanProceedTo(ElevatorManager elevator)
        {
            return true;
        }
        public abstract void OnLeaveState(ElevatorManager elevator);

        public virtual ElevatorState Clone()
        {
            return (ElevatorState)this.MemberwiseClone();
        }
        
    }
}