import type { Dispatch } from "react";
import { createContext, useState } from "react";
import IElevator from "../Model/IElevator";

export interface IElevatorContext {
  elevators: IElevator[],
  setElevators: Dispatch<React.SetStateAction<IElevator[]>>
}


export const ElevatorContext = createContext<IElevatorContext>({
  elevators: [],
  setElevators: () => null,
});

export const ElevatorContextProvider = ({ children }: any) => {
  const [elevators, setElevators] = useState([] as IElevator[]);
  
  const elevatorState = { elevators, setElevators };
  return <ElevatorContext.Provider value={elevatorState}>{children}</ElevatorContext.Provider>
};
