import {useEffect, useState} from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import Lobby from './components/Lobby';
import Chat from './components/Chat';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import * as signalR from "@microsoft/signalr";
import {BrowserRouter, Route} from "react-router-dom";
import {Nav} from "react-bootstrap";
import Login from "./components/Login";
import Register from "./components/Register";

const App = () => {
  const [connection, setConnection] = useState();
  const [messages, setMessages] = useState([]);
  const [users, setUsers] = useState([]);

  const joinRoom = async (user, room) => {
    try {
      const connection = new HubConnectionBuilder()
          .withUrl("https://localhost:7284/chat",
              { skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
              })
          .configureLogging(LogLevel.Information)
          .build();

      connection.on("ReceiveMessage", (user, message) => {
        setMessages(messages => [...messages, { user, message }]);
      });

      connection.on("UsersInRoom", (users) => {
        setUsers(users);
      });

      connection.onclose(e => {
        setConnection();
        setMessages([]);
        setUsers([]);
      });

      await connection.start();
      await connection.invoke("JoinRoom", { user, room });
      setConnection(connection);
    } catch (e) {
      console.log(e);
    }
  }

  const sendMessage = async (message) => {
    try {
      await connection.invoke("SendMessage", message);
    } catch (e) {
      console.log(e);
    }
  }

  const closeConnection = async () => {
    try {
      await connection.stop();
    } catch (e) {
      console.log(e);
    }
  }
 
  return  <div className="flex min-h-screen flex-col justify-center py-2 h-screen w-screen">
    
      <div className='app'>
      <h2 className="text-white">Chat Aplication</h2>
      <hr className='line' />
        
      {!connection 
        ? <Lobby joinRoom={joinRoom} />
        : <Chat sendMessage={sendMessage} messages={messages} users={users} closeConnection={closeConnection} />}
    </div>
  </div>
}

export default App;
