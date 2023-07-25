import React, { useContext, useState } from 'react'
import './LeaveApplication.css'
import axios from 'axios'
import { Divider, FormControl, InputLabel, MenuItem, Select, TextField } from '@mui/material'
import { DemoItem } from '@mui/x-date-pickers/internals/demo';
import Button from '@mui/joy/Button';
import { ToastContainer, toast } from 'react-toastify';
import AuthContext from '../../../context/AuthProvider';
import dayjs from 'dayjs';
import { MobileDatePicker, StaticDatePicker, DatePicker } from '@mui/x-date-pickers';
import { useEffect } from 'react';

const initialState = {
    leaveDates:[],
    leaveType: '',
    reason: ''
}

const isWeekend = (date) => {
    const day = date.day();

    return day === 0 || day === 6;
};

const LeaveApplication = () => {
    const { auth, setNotification, notification } = useContext(AuthContext);

    const [leaveData, setLeaveData] = useState(initialState);
    const [isLoading, setLoading] = useState(false);
    const [leavetypes, setLeaveTypes] = useState([]);

    const handleChange = (event) => {
        const { name, value } = event.target;
        setLeaveData((prevState) => ({
            ...prevState,
            [name]: value
        }));
        console.log(leaveData)
        console.log(auth)

    };

    const handleLeaveDates = (e) => {
        setLeaveData((prevState) => ({
            ...prevState,
            leaveDates: [...prevState.leaveDates, new Date(e.$d)]
        }));
        console.log(leaveData.leaveDates)
        console.log(leaveData)
    };

    //function calculateDateDifference(leaveTill, leaveFrom) {
    //    const oneDay = 24 * 60 * 60 * 1000; // One day in milliseconds
    //    const tillDate = new Date(leaveTill);
    //    const fromDate = new Date(leaveFrom);

    //    // Calculate the difference in days
    //    const diffDays = Math.round(Math.abs((tillDate - fromDate) / oneDay)) + 1;

    //    return diffDays;
    //};

    const handleSubmit = (e) => {
        e.preventDefault();
        setLoading(true)
        axios.post('api/leave/ApplyLeave', {
            id: 0,
            userId: auth.userId,
            leaveType: leaveData.leaveType,
            reason: leaveData.reason,
            appliedDate: new Date(),
            leaveStatus: 'Pending',
            leaveDays: leaveData.leaveDates.length,
            leaveDates: leaveData.leaveDates
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

    useEffect(() => {
        handleGetLeaveTypes()
    }, [])
    return (
        <div className='leave-application-page'>
            <div className='leave-application-header'>
                <h1>Leave Application</h1>
                <Divider />
            </div>
            <form className='leave-application' onSubmit={(e) => handleSubmit(e)}>
                <div className='date-pickers'>
                    <DemoItem label="Select Dates">
                        <MobileDatePicker
                            label={'Select Dates'}
                            format="YYYY/MM/DD"
                            disablePast
                            shouldDisableDate={isWeekend}
                            closeOnSelect={false}
                            //orientation="landscape"
                            value={leaveData.leaveDates[0]}
                            onChange={(e) => handleLeaveDates(e)}
                        />
                    </DemoItem>
                </div>
                {
                    leaveData.leaveDates.length > 0 && (
                        <div>
                            <h4>Leave Dates:</h4>
                            <ol>
                                {
                                    leaveData.leaveDates.map((leave, index) => (
                                        <li key={index}>{leave.toDateString()}</li>
                                    ))
                                }
                            </ol>
                        </div>)
                }
                <FormControl className='leave-type'>
                    <InputLabel id="demo-select-small-label">Leave Type</InputLabel>
                    <Select
                        // size='small'
                        label='Leave Type'
                        name='leaveType'
                        value={leaveData.leaveType}
                        onChange={(e) => handleChange(e)}
                        required
                    >
                        {
                            leavetypes.map((el) => (
                                <MenuItem key={el} value={el}>{el}</MenuItem>
                            ))
                        }
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
