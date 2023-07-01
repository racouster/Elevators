import type { Dispatch } from "react";
import { createContext, useState } from "react";
import * as signalR from "@microsoft/signalr";

export interface IConnectionContext {
  connection: signalR.HubConnection,
  setConnection: Dispatch<React.SetStateAction<signalR.HubConnection>>,
  addHandler: (identifier: string, handler: (...args: any[])=> any) => void
}

const signalRConnection = new signalR.HubConnectionBuilder()
  .configureLogging(signalR.LogLevel.Information)
  .withAutomaticReconnect()
  .withUrl("//localhost:5208/hub")
  .build();
    
export const ConnectionContext = createContext<IConnectionContext>({
  connection: {} as signalR.HubConnection,
  setConnection: () => null,
  addHandler: () => null,
});

export const ConnectionContextProvider = ({ children }: any) => {
  const [connection, setConnection] = useState(signalRConnection);
  
  const addHandler = async (identifier: string, handler: (...args: any[])=> any) => {
    connection.on(identifier, handler);
    if (connection.state === 'Disconnected') {
      connection.start().catch((err) => {
        return console.log(err);
      });
    }
  };

  const connectionState = { connection, setConnection, addHandler };
  return <ConnectionContext.Provider value={connectionState}>{children}</ConnectionContext.Provider>
};
