import React, { useContext, useState } from 'react'
import './Contact.css'
import axios from "axios";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import AuthContext from '../../context/AuthProvider';
import { useNavigate } from 'react-router-dom';

// API Link 
const APIurl = 'api/home/Contactus';

const initialState = {
  name: "",
  mail:"",
  phone:0,
  address:"",
  subject:"",
  message:""
}

const Contact = () => {
  const navigate = useNavigate()
  const {auth , setAuth , notification , setNotification} = useContext(AuthContext)

  // User Details 
  const [userdata , setUserdata] = useState(initialState)

  const handleChange = (e)=>{
    const newdata = {...userdata }
    newdata[e.target.id] = e.target.value
    setUserdata(newdata);
    console.log(userdata);
  }

  const handleSubmit = (e)=>{
    e.preventDefault()
    console.log(userdata)
    axios.post( APIurl , {
    name: userdata.name,
    mail:userdata.mail,
    phone:userdata.phone,
    subject:userdata.subject,
    address:userdata.address,
    message:userdata.message
    })
    .then(response=>{
      console.log(response?.data)
      console.log(response?.data.message[0].reason)
      const isSuccessful = response?.data.requestedObject

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
          theme: "dark",
        });
        setNotification([...notification, {message:response?.data.message[0].reason,success:isSuccessful}])
        setUserdata(initialState)
        // setTimeout(() => {
        //   navigate('/')
        // }, 4000);
      }

      // For Error 
      // if(!isSuccessful){
      //   toast.error('All Fields Are Required',{
      //     position: "bottom-center",
      //     autoClose: 5000,
      //     hideProgressBar: true,
      //     closeOnClick: true,
      //     pauseOnHover: false,
      //     draggable: true,
      //     progress: undefined,
      //     theme: "dark",
      //   });
      //   setNotification([...notification, {message:response?.data.message[0].reason,success:isSuccessful}])
      // }
    })
    .catch(error=>{
      console.log(error?.response.data)
      const isSuccessful = error?.response.data.requestedObject

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
          theme: "dark",
        });
        setNotification([...notification, {message:error.response?.data.message[0].reason,success:isSuccessful}])
      }

      // console.log(error?.response.data?.errors)
      // const errormessages = error?.response.data?.errors
      // if(errormessages){
      //   for(let key in errormessages){
      //     console.log(errormessages[key][0])
      //     let msg = errormessages[key][0]
      //     toast.error(msg,{
      //       position: "bottom-center",
      //       autoClose: 5000,
      //       hideProgressBar: true,
      //       closeOnClick: true,
      //       pauseOnHover: false,
      //       draggable: true,
      //       progress: undefined,
      //       theme: "dark",
      //     });
      //     setNotification([...notification, {message:msg,success:false}])
      //     break;
      //   }
      // }
    })
  }


  return (
    <div className='Contact-body'>
      <div className='Contact-details'>
        <div className='smartdust-details'>
          <p id='title'>GET IN TOUCH</p>
          <p>Bhatta Road, Danapur, Patna - 801503</p>
          <p>rishirodeo@hotmail.com</p>
          <p>7857068847</p>
        </div>

        {/* User Detail Form */}
        <div className='user-details'>
          <form onSubmit={(e)=>handleSubmit(e)} className='user-details-form' > 
          <div className='flex-input-div'>
            <div>
              <label htmlFor="">Name</label> <br />
              <input onChange={(e)=>handleChange(e)} id='name' value={userdata.name} type="text" placeholder='Enter your name' required />
            </div>
            <div>
              <label htmlFor="">Email</label> <br />
              <input onChange={(e)=>handleChange(e)} id='mail' value={userdata.mail} type="email" placeholder='Enter your email' required />
            </div>
          </div>
          <div className='flex-input-div'>
            <div>
              <label htmlFor="">Phone</label> <br />
              <input onChange={(e)=>handleChange(e)} id='phone' value={userdata.phone} type="number" placeholder='Enter your phone number' required />
            </div>
            <div>
              <label htmlFor="">Address</label> <br />
              <input onChange={(e)=>handleChange(e)} id='address' value={userdata.address} type="text" placeholder='Enter your address' required />
            </div>
          </div>
          <div className='long-input'>
            <label htmlFor="">Subject</label> <br />
            <input onChange={(e)=>handleChange(e)} id='subject' value={userdata.subject} type="text" placeholder='Type your subject' required />
          </div>
          <div className='long-input'>
            <label htmlFor="">Message</label> <br />
            <input onChange={(e)=>handleChange(e)} id='message' value={userdata.message} type="text" placeholder='Type your message here' required />
          </div>
          <div  className='submit-user-details'>
            <input type="submit" />
          </div>
          </form>
        </div>
      </div>
      <ToastContainer/>
    </div>
  )
}

export default Contact
