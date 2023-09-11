import React, { useContext, useState, useEffect } from 'react'
import axios from 'axios'
import './LeaveBalance.css'
import { Divider, InputLabel, TextField, Box, FormControl, MenuItem, Select } from '@mui/material'
import { DemoItem } from '@mui/x-date-pickers/internals/demo';
import Button from '@mui/joy/Button';
import { ToastContainer, toast } from 'react-toastify';
import dayjs from 'dayjs';
import Chip from '@mui/material/Chip';
import { MobileDatePicker, StaticDatePicker, DatePicker } from '@mui/x-date-pickers';
import AuthContext from '../../../context/AuthProvider'
import LoadingProgress from '../../../components/LoadingProgress/LoadingProgress';
import CreateLeaveBalance from './LeaveBalanceComponents/CreateLeaveBalance';

const LeaveBalancePage = () => {
    const [isLoading, setLoading] = useState(false);
    const [leavetypes, setLeaveTypes] = useState([]);
    const [employeeDetails, setEmployeeDetails] = useState([]);
    const { auth, setNotification, notification } = useContext(AuthContext);

    const handleCreate = (formData) => {

        axios.post('api/leave/CreateLeaveBalance', {
            userId: formData.user,
            leavetype: formData.leaveType,
            available: formData.balance
        })
            .then(response => {
                console.log(response?.data?.message[0]?.reason)
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
                //setLoading(false)
                //setLeaveData(initialState)
            })
            .catch(error => {
                //setLoading(false)
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

    useEffect(() => {
        handleGetLeaveTypes()
        handleGetEmployeeDetails()
    }, [])
    return (
        <div className='leave-balance-page'>
            <div className='heading'>
                <h1>Leave Balance</h1>
            </div>
            <CreateLeaveBalance
                leavetypes={leavetypes}
                handleCreate={handleCreate}
                employeeDetails={employeeDetails }
            />
            <ToastContainer />
        </div>
    )
}

export default LeaveBalancePage
