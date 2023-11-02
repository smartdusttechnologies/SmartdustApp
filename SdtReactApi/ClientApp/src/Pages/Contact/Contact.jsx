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
    console.log(userdata)
    
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
          setLoading(false)
        }
      })
      .catch(error=>{
        setLoading(false)

        // For Error 
        if(!error?.response.data.requestedObject){
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
          setNotification([...notification, {message : error.response?.data.message[0].reason , success : error?.response.data.requestedObject}])
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
