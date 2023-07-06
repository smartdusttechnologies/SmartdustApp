import React from 'react'
import {Route, Routes} from 'react-router-dom'
import Home from '../../Pages/Home/Home'
import Contact from '../../Pages/Contact/Contact'
import About from '../../Pages/About/About'
import Login from '../../Pages/Login/Login'
import Signup from '../../Pages/Signup/Signup'
import ChangePassword from '../../Pages/ChangePassword/ChangePassword'
import ForgotPassword from '../../Pages/Login/ForgotPassword/ForgotPassword'
import PrivateRoute from './PrivateRoute'

const AllRoutes = () => {
  return (
    <Routes>
      <Route path='/' element={<Home/>}></Route>
      <Route path='/contact' element={<Contact/>}></Route>
      <Route path='/about' element={<About/>}></Route>
      <Route path='/login' element={<Login/>}></Route>
      <Route path='/signup' element={<Signup/>}></Route>
      <Route
        path='/changepassword'
        element={
          <PrivateRoute>
            <ChangePassword/>
          </PrivateRoute>
        }
      ></Route>
      <Route path='/forgotpassword' element={<ForgotPassword/>}></Route>
    </Routes>
  )
}

export default AllRoutes
