import React, { useContext, useEffect, useState } from 'react'
import './LeaveDashboard.css'
import LeaveBalanceMenu from './LeaveDashboardComponents/LeaveBalance'
import { Button, Box } from '@mui/material'
import { useNavigate } from 'react-router-dom'
import axios from 'axios'
import LeavesDataTable from './LeaveDashboardComponents/LeavesTable'
import EmployeeLeaveTable from './LeaveDashboardComponents/EmployeeLeaveTable'
import CircularProgress from '@mui/material/CircularProgress';
import LoadingProgress from '../../../components/LoadingProgress/LoadingProgress';
import AuthContext from '../../../context/AuthProvider'

const LeaveDashboard = () => {
    const navigate = useNavigate();
    const [rows, setRows] = useState([]);
    const [employeeRows, setEmployeeRows] = useState([]);
    const [leavebalance, setLeavebalance] = useState([]);
    const [isLoading, setLoading] = useState(false);
    const [isLoadingg, setLoadingg] = useState(false);
    const { auth } = useContext(AuthContext);


    const handleGetLeaves = () => {
        setLoading(true)
        axios.get(`api/leave/GetLeave/${auth.userId}`)
            .then(response => {
                setRows(response?.data?.requestedObject)
                setLoading(false)
            })
            .catch(error => {
                console.log(error)
                setLoading(false)
            })
    }
    const handleGetEmployeeLeave = () => {
        setLoadingg(true)
        axios.get(`api/leave/GetEmployeeLeave/${auth.userId}`)
            .then(response => {
                console.log(response?.data?.requestedObject, 'Employee Leaves')
                setEmployeeRows(response?.data?.requestedObject)
                setLoadingg(false)
            })
            .catch(error => {
                console.log(error)
                setLoadingg(false)
            })
    }
    const handleGetLeaveBalance = () => {
        axios.get(`api/leave/GetLeaveBalance/${auth.userId}`)
            .then(response => {
                console.log(response.data.requestedObject , 'Leave Balance')
                setLeavebalance(response?.data?.requestedObject)
            })
            .catch(error => {
                console.log(error)
            })
    }

    const handleGetManagerLeaveStatusActions = () => {
        axios.get('api/leave/GetManagerLeaveStatusActions')
            .then(response => {
                console.log(response?.data?.requestedObject, 'GetManagerLeaveStatusActions')
                //setLeaveTypes(response?.data?.requestedObject)
            })
            .catch(error => {
                console.log(error)
            })
    }
    useEffect(() => {
        handleGetLeaves()
        handleGetLeaveBalance()
        handleGetEmployeeLeave()
        handleGetManagerLeaveStatusActions()
    }, [])

  return (
    <div className='leave-dashboard'>
        <div className='leaveboard-head'>
          <div className='leaveboard-options'>
                  <h1>LeaveBoard</h1>
                  <div>
                      <LeaveBalanceMenu rows={leavebalance} />
                  </div>
            <div>
             <Button 
              variant="outlined"
              onClick={()=> navigate('/leaveapplication')}
             >
              Apply a Leave
             </Button>
            </div>
          </div>
          </div>
          <div>
              {
                  isLoading ? <Box><LoadingProgress /></Box> : <LeavesDataTable rows={rows} />
              }
          </div>
          <div>
              {
                  isLoadingg ? <Box><LoadingProgress /></Box> : <EmployeeLeaveTable rows={employeeRows} />
              }
          </div>
    </div>
  )
}

export default LeaveDashboard
