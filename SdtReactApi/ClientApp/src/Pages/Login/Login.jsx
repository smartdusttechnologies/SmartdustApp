import React, { useContext, useState } from 'react'
import './Login.css'
import { Link, Navigate, useNavigate } from 'react-router-dom'
import axios from 'axios';
import AuthContext from '../../context/AuthProvider';
import { TextField } from '@mui/material';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const loginurl = 'api/security/login';

const Login = () => {
  const navigate = useNavigate()
  const {auth , setAuth , notification , setNotification} = useContext(AuthContext)

  const [email , setEmail] = useState('');
  const [password , setPassword] = useState('');

  
  const handleSubmit = (e)=>{
    e.preventDefault()
    
      axios.post(loginurl ,
      {
        userName:email,
        password:password
      }        
      )
      .then(response=> {
        console.log(response?.data)
        console.log(response?.data.message[0].reason)
        const isAuthenticated = response?.data.isSuccessful

        // For Success
        if(isAuthenticated){
          const accessToken = response?.data.requestedObject.accessToken 
          const userName = response?.data.requestedObject.userName
          const userId = response?.data.requestedObject.userId
          console.log(accessToken,userName,userId)
          setAuth({accessToken , userName , userId , isAuthenticated})
          toast.success(response?.data.message[0].reason,{
            position: "bottom-center",
            autoClose: 5000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: false,
            draggable: true,
            progress: undefined,
            theme: "colored",
          });
          setNotification([...notification, {message:response?.data.message[0].reason,success:isAuthenticated}])

          setEmail('')
          setPassword('')
          
          setTimeout(() => {
            navigate('/')
          }, 3000);
        }
      })
      .catch(error=>{
        console.log(error.response.data)
        const isAuthenticated = error?.response?.data.isSuccessful

        // For Error 
        if(!isAuthenticated){
          toast.error(error.response?.data.message[0].reason,{
            position: "bottom-center",
            autoClose: 5000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: false,
            draggable: true,
            progress: undefined,
            theme: "colored",
          });
          setNotification([...notification, {message:error.response?.data.message[0].reason,success:isAuthenticated}])
        }
      })
  }


  return (
    <div className='login-page'>
      <div className='login-container'>
        <div className='login-header'>
          <div className='login-text-div'>
            <Link style={{textDecoration:'none' , color:"blue"}}>
            <p className='text-login'>Sign in</p>
            </Link>
          </div>
          <div>
            <Link to={'/signup'} style={{textDecoration:'none'}}>
            <p style={{fontSize:"23px", color: 'rgb(62, 61, 61)'}}>Sign up</p>
            </Link>
          </div>
        </div>
        <form onSubmit={(e)=>handleSubmit(e)} action="">
          <TextField size='small'
           onChange={(e)=>setEmail(e.target.value)}
           label='Username or Email ID' type="text"
           value={email}
           required
          />
          <TextField size='small'
           onChange={(e)=>setPassword(e.target.value)}
           label='Password' type="password"
           value={password}
           required
          />
          <div className='remeber-me-forgot-pass'>
           <div> <label htmlFor="">Remember me</label> <input type="checkbox" /></div>
          <Link to={'/forgotpassword'} className='forgot-pass'>Forgot password</Link>
          </div>
          <input className='submit-btn' type="submit" value={'Sign in'}/>
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
        <div className='sign-up'>
          <Link to={'/signup'}>Don't have an account? Sign Up</Link>
        </div>
        <div className='why-box'>
          <p class="why">Why Create an Account?</p>
          <div>
              <p class="ans">
                  By creating the account you agreed to our
                  <a>Privacy Policy</a>&
                  <a>Cookie Policy</a>
              </p>
          </div>
        </div>
      </div>
      <ToastContainer/>
    </div>
  )
}

export default Login
