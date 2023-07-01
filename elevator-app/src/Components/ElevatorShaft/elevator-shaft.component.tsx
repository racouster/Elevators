import IElevatorShaft from '../../Model/IElevatorShaft';
import Elevator from '../Elevator/elevator.component';
import './elevator-shaft.styles.scss'

const ElevatorShaft = (props: IElevatorShaft) => {
    const { elevator } = props;
    return (
        <div className="elevator-shaft-container">
            <Elevator currentFloor={elevator.currentFloor} statusMessage={ elevator.statusMessage}></Elevator>
        </div>);
}

export default ElevatorShaft;