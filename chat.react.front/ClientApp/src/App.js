import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css'
import Lobby from "./components/Lobby";
import {HubConnectionBuilder,LogLevel} from '@microsoft/signalr'
import {useState} from "react";
import * as signalR from "@microsoft/signalr";

const App = ()  => {   
    const [conn, setConnection] = useState();
    const [messages, setMessages] = useState([]);
    const joinRoom = async (user,room) => {
        try
        {
            const conn = new HubConnectionBuilder().withUrl("https://localhost:7284/chat" , {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets
            })
                .configureLogging(LogLevel.Information)                
                .build();
            conn.on("Message Receive", (user,message)=>{
                setMessages(messages=> [...messages,{user,message}]);
                console.log('Message Received', message)
            });
            await conn.start();
            await conn.invoke("JoinRoom",{user,room})
            setConnection(conn);
        }catch (e)
        {
            console.log(e);
        }
        
    }
  return (
      <div className='app'>
        <h2> Chat Aplicaction</h2>
        <hr className='line'/>
        <Lobby joinRoom={joinRoom}/>
      </div>
  )
}

export default App;
