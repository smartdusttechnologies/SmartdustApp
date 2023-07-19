import React, { useContext, useState } from 'react'
import './LeaveApplication.css'
import axios from 'axios'
import {Divider, FormControl, InputLabel, MenuItem, Select, TextField } from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { DemoItem } from '@mui/x-date-pickers/internals/demo';
import Button from '@mui/joy/Button';
import { ToastContainer, toast } from 'react-toastify';
import AuthContext from '../../../context/AuthProvider';

const initialState = {
    leaveFrom: null,
    leaveTill: null,
    leaveType: '',
    reason: ''
}

const LeaveApplication = () => {
    const { auth, setNotification, notification } = useContext(AuthContext);

    const [leaveData, setLeaveData] = useState(initialState);
    const [isLoading, setLoading] = useState(false);


    const handleChange = (event) => {
        const { name, value } = event.target;
        setLeaveData((prevState) => ({
            ...prevState,
            [name]: value
        }));
    };

    const handleLeaveFromChange = (e) => {
        setLeaveData((prevState) => ({
            ...prevState,
            leaveFrom: new Date(e.$d)
        }));
    };

    const handleLeaveTillChange = (e) => {
        setLeaveData((prevState) => ({
            ...prevState,
            leaveTill: new Date(e.$d)
        }));
        console.log(calculateDateDifference(leaveData.leaveTill, leaveData.leaveFrom))
    };

    function calculateDateDifference(leaveTill, leaveFrom) {
        const oneDay = 24 * 60 * 60 * 1000; // One day in milliseconds
        const tillDate = new Date(leaveTill);
        const fromDate = new Date(leaveFrom);

        // Calculate the difference in days
        const diffDays = Math.round(Math.abs((tillDate - fromDate) / oneDay)) + 1;

        return diffDays;
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        setLoading(true)
        axios.post('api/leave/ApplyLeave', {
            id: 0,
            userId: auth.userId,
            userName: auth.userName,
            leaveType: leaveData.leaveType,
            leaveFrom: leaveData.leaveFrom,
            leaveTill: leaveData.leaveTill,
            reason: leaveData.reason,
            appliedDate: new Date(),
            leaveStatus: 'Pending',
            leaveDays: calculateDateDifference(leaveData.leaveTill, leaveData.leaveFrom)
        })
            .then(response => {
                console.log(response?.data.message[0].reason)
                toast.success(response?.data.message[0].reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data.message[0].reason, success: true }])
                setLoading(false)
                setLeaveData(initialState)
            })
            .catch(error => {
                setLoading(false)
                console.log(error)
                toast.error(error?.response?.data.message[0].reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error.response?.data.message[0].reason, success: false }])
            })
    };

    return (
        <div className='leave-application-page'>
            <div className='leave-application-header'>
                <h1>Leave Application</h1>
                <Divider />
            </div>
            <form className='leave-application' onSubmit={(e) => handleSubmit(e)}>
                <div className='date-pickers'>
                    <DemoItem label="Leave From">
                        <DatePicker
                            label={'Leave From'}
                            format="YYYY/MM/DD"
                            value={leaveData.leaveFrom}
                            onChange={(e) => handleLeaveFromChange(e)}
                        />
                    </DemoItem>
                    <DemoItem label="To">
                        <DatePicker
                            label={'Leave To'}
                            format="YYYY/MM/DD"
                            value={leaveData.leaveTill}
                            onChange={(e) => handleLeaveTillChange(e)}
                        />
                    </DemoItem>
                </div>

                <FormControl className='leave-type'>
                    <InputLabel id="demo-select-small-label">Leave Type</InputLabel>
                    <Select
                        size='small'
                        label='Leave Type'
                        name='leaveType'
                        value={leaveData.leaveType}
                        onChange={(e) => handleChange(e)}
                        required
                    >
                        <MenuItem value="MedicalLeave">Medical Leave</MenuItem>
                        <MenuItem value="PaidLeave">Paid Leave</MenuItem>
                        <MenuItem value="UnpaidLeave">Unpaid Leave</MenuItem>
                    </Select>
                </FormControl>

                <TextField
                    label='Reason/Comments'
                    required
                    type='text'
                    sx={{ height: '50px' }}
                    name='reason'
                    value={leaveData.reason}
                    onChange={(e) => handleChange(e)}
                />

                <Button
                    type='submit'
                    loading={isLoading}
                >
                    Submit
                </Button>
            </form>
            <ToastContainer />
        </div>
    )
}

export default LeaveApplication
