import React, { useContext, useEffect, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import './Signup.css'
import axios from 'axios'
import { FormControl, InputLabel, MenuItem, Select, TextField } from '@mui/material'
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import AuthContext from '../../context/AuthProvider'
import Button from '@mui/joy/Button';
import Backdrop from '@mui/material/Backdrop';
import CircularProgress from '@mui/material/CircularProgress';

const signupapi = 'api/security/SignUp';

const Signup = () => {
  const { notification , setNotification} = useContext(AuthContext)
  const navigate = useNavigate()

  const [newuser , setNewuser] = useState({
    firstname:"",
    lastname:"",
    username:"",
    mail:"",
    phone:0,
    country:"",
    org:0,
    password:"",
    confirmpassword:""
  })
  const [organizations , setOrganizations] = useState([]);
  const [isLoading, setLoading] = useState(false);


  const handleChange = (e)=>{
    const newdata = {...newuser}
    newdata[e.target.name] = e.target.value
    setNewuser(newdata)
  }

    const handleSubmit = (e) => {
        e.preventDefault()
        setLoading(true)
        if (validateForm()) {
            axios.post(signupapi, {
                id: 0,
                userName: newuser.username,
                firstName: newuser.firstname,
                lastName: newuser.lastname,
                email: newuser.mail,
                mobile: newuser.phone,
                country: newuser.country,
                isdCode: '',
                mobileValidationStatus: 0,
                orgId: newuser.org,
                password: newuser.password,
                newPassword: newuser.confirmpassword
            })
                .then(response => {
                    const isSuccessful = response?.data.isSuccessful

                    // For Success
                    if (isSuccessful) {
                        const messages = response?.data?.message;

                        if (messages && messages.length > 0) {
                            const newNotifications = [];
                            for (let i = 0; i < messages.length; i++) {
                                if (i < 3) {
                                    toast.success(messages[i].reason, { position: "bottom-center", theme: "colored" });
                                }
                                newNotifications.push({ message: messages[i].reason, success: true });
                            }
                            setNotification([...notification, ...newNotifications]);
                        }
                        setLoading(false)
                        setTimeout(() => {
                            navigate('/login')
                        }, 2000);
                    }

                })
                .catch(error => {
                    setLoading(false)
                    const isSuccessful = error.response?.data.isSuccessful

                    // For Error 
                    if (!isSuccessful) {
                        const messages = error?.response?.data?.message;

                        if (messages && messages.length > 0) {
                            const newNotifications = [];
                            for (let i = 0; i < messages.length; i++) {
                                if (i < 3) {
                                    toast.error(messages[i].reason, { position: "bottom-center", theme: "colored" });
                                }
                                newNotifications.push({ message: messages[i].reason, success: false });
                            }
                            setNotification([...notification, ...newNotifications]);
                        }
                    }
                })
        }
    }

    const validateForm = () => {
        const errors = {};

        if (!/^\d{10}$/.test(newuser.phone)) {
            errors.phone = 'Phone number must be a 10-digit number';
            toast.warn(errors.phone, { position: "bottom-center" });
        }

        return Object.keys(errors).length === 0;
    };

  const handleGetOrganizations = ()=>{
      axios.get('api/home/GetOrganizations')
    .then(response=>{
      setOrganizations(response?.data?.requestedObject)
    })
    .catch(error=>{
    })
  }

  useEffect(()=>{
    handleGetOrganizations()
  },[])

  return (
    <div className='signup-page'>
      <div className='signup-container'>
        <div className='login-header'>
          <div className='login-text-div'>
            <Link  to={'/login'}  style={{textDecoration:'none'}}>
            <p style={{fontSize:"23px", color: 'rgb(62, 61, 61)'}}>Sign in</p>
            </Link>
          </div>
          <div>
            <Link style={{textDecoration:'none'}}>
            <p className='text-signup'>Sign up</p>
            </Link>
          </div>
        </div>
        <form onSubmit={(e)=>handleSubmit(e)} action="">
          <TextField size='small' onChange={(e)=>handleChange(e)} name='firstname' label='Enter FirstName' type="text" required/>
          <TextField size='small' onChange={(e)=>handleChange(e)} name='lastname' label='Enter LastName' type="text" required/>
          <TextField size='small' onChange={(e)=>handleChange(e)} name='username' label='Enter UserName' type="text" required/>
          <TextField size='small' onChange={(e)=>handleChange(e)} name='mail' label='Enter Email' type="email" required/>
          <TextField size='small' onChange={(e)=>handleChange(e)} name='phone' label='Enter MobileNumber' type="number" required/>

          <FormControl>
          <InputLabel id="demo-select-small-label">Country</InputLabel>
            <Select onChange={(e) => handleChange(e)} size='small' label='Country' name="country" required>
              <MenuItem value="india">India</MenuItem>
            </Select>
          </FormControl>

          <FormControl>
          <InputLabel id="demo-select-small-label">SYSORG</InputLabel>
                      <Select onChange={(e) => handleChange(e)} size='small' label='SYSORG' name='org' required>
              {
                organizations.map((el)=>(
                    <MenuItem key={el.id} value={el.id}>{el.orgName}</MenuItem>
                ))
              }
            </Select>
          </FormControl>

          <TextField size='small' onChange={(e)=>handleChange(e)} name='password' label='Enter Password' type="password"  required/>
          <TextField size='small' onChange={(e)=>handleChange(e)} name='confirmpassword' label='Re-Enter Password' type="password"  required/>
          <Button
            type='submit'
            color='success'
            loading={isLoading}
          >
            Sign up
          </Button>
        </form>
        
        <div className='Or-div'>
          <div><hr /></div>
          <div><p>or</p></div>
          <div><hr /></div>
        </div>

        <div className='social-media-login'>
          <a className='Twitter blue-login' href=""><p>Twitter</p></a>
          <a className='Facebook blue-login' href=""><p>Facebook</p></a>
          <a className='Google red-login' href=""><p>Google</p></a>
          <a className='Linked-In blue-login' href=""><p>Linked-In</p></a>
        </div>
      </div>
          <ToastContainer />
          <Backdrop
              sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }}
              open={isLoading}
          >
              <CircularProgress color="inherit" />
          </Backdrop>
    </div>
  )
}

export default Signup
