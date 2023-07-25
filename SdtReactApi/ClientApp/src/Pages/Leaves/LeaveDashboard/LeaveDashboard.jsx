import React, { useContext, useEffect, useState } from 'react'
import './LeaveDashboard.css'
import LeaveBalanceMenu from './LeaveBalance'
import { Button,Box } from '@mui/material'
import { useNavigate } from 'react-router-dom'
import axios from 'axios'
import LeavesDataTable from './LeavesTable'
import CircularProgress from '@mui/material/CircularProgress';
import LoadingProgress from '../../../components/LoadingProgress/LoadingProgress';
import AuthContext from '../../../context/AuthProvider'

const LeaveDashboard = () => {
    const navigate = useNavigate();
    const [rows, setRows] = useState([]);
    const [leavebalance, setLeavebalance] = useState([]);
    const [isLoading, setLoading] = useState(false);
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
    const handleGetLeaveBalance = () => {
        axios.get(`api/leave/GetLeaveBalance/${auth.userId}`)
            .then(response => {
                console.log(response.data.requestedObject)
                setLeavebalance(response?.data?.requestedObject)
            })
            .catch(error => {
                console.log(error)
            })
    }
    useEffect(() => {
        handleGetLeaves()
        handleGetLeaveBalance()
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
    </div>
  )
}

export default LeaveDashboard
