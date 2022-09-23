import React, {SyntheticEvent, useEffect, useState} from 'react';
import {Button, Form } from "react-bootstrap";
import {axiosPrivate} from "../api/axios";
import {useLocation, useNavigate} from "react-router-dom";
import useAxiosPrivate from "../hooks/useAxiosPrivate";


const Lobby = ({ joinRoom }) => {
    const [name, setName] = useState();
    const [room, setRoom] = useState();
    const [rooms, setRooms] = useState();
    const axiosPrivate = useAxiosPrivate();
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();   
    const getRooms = async () => {
        try {
            const response = await axiosPrivate.get('/api/Rooms', {
                signal: controller.signal
            });
            console.log(response.data);
            isMounted && setRooms(response.data);
            
        } catch (err) {
            console.error(err);
            navigate('/login', { state: { from: location }, replace: true });
        }
    }
    getRooms();
        return () => {
            isMounted = false;
            controller.abort();
        }
    }, [])
    return <Form className='lobby'
                 onSubmit={e => {
                     e.preventDefault();
                     joinRoom(name, room);
                 }} >
        <Form.Group>
            <Form.Control placeholder="name" onChange={e => setName(e.target.value)} />   
            <Form.Control  as="select" value={room} onChange={e => setRoom(e.target.value)}>                    
                <option>Pick a Room</option>
                {rooms?.map((r) => <option value={r.id}>{r.name}</option>) }         
            </Form.Control> 
        </Form.Group>
            <Button  type="submit" disabled={!name || !room}>SingIn</Button>
    </Form>
}

export default Lobby;
