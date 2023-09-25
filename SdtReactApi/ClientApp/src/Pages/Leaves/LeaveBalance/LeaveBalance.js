import React, { useContext, useState, useEffect } from 'react'
import axios from 'axios'
import './LeaveBalance.css'
import { Divider, InputLabel, TextField, Box, FormControl, MenuItem, Select } from '@mui/material'
import { DemoItem } from '@mui/x-date-pickers/internals/demo';
import Button from '@mui/joy/Button';
import { ToastContainer, toast } from 'react-toastify';
import dayjs from 'dayjs';
import Chip from '@mui/material/Chip';
import AuthContext from '../../../context/AuthProvider'
import LoadingProgress from '../../../components/LoadingProgress/LoadingProgress';
import CreateLeaveBalance from './LeaveBalanceComponents/CreateLeaveBalance';
import LeavesBalanceTable from './LeaveBalanceComponents/LeaveBalanceTable';

const LeaveBalancePage = () => {
    const [isLoading, setLoading] = useState(false);
    const [leavetypes, setLeaveTypes] = useState([]);
    const [employeeDetails, setEmployeeDetails] = useState([]);
    const [employeeLeaveBalance, setEmployeeLeaveBalance] = useState([]);
    const { auth, setNotification, notification } = useContext(AuthContext);

    const handleCreate = (formData) => {
        //console.log(auth.accessToken, 'auth.accessToken')
        axios.post('api/leave/CreateLeaveBalance', {
            id: 0,
            userId: formData.user,
            leavetype: formData.leaveType,
            available: formData.balance,
            userName: ''
        }, {
            headers: {
                'Authorization': `Bearer ${auth.accessToken}`
            }
        })
            .then(response => {
                console.log(response?.data?.message[0]?.reason)
                handleGetEmployeeLeaveBalance()
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
            })
            .catch(error => {
                console.log(error)
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
            })
    }
    const handleUpdate = (formData) => {

        axios.post('api/leave/UpdateLeaveBalance', {
            id: formData.Id,
            userId: formData.user,
            leavetype: formData.leaveType,
            available: formData.balance,
            userName: ''
        })
            .then(response => {
                console.log(response?.data?.message[0]?.reason)
                handleGetEmployeeLeaveBalance()
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
            })
            .catch(error => {
                console.log(error)
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
            })
    }
    const handleDelete = (id) => {
        axios.post(`api/leave/DeleteLeaveBalance/${id}`)
            .then(response => {
                console.log(response?.data?.message[0]?.reason)
                handleGetEmployeeLeaveBalance()
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
            })
            .catch(error => {
                console.log(error)
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
            })
    }
    const handleGetLeaveTypes = () => {
        axios.get('api/leave/GetLeaveTypes')
            .then(response => {
                console.log(response?.data?.requestedObject)
                setLeaveTypes(response?.data?.requestedObject)
            })
            .catch(error => {
                console.log(error)
            })
    }
    const handleGetEmployeeDetails = () => {
        axios.get(`api/leave/GetEmployeeDetails/${auth.userId}`)
            .then(response => {
                console.log(response?.data?.requestedObject, 'GetEmployeeDetails')
                setEmployeeDetails(response?.data?.requestedObject)
            })
            .catch(error => {
                console.log(error)
            })
    }
    const handleGetEmployeeLeaveBalance = () => {
        setLoading(true)
        axios.get(`api/leave/GetEmployeeLeaveBalance/${auth.userId}`)
            .then(response => {
                console.log(response?.data?.requestedObject, 'GetEmployeeLeaveBalance')
                setEmployeeLeaveBalance(response?.data?.requestedObject)
                setLoading(false)
            })
            .catch(error => {
                console.log(error)
                setLoading(false)
            })
    }

    useEffect(() => {
        handleGetLeaveTypes()
        handleGetEmployeeDetails()
        handleGetEmployeeLeaveBalance()
    }, [])
    return (
        <div className='leave-balance-page'>
            <div className='heading'>
                <h1>Leave Balance</h1>
            </div>
            <div style={{
                display: 'flex',
                justifyContent: 'right',
                width: '90%',
                margin: '5px auto',
            } }>
                <CreateLeaveBalance
                    leavetypes={leavetypes}
                    handleCreate={handleCreate}
                    employeeDetails={employeeDetails}
                />
            </div>
            <div>
                {
                    isLoading ? <Box><LoadingProgress /></Box> : <LeavesBalanceTable rows={employeeLeaveBalance} leavetypes={leavetypes} employeeDetails={employeeDetails} handleUpdate={handleUpdate} handleDelete={handleDelete } />
                }
            </div>
            <ToastContainer />
        </div>
    )
}

export default LeaveBalancePage
