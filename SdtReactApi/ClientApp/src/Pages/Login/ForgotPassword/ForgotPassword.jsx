import React, { useState } from 'react'
import './ForgotPass.css'
import { Button, Input, TextField } from '@mui/material'

const ForgotPassword = () => {
  const [regemail , setEmail] = useState("")
  const [otp , setOtp] = useState(0)

  const handleEmailSubmit = ()=>{
    
  }
  
  return (
    <div className='Forgotpass-body'>
      <div className='forgotpass-container'>
        <div className='Text-forgot-pass'>
            <p>Forgot Password</p>
        </div>
        <div>
            <TextField onChange={(e)=>setEmail(e.target.value)} label="Enter Registered Email" size='small' />
            <Button onClick={handleEmailSubmit} id='send-otp' variant="contained" background-color='red'>Send OTP</Button>
        </div>
        <div>
            <TextField onChange={(e)=>setOtp(e.target.value)}   label="OTP" size='small' type='number' />
            <Button id='confirm-otp' variant="contained" background-color='red'>Confirm</Button>
        </div>
      </div>
    </div>
  )
}

export default ForgotPassword
