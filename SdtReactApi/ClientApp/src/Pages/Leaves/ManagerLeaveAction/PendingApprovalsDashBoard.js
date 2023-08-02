import React, { useContext, useEffect, useState } from 'react'
import LoadingProgress from '../../../components/LoadingProgress/LoadingProgress';
import axios from 'axios'
import AuthContext from '../../../context/AuthProvider'
import { Button, Box } from '@mui/material'
import EmployeeLeaveTable from './ManagerActionsComponents/EmployeeLeaveTable';
import { ToastContainer, toast } from 'react-toastify';

const PendingApprovalsDashBoard = () => {
    const [isLoading, setLoading] = useState(false);
    const [employeeRows, setEmployeeRows] = useState([]);
    const [actionRows, setActionsRows] = useState([]);
    //const [isLoading, setLoading] = useState(false);
    const { auth, setNotification, notification } = useContext(AuthContext);

    const handleGetEmployeeLeave = () => {
        setLoading(true)
        axios.get(`api/leave/GetEmployeeLeave/${auth.userId}`)
            .then(response => {
                console.log(response?.data?.requestedObject, 'Employee Leaves')
                setEmployeeRows(response?.data?.requestedObject)
                setLoading(false)
            })
            .catch(error => {
                console.log(error)
                setLoading(false)
            })
    }
    const handleGetManagerLeaveStatusActions = () => {
        axios.get('api/leave/GetManagerLeaveStatusActions')
            .then(response => {
                console.log(response?.data?.requestedObject, 'GetManagerLeaveStatusActions')
                setActionsRows(response?.data?.requestedObject)
            })
            .catch(error => {
                console.log(error)
            })
    }

    const handleUpdatestatus = (LeaveID, StatusID, comment ) => {
        axios.post('api/leave/UpdateLeaveStatus', {
            leaveID: LeaveID,
            statusID: StatusID,
            comment: comment
        })
            .then(response => {
                console.log(response?.data?.message[0]?.reason)
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
                handleGetEmployeeLeave()
            })
            .catch(error => {
                console.log(error)
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
            })
    };

    useEffect(() => {
        handleGetEmployeeLeave()
        handleGetManagerLeaveStatusActions()
    }, [])

    return (
        <div className='pending-approvals-dashboard'>
            <div className='pending-approvals-head'>
                <div className='leaveboard-options'>
                    <h1>Pending Approvals</h1>
                </div>
            </div>
            <div>
                {
                    isLoading ? <Box><LoadingProgress /></Box> : <EmployeeLeaveTable rows={employeeRows} actionRows={actionRows} handleUpdatestatus={handleUpdatestatus } />
                }
            </div>
            <ToastContainer />
        </div>
    )
}

export default PendingApprovalsDashBoard
