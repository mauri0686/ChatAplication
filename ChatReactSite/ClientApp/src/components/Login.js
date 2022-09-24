import React, { useRef, useState, useEffect } from 'react';
import useAuth from '../hooks/useAuth';
import { Link, useNavigate, useLocation } from 'react-router-dom';

import axios from '../api/axios';
import {Button, Form} from "react-bootstrap";
import useAxiosPrivate from "../hooks/useAxiosPrivate";
const LOGIN_URL = '/api/Authentication/Login';

const Login = ({ joinRoom }) => {
    const { setAuth } = useAuth();

    const navigate = useNavigate();
    const location = useLocation();
    const from = location.state?.from?.pathname || "/";
    const userRef = useRef();
    const errRef = useRef();
    const [email, setEmail] = useState('');
    const [password, setPwd] = useState('');
    const [errMsg, setErrMsg] = useState('');
    const [room, setRoom] = useState();
    const [rooms, setRooms] = useState();
    const axiosPrivate = useAxiosPrivate();
   

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
    useEffect(() => {
        userRef.current.focus();
    }, [])

    useEffect(() => {
        setErrMsg('');
    }, [email, password])

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(LOGIN_URL,
                JSON.stringify({ email, password }),
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            )
                const accessToken = response?.data?.token;
                const roles = response?.data?.roles;
                setAuth({ email, password, roles, accessToken });
                setEmail('');
                setPwd('');          
                joinRoom(email, room);
                navigate("/Chat", { replace: true });
        } catch (err) {
            if (!err?.response) {
                setErrMsg('No Server Response');
            } else if (err.response?.status === 400) {
                setErrMsg('Missing Username or Password');
            } else if (err.response?.status === 401) {
                setErrMsg('Unauthorized');
            } else {
                setErrMsg('Login Failed');
            }
            errRef.current.focus();
        }
    }

    return (

        <section className='lobby'>
            <p ref={errRef} className={errMsg ? "errmsg" : "offscreen"} aria-live="assertive">{errMsg}</p>
            <h1 className="text-white">Sign In</h1>

            <Form  onSubmit={handleSubmit}>                  
                <label htmlFor="email" className="text-white">Email:</label>
                <input
                    type="text"
                    id="email"
                    ref={userRef}
                    autoComplete="off"
                    onChange={(e) => setEmail(e.target.value)}
                    value={email}
                    required
                />

                <label htmlFor="password" className="text-login">Password:</label>
                <input
                    type="password"
                    id="password"
                    onChange={(e) => setPwd(e.target.value)}
                    value={password}
                    required
                />
                <label htmlFor="room" className="text-login">Room:</label>
                <Form.Group>
                    {/*<Form.Control placeholder="name" onChange={e => setName(e.target.value)} />   */}
                    <Form.Control  as="select" value={room} onChange={e => setRoom(e.target.value)}>
                        <option>Pick a Room</option>
                        {rooms?.map((r) => <option value={r.id}>{r.name}</option>) }
                    </Form.Control>
                </Form.Group>
                <Button  type="submit" disabled={!email || !room || !password }>SingIn</Button>
            </Form>
            <p className="text-white-50">
                Need an Account?<br />
                <span className="text-login">
                    <Link to="/register">Sign Up</Link>
                </span>
            </p>
            
            
        </section>

    )
}

export default Login
