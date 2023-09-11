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
import LeaveDashboard from '../../Pages/Leaves/LeaveDashboard/LeaveDashboard'
import LeaveApplication from '../../Pages/Leaves/LeaveApplication/LeaveApplication'
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import LeaveReport from '../../Pages/Leaves/LeaveReport/LeaveReport'
import LeaveBalancePage from '../../Pages/Leaves/LeaveBalance/LeaveBalance'

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
      <Route
        path='/leavedashboard'
        element={
          <PrivateRoute>
            <LeaveDashboard/>
          </PrivateRoute>
        }
      ></Route>
      <Route
        path='/leaveapplication'
        element={
            <PrivateRoute>
                <LocalizationProvider dateAdapter={AdapterDayjs}>
                  <LeaveApplication/>
                </LocalizationProvider>
            </PrivateRoute>
        }
          ></Route>
          <Route
              path='/leavereport'
              element={
                  <PrivateRoute>
                      <LocalizationProvider dateAdapter={AdapterDayjs}>
                          <LeaveReport />
                      </LocalizationProvider>
                  </PrivateRoute>
              }
          ></Route>
          <Route
              path='/leavebalance'
              element={
                  <PrivateRoute>
                          <LeaveBalancePage />
                  </PrivateRoute>
              }
          ></Route>
    </Routes>
  )
}

export default AllRoutes
