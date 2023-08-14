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
import Chip from '@mui/material/Chip';

const initialState = {
    leaveDates:[],
    leaveType: 0,
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
    const [file, setFile] = useState(null);

    const handleFileChange = (e) => {
        const selectedFile = e.target.files[0];
        setFile(selectedFile);
        console.log(selectedFile)
    };

    const handleChange = (event) => {
        const { name, value } = event.target;
        setLeaveData((prevState) => ({
            ...prevState,
            [name]: value
        }));
        console.log(leaveData)

    };

    const handleLeaveDates = (e) => {
        const selectedDate = new Date(e?.$d);

        // Check if the selected date already exists in the leaveDates array
        const dateExists = leaveData.leaveDates.some((date) =>
            isSameDay(date, selectedDate)
        );

        // If the date doesn't exist, add it to the leaveDates array
        if (!dateExists) {
            setLeaveData((prevState) => ({
                ...prevState,
                leaveDates: [...prevState.leaveDates, selectedDate],
            }));
        }

        console.log(leaveData.leaveDates);
        console.log(leaveData);
    };
    const handleDeleteDate = (indexToDelete) => {
        setLeaveData((prevState) => ({
            ...prevState,
            leaveDates: prevState?.leaveDates?.filter((date, index) => index !== indexToDelete),
        }));
        console.log(leaveData?.leaveDates, 'Deleted Date')
    };
    // Utility function to check if two dates have the same day, month, and year (ignoring time)
    const isSameDay = (date1, date2) =>
        date1.getDate() === date2.getDate() &&
        date1.getMonth() === date2.getMonth() &&
        date1.getFullYear() === date2.getFullYear();


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
        const reader = new FileReader();

        if (leaveData.leaveType === 1 && file) {
            const reader = new FileReader();

            reader.onload = function (event) {
                const base64Data = event.target.result.split(',')[1]; // Extract the base64 data
                axios.post('api/leave/ApplyLeave', {
                    id: 0,
                    userId: auth?.userId,
                    userName: auth?.userName,
                    leaveType: '',
                    leaveTypeId: leaveData?.leaveType,
                    reason: leaveData?.reason,
                    appliedDate: new Date(),
                    leaveStatus: 'Pending',
                    leaveStatusId: 6,
                    leaveDays: leaveData?.leaveDates?.length,
                    leaveDates: leaveData?.leaveDates,
                    attachedFile: base64Data // Set the base64-encoded file data
                })
                    .then(response => {
                        console.log(response?.data?.message[0]?.reason)
                        toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                        setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
                        setLoading(false)
                        setLeaveData(initialState)
                    })
                    .catch(error => {
                        setLoading(false)
                        console.log(error)
                        toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                        setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
                    })
            };

            // Read the file as base64 data
            reader.readAsDataURL(file);
        } else {
            axios.post('api/leave/ApplyLeave', {
                id: 0,
                userId: auth?.userId,
                userName: auth?.userName,
                leaveType: '',
                leaveTypeId: leaveData?.leaveType,
                reason: leaveData?.reason,
                appliedDate: new Date(),
                leaveStatus: 'Pending',
                leaveStatusId: 6,
                leaveDays: leaveData?.leaveDates?.length,
                leaveDates: leaveData?.leaveDates,
                attachedFile: '' // Set the attachedFile to null when not applicable
            })
                .then(response => {
                    console.log(response?.data?.message[0]?.reason)
                    toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                    setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
                    setLoading(false)
                    setLeaveData(initialState)
                })
                .catch(error => {
                    setLoading(false)
                    console.log(error)
                    toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                    setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
                })
        }
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
                            <ol className="leavedateslist">
                                {
                                    leaveData.leaveDates.map((leave, index) => (
                                        <li key={index}>
                                            <Chip
                                                label={leave.toDateString()}
                                                variant="outlined"
                                                //onDelete={() => handleDeleteDate(index)}
                                            />
                                        </li>
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
                        //value={leaveData.leaveType}
                        onChange={(e) => handleChange(e)}
                        required
                    >
                        {
                            leavetypes.map((el,index) => (
                                <MenuItem key={index} value={el.id}>{el.name}</MenuItem>
                            ))
                        }
                    </Select>
                </FormControl>
                {leaveData.leaveType === 1 && (
                    <div className="documentupload">
                        <label for="fileupload">Choose a file</label>
                        <span>{file ? file.name : "No file chosen"}</span>
                        <input 
                            type="file"
                            
                            id="fileupload"
                            //accept="image/*"
                            onChange={(e) => handleFileChange(e)}
                        />
                    </div>
                )}
                <TextField
                    label='Reason/Comments'
                    required
                    multiline
                    size='large'
                    type='text'
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
