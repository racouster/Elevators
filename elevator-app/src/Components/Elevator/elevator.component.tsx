import IElevator from '../../Model/IElevator';
import './elevator.styles.scss'

const Elevator = (elevator: IElevator) => {
    const { currentFloor, statusMessage } = elevator;
    console.log("ElevatorComponent: currentFloor:", currentFloor);
    return (
        <div className="elevator-container">
            <div className='elevator-disaplay'>
                {currentFloor}
            </div>
            <div className='elevator-box'>
                {statusMessage}
            </div>
        </div>
    )
}

export default Elevator;