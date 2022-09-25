import { useState} from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import Chat from './components/Chat';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import * as signalR from "@microsoft/signalr";
import Login from "./components/Login";
import {Route, Routes, useLocation, useNavigate} from "react-router-dom";
import Register from "./components/Register";
import RequireAuth from "./components/RequireAuth";
import useAuth from "./hooks/useAuth";
import {axiosPrivate} from "./api/axios";
require('dotenv').config()

 
const App = () => {
  const [connection, setConnection] = useState();
  const [messages, setMessages] = useState([]);
  const [users, setUsers] = useState([]);
  const { setAuth } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const getOldMessages = async (roomId) => {
    try {
   
      const controller = new AbortController();
      const response = await axiosPrivate.get('/api/Messages/' + roomId, {
        signal: controller.signal
      });
      
      console.log(response.data);   
      response.data.map((m, index) => {        
        const user= m.user.userName;
        const message = m.message;
        setMessages(messages => [...messages, { user, message }]);        
      });

    } catch (err) {
      console.error(err);
      navigate('/', { state: { from: location }, replace: true });
    }
  }
  const joinRoom = async (userEmail, roomId ) => {
    try {
      const connection = new HubConnectionBuilder()
          .withUrl(process.env.REACT_APP_CHAT_URL,
              { skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
              })
          .configureLogging(LogLevel.Information)
          .build();
      
      await getOldMessages(roomId);
      
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
      console.log(roomId);
      await connection.invoke("JoinRoom", { userEmail, roomId });
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
      setAuth([]);
      navigate('/login',  {replace: true });
    } catch (e) {
      console.log(e);
    }
  }

  return <div className="flex min-h-screen flex-col justify-center py-2 h-screen w-screen">
    <div className='app'>
      <h2 className="text-white">Chat Aplication</h2>
      <hr className='line' />
      <Routes>
        <Route path="/" element={<Login joinRoom={joinRoom} />} />   
        
          <Route path="register" element={<Register />} /> 
        
          <Route element={<RequireAuth/>}>
            <Route path="/Chat" element={  <Chat sendMessage={sendMessage} messages={messages} users={users} closeConnection={closeConnection} />}>
          </Route>

        </Route>
        </Routes>
    </div>
  </div>
  
}

export default App;
