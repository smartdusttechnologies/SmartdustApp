import React, { useContext, useEffect, useState } from 'react'
import './LeaveDashboard.css'
import {  toast } from 'react-toastify';
import LeaveBalanceMenu from './LeaveDashboardComponents/LeaveBalance'
import {  Box } from '@mui/material'
import { useNavigate } from 'react-router-dom'
import axios from 'axios'
import LeavesDataTable from './LeaveDashboardComponents/LeavesTable';
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
                if (error.response && error.response.status === 401) {
                    // Handle 401 Unauthorized error
                    navigate('/unauthorizedpage')
                }
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
                setLeavebalance(response?.data?.requestedObject)
            })
            .catch(error => {
            })
    }

    const handleGetLeaveTypes = () => {
        axios.get('api/leave/GetLeaveTypes', {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                setLeaveTypes(response?.data?.requestedObject)
            })
            .catch(error => {
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
            comment: '',
            leaveDates: updatedLeaveDates,
            attachedFileIDs: attachedFileIDs
        }, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                handleGetLeaves()
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
            })
            .catch(error => {
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
