import React, {SyntheticEvent, useState} from 'react';
import {Button, Form } from "react-bootstrap";


const Lobby = ({ joinRoom }) => {
    const [name, setName] = useState();
    const [room, setRoom] = useState();

    return <Form className='lobby'
                 onSubmit={e => {
                     e.preventDefault();
                     joinRoom(name, room);
                 }} >
        <Form.Group>
            <Form.Control placeholder="name" onChange={e => setName(e.target.value)} />   
            <Form.Control  as="select" value={room} onChange={e => setRoom(e.target.value)}>                    
                <option>Pick a Room</option>
                <option value="General">General</option>
                <option value="Finance">Finance</option>
                <option value="Sport">Sport</option>
            </Form.Control> 
        </Form.Group>
            <Button variant="success" type="submit" disabled={!name || !room}>SingIn</Button>
    </Form>
}

export default Lobby;
