import React, { useContext, useState } from 'react'
import './ChangePassword.css'
import axios from 'axios'
import { TextField } from '@mui/material';
import AuthContext from '../../context/AuthProvider';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { useNavigate } from 'react-router-dom';
import Button from '@mui/joy/Button';

const api = 'api/security/ChangePassword';

const ChangePassword = () => {
  const navigate = useNavigate()
  const {auth ,setAuth ,notification ,setNotification} = useContext(AuthContext)

  const [isLoading, setLoading] = useState(false);
  const [oldPassword , setOldpassword] = useState('');
  const [newPassword , setNewpassword] = useState('');
  const [confirmPassword , setConfirmpassword] = useState('');
  const [msg , setMsg] = useState('');


  const handleSubmit = (e)=>{
    e.preventDefault()
    setLoading(true)
    console.log(oldPassword,newPassword,confirmPassword,auth.userName,auth.userId)

    axios.post(api , {
      oldPassword,
      newPassword,
      confirmPassword,
      userId: auth.userId,
      username: auth.userName
    },{
       headers: {"Authorization" : `${auth.accessToken}`}
    })
    .then(response=>{
      console.log(response?.data)
      console.log(response?.data.message[0].reason)
      const isSuccessful = response?.data.isSuccessful

      // For Success
      if(isSuccessful){
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
          setNotification([...notification, { message: response?.data.message[0].reason, success: isSuccessful }])
          setLoading(false)
      }
    })
    .catch(error =>{
        console.log(error)
        setLoading(false)
      const isSuccessful = error.response?.data.isSuccessful

      // For Error 
      if(!isSuccessful){
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
        setNotification([...notification, {message:error.response?.data.message[0].reason,success:isSuccessful}])
      }

    })
  }


  return (
    <div className='Changepass-body'>
      <div className='Changepass-container'>
        <p>Change Password</p>
        <form  onSubmit={(e)=>handleSubmit(e)}>
          <div className='changepass-inputs'>

          <TextField onChange={(e)=> setOldpassword(e.target.value)} label='OldPassword' required size='small' type="password" />
          <TextField onChange={(e)=> setNewpassword(e.target.value)} label='NewPassword' required size='small' type="password" />
          <TextField onChange={(e)=> setConfirmpassword(e.target.value)} label='ConfirmPassword' required size='small' type="password" />
          </div>
          <div className='changepass-save'>
            <div>
                 <Button
                     loading={isLoading}
                     type='submit'
                     id='save-btn'
                 >
                     Save
                 </Button>
            </div>
            <div>
              <Button onClick={()=> navigate('/')} id='cancel-btn'>Cancel</Button>
            </div>
          </div>
        </form>
      </div>
      <ToastContainer/>
    </div>
  )
}

export default ChangePassword
