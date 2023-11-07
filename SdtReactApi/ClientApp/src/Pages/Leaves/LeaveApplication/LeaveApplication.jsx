import React, { useContext, useState, useEffect } from 'react'
import './LeaveApplication.css'
import axios from 'axios'
import { useNavigate } from 'react-router-dom'
import { Divider, FormControl, InputLabel, MenuItem, Select, TextField } from '@mui/material'
import { DemoItem } from '@mui/x-date-pickers/internals/demo';
import Button from '@mui/joy/Button';
import { ToastContainer, toast } from 'react-toastify';
import AuthContext from '../../../context/AuthProvider';
import dayjs from 'dayjs';
import { MobileDatePicker} from '@mui/x-date-pickers';
import Chip from '@mui/material/Chip';
import Backdrop from '@mui/material/Backdrop';
import CircularProgress from '@mui/material/CircularProgress';

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
    const navigate = useNavigate();

    const { auth, setNotification, notification } = useContext(AuthContext);

    const [leaveData, setLeaveData] = useState(initialState);
    const [isLoading, setLoading] = useState(false);
    const [leavetypes, setLeaveTypes] = useState([]);
    const [file, setFile] = useState([]);

    const handleFileChange = (e) => {
        const filesArray = Array.from(e.target.files);
        setFile(filesArray);
    };
    const handleDeleteFile = (indexToDelete) => {
        setFile(file?.filter((file, index) => index !== indexToDelete));
    };

    const handleChange = (event) => {
        const { name, value } = event.target;
        setLeaveData((prevState) => ({
            ...prevState,
            [name]: value
        }));

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

    };

    // Utility function to check if two dates have the same day, month, and year (ignoring time)
    const isSameDay = (date1, date2) =>
        date1.getDate() === date2.getDate() &&
        date1.getMonth() === date2.getMonth() &&
        date1.getFullYear() === date2.getFullYear();


    
    const handleDeleteDate = (indexToDelete) => {
        setLeaveData((prevState) => ({
            ...prevState,
            leaveDates: prevState?.leaveDates?.filter((date, index) => index !== indexToDelete),
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        setLoading(true)
        const formData = new FormData();

        // Check if there are files to upload
        if (file && file.length > 0) {
            file.forEach(file => {
                const fileName = file.name;
                const fileExtension = fileName.split('.').pop().toLowerCase();
                // Check for allowed file extensions
                if (['jpg', 'jpeg', 'png', 'xlsx', 'pdf'].includes(fileExtension)) {
                    if (file.size <= 1024 * 1024) { // Check file size
                        formData.append('files', file);
                    } else {
                        toast.warn('File size should not exceed 1MB.', { position: "bottom-center" });
                        setLoading(false)
                    }
                } else {
                    toast.warn('Wrong File Type!', { position: "bottom-center" });
                    setLoading(false)
                }
            });

            if (formData.has('files')) {
                // Upload files and get AttachedFileIDs
                axios.post('api/document/FileUpload', formData, {
                    headers: {
                        'Authorization': `${auth.accessToken}`
                    }
                })
                    .then(response => {
                        // Call ApplyLeave with AttachedFileIDs
                        ApplyLeave(response.data);
                    })
                    .catch(error => {
                        console.error(error);
                    });
            }
        } else {
            // Call ApplyLeave without AttachedFileIDs
            ApplyLeave([]);
        }
        

        };

    const ApplyLeave = (attachedFileIDs) => {
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
            comment: '',
            leaveDates: leaveData?.leaveDates,
            attachedFileIDs: attachedFileIDs
        }, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
                setLoading(false)
                setLeaveData(initialState)
            })
            .catch(error => {
                setLoading(false)

                if (error.response && error.response.status === 401) {
                    // Handle 401 Unauthorized error
                    navigate('/unauthorizedpage')
                } else {
                    // Handle other errors
                    toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                    setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
                }
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
                            value={dayjs(leaveData.leaveDates[0])}
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
                                    leaveData?.leaveDates && leaveData?.leaveDates?.length > 0 && leaveData?.leaveDates.map((leave, index) => (
                                        <li key={index}>
                                            <Chip
                                                label={leave.toDateString()}
                                                variant="outlined"
                                                onDelete={() => handleDeleteDate(index)}
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
                        <label htmlFor="fileupload">Choose files</label>
                        <span>
                            {file.length > 0
                                ? <div className="fileslist">
                                    {
                                        file?.map((el, index) => (
                                            <div key={index}>
                                                <Chip
                                                    label={el.name}
                                                    variant="outlined"
                                                    onDelete={() => handleDeleteFile(index)}
                                                />
                                            </div>
                                        ))
                                    }
                                </div>
                                : "No files chosen"}
                        </span>
                        <input 
                            type="file"
                            id="fileupload"
                            multiple
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
            <Backdrop
                sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }}
                open={isLoading}
            >
                <CircularProgress color="inherit" />
            </Backdrop>
        </div>
    )
}

export default LeaveApplication
