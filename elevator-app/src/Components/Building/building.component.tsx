import { useContext } from 'react';
import { ElevatorContext } from '../../Contexts/elevator.context';
import ElevatorShaft from '../ElevatorShaft/elevator-shaft.component'
import './building.styles.scss'

const Building = () => {
    const { elevators } = useContext(ElevatorContext);
    return (
        <>
            <div className='building-container'>
                <div className='building'>
                {elevators && elevators.map((elevator, index) => (
                    <div key={index} className='elevator-shaft-container'>
                        <ElevatorShaft elevator={elevator} ></ElevatorShaft>
                    </div>
                ))}
                </div>
            </div>
        </>
    )
}

export default Building;