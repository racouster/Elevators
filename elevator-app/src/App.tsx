import { ChangeEvent, useContext, useEffect, useState } from 'react';

import { ConnectionContext } from './Contexts/connection.context';
import { IUserPacket } from './Model/IUserPacket';
import './App.scss';
// import logo from './logo.svg';
import IElevator from './Model/IElevator';
import Building from './Components/Building/building.component';
import { ElevatorContext } from './Contexts/elevator.context';

let currentUserDefault = {
  username: "User1",
  message: "User1 started!",
  xPosition: 0,
  yPosition: 0
} as IUserPacket;
const defaultUsers: Array<IUserPacket> = [currentUserDefault];

function App() {
  // TODO: Add users context or service
  const { connection, addHandler } = useContext(ConnectionContext);
  const { setElevators } = useContext(ElevatorContext);

  const [users, setUsers] = useState(defaultUsers);
  const [currentUser, setCurrentUser] = useState(currentUserDefault);
  const [requestedFloor, setRequestedFloor] = useState(0);

  useEffect(() => {
    // Register user update event
    addHandler("PositionUpdated", (userPacket: IUserPacket) => {
      const existingUserIndex = users.findIndex(u => u.username === userPacket.username)

      if (existingUserIndex !== -1)
        return [users.slice(0, existingUserIndex), userPacket, users.slice(existingUserIndex)]
      else
        setUsers([...users, userPacket]);

      console.log("user upserted:", users);
    });

    // Register user message event
    addHandler("MessageReceived", (userId: string, message: string) => {
      console.log("New Message:", userId, message);
    });

    // Register user message event
    addHandler("UpdateElevators", (elevators: IElevator[]) => {
      //console.log("UpdateElevators:", elevators);
      setElevators(elevators)
    });
    // Ignore warning as this is a once off section
    // eslint-disable-next-line
  }, [])

  const getElevatorSystemState = async (clickEvent: any) => {
    var a = await connection.invoke("GetElevatorSystemState");
    console.log("GetElevatorSystemState", a);
  };

  const requestFloor = async (clickEvent: any) => {
    await connection.invoke("RequestElevator", requestedFloor);
    console.log("RequestElevator", requestedFloor);
  };

  const sendMessage = async (clickEvent: any) => {
    console.log("Sending message", currentUser, clickEvent);
    connection.invoke("SendMessage", currentUser.username, currentUser.message);
  };

  const sendUpdatedPosition = async (clickEvent: any) => {
    console.log("UpdatePosition", currentUser, clickEvent);
    connection.invoke("UpdatePosition", currentUser).then((noice) => (console.log("position updated.", noice)));
    currentUser.message = "";
  };

  const messageInputChange = (changeEvent: ChangeEvent<HTMLInputElement>) => setCurrentUser({ ...currentUser, message: changeEvent.target.value });
  const requestedFloorChange = (changeEvent: ChangeEvent<HTMLInputElement>) => setRequestedFloor(Number(changeEvent.target.value));

  return (
    <div className="App">
      <header>
        <h1>Elevator</h1>
      </header>
      <main>
        <Building />
      </main>
      <footer>
        <div>
          <input type='text' value={currentUser.message} onChange={messageInputChange} />
          <button type='button' onClick={sendMessage}>Send</button>
          <button type='button' onClick={sendUpdatedPosition}>Join</button>
          <button type='button' onClick={getElevatorSystemState}>Log State</button>
        </div>
        <div className=' max-w-sm'>
          <input type='text' value={requestedFloor} onChange={requestedFloorChange} className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500'/>
          <button type='button' className=' bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded ' onClick={requestFloor}>Call Elevator</button>
        </div>
      </footer>
    </div>
  );
}

export default App;
