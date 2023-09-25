import React, { useContext, useEffect, useState } from 'react'
import './LeaveDashboard.css'
import { ToastContainer, toast } from 'react-toastify';
import LeaveBalanceMenu from './LeaveDashboardComponents/LeaveBalance'
import { Button, Box } from '@mui/material'
import { useNavigate } from 'react-router-dom'
import axios from 'axios'
import LeavesDataTable from './LeaveDashboardComponents/LeavesTable';
import CircularProgress from '@mui/material/CircularProgress';
import LoadingProgress from '../../../components/LoadingProgress/LoadingProgress';
import AuthContext from '../../../context/AuthProvider'

const LeaveDashboard = () => {
    const navigate = useNavigate();
    const [rows, setRows] = useState([]);
    const [leavebalance, setLeavebalance] = useState([]);
    const [isLoading, setLoading] = useState(false);
    const [leavetypes, setLeaveTypes] = useState([]);
    const { auth, setNotification, notification } = useContext(AuthContext);


    const handleGetLeaves = () => {
        setLoading(true)
        axios.get(`api/leave/GetLeave/${auth.userId}`, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
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
        axios.get(`api/leave/GetLeaveBalance/${auth.userId}`, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                console.log(response.data.requestedObject , 'Leave Balance')
                setLeavebalance(response?.data?.requestedObject)
            })
            .catch(error => {
                console.log(error)
            })
    }

    const handleGetLeaveTypes = () => {
        axios.get('api/leave/GetLeaveTypes', {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                console.log(response?.data?.requestedObject, 'GetLeaveTypes')
                setLeaveTypes(response?.data?.requestedObject)
            })
            .catch(error => {
                console.log(error)
            })
    };

    const UpdateLeave = (attachedFileIDs, id, updatedLeaveType, updatedReason, updatedLeaveDates) => {
        axios.post('api/leave/UpdateLeave', {
            id: id,
            userId: auth?.userId,
            userName: auth?.userName,
            leaveType: '',
            leaveTypeId: updatedLeaveType,
            reason: updatedReason,
            appliedDate: new Date(),
            leaveStatus: '',
            leaveStatusId: 6,
            leaveDays: updatedLeaveDates?.length,
            leaveDates: updatedLeaveDates,
            attachedFileIDs: attachedFileIDs
        }, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                console.log(response?.data?.message[0]?.reason)
                handleGetLeaves()
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
            })
            .catch(error => {
                console.log(error)
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
            })

    }

    useEffect(() => {
        handleGetLeaves()
        handleGetLeaveBalance()
        handleGetLeaveTypes()
    }, [])

  return (
    <div className='leave-dashboard'>
        <div className='leaveboard-head'>
          <div className='leaveboard-options'>
                  <h1>LeaveBoard</h1>
                  <div>
                      <LeaveBalanceMenu rows={leavebalance} />
                  </div>
          </div>
          </div>
          <div>
              {
                  isLoading ? <Box><LoadingProgress /></Box> : <LeavesDataTable rows={[...rows].reverse()} leavetypes={leavetypes} UpdateLeave={UpdateLeave } />
              }
          </div>
    </div>
  )
}

export default LeaveDashboard
