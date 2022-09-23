import React, {SyntheticEvent, useState} from 'react';
import {Button, Form } from "react-bootstrap";



const Lobby = ({ joinRoom }) => {
    
    const [email, setEmail] = useState();
    const [room, setRoom] = useState();
    const [password, setPassword] = useState();
    const [redirect, setRedirect] = useState(false);
    const submit = async (e: SyntheticEvent) => {
        e.preventDefault();

        const response = await fetch('https://localhost:7284/api/Authentication/Login', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},          
            body: JSON.stringify({
                email,
                password
            })
        });       
    
        const content = await response.json();
        console.log(content);
        setRedirect(true);
        //props.setName(content.name);
    }

    if (redirect) {
        return <redirect to="/"/>;
        joinRoom(email, room);
    }
    
    return <Form className='lobby' onSubmit={submit}>     
        <Form.Group>
            <Form.Control placeholder="email" onChange={e => setEmail(e.target.value)} />
            <Form.Control placeholder="password" onChange={e => setPassword(e.target.value)} />
            {/*<Form.Control placeholder="room" onChange={e => setRoom(e.target.value)} */}
            <Form.Control  as="select" value={room} onChange={e => setRoom(e.target.value)}>                    
                <option>Pick a Room</option>
                <option value="General">General</option>
                <option value="Finance">Finance</option>
                <option value="Sport">Sport</option>
            </Form.Control> 
        </Form.Group>
            <Button variant="success" type="submit" disabled={!email || !room}>SingIn</Button>
    </Form>
}

export default Lobby;