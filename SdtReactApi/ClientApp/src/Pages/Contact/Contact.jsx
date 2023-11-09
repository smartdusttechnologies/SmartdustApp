import React, { useContext, useState } from 'react'
import './Contact.css'
import axios from "axios";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import AuthContext from '../../context/AuthProvider';
import LocationMap from '../../components/Localtionmap/Locationmap';
import Button from '@mui/joy/Button';

// API Link 
const APIurl = 'api/home/Contactus';

const initialState = {
  name: "",
  mail:"",
  phone:'',
  address:"",
  subject:"",
  message:""
}

const Contact = () => {
  const {auth  , notification , setNotification} = useContext(AuthContext)

  // User Details 
  const [userdata , setUserdata] = useState(initialState)
  const [isLoading , setLoading] = useState(false)

    const handleChange = (e) => {
        const { id, value } = e.target;
        const newdata = { ...userdata };

        // Check if the field is 'phone' and convert its value to a number
        if (id === 'phone') {
            newdata[id] = parseInt(value, 10);
        } else {
            newdata[id] = value;
        }

        setUserdata(newdata);
    }

  const handleSubmit = (e)=>{
    e.preventDefault()
    
    if(validateForm()){
      setLoading(true)
      axios.post( APIurl , {
      name: userdata.name,
      mail:userdata.mail,
      phone:userdata.phone,
      subject:userdata.subject,
      address:userdata.address,
      message:userdata.message
      }, {
          headers: {
              'Authorization': `${auth.accessToken}`
          }
      })
      .then(response=>{
        const isSuccessful = response?.data.requestedObject

        // For Success
          if (isSuccessful) {
              const messages = response?.data?.message;

              if (messages && messages.length > 0) {
                  const newNotifications = [];
                  for (let i = 0; i < messages.length; i++) {
                      if (i < 3) {
                          toast.success(messages[i].reason, { position: "bottom-center", theme: "dark" });
                      }
                      newNotifications.push({ message: messages[i].reason, success: true });
                  }
                  setNotification([...notification, ...newNotifications]);
              }
          setUserdata(initialState)
          setLoading(false)
        }
      })
      .catch(error=>{
        setLoading(false)

        // For Error 
          if (!error?.response.data.requestedObject) {
              const messages = error?.response?.data?.message;

              if (messages && messages.length > 0) {
                  const newNotifications = [];
                  for (let i = 0; i < messages.length; i++) {
                      if (i < 3) {
                          toast.error(messages[i].reason, { position: "bottom-center", theme: "dark" });
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

		if (!/^[a-zA-Z\s]+$/.test(userdata.name)) {
		  errors.name = 'Name can only contain letters and spaces';
          toast.warn(errors.name,{position: "bottom-center",theme: "dark"});
		}

		if (!/^\d{10}$/.test(userdata.phone)) {
		  errors.phone = 'Phone number must be a 10-digit number';
          toast.warn(errors.phone,{position: "bottom-center",theme: "dark"});
		}

		return Object.keys(errors).length === 0;
	};


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
            <Button
              type='submit'
              loading={isLoading}
            >
              Submit
            </Button>
          </div>
          </form>
        </div>
      </div>
      
      {/* Location Map  */}
      <LocationMap/>

      <ToastContainer/>
    </div>
  )
}

export default Contact
