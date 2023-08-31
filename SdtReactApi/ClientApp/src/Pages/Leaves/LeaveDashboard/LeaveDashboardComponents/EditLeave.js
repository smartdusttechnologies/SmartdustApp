import React, { useContext, useState, useEffect } from 'react'
import axios from 'axios'
import { ToastContainer, toast } from 'react-toastify';
import Button from '@mui/joy/Button';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import Divider from '@mui/material/Divider';
import DialogTitle from '@mui/material/DialogTitle';
import { TextField, FormControl, InputLabel, MenuItem, Select } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { DateField } from '@mui/x-date-pickers/DateField';
import dayjs from 'dayjs';
import AuthContext from '../../../../context/AuthProvider';

export default function EditLeave({ rowData, handleGetLeaves }) {
    const [open, setOpen] = React.useState(false);
    const { auth, setNotification, notification } = useContext(AuthContext);
    const [updatedLeaveDates, setUpdatedLeaveDates] = useState(rowData?.leaveDates);
    const [updatedLeaveType, setUpdatedLeaveType] = useState(rowData.leaveType);
    const [updatedReason, setUpdatedReason] = useState(rowData.reason);
    const [leavetypes, setLeaveTypes] = useState([]);

    const handleClickOpen = () => {
        setOpen(true);
        console.log(rowData,'editable data')
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleDateChange = (newValue, index) => {
        const updatedDates = [...updatedLeaveDates];
        updatedDates[index] = newValue;
        setUpdatedLeaveDates(updatedDates);
    };

    const handleUpdate = () => {
        console.log(updatedLeaveDates,'updatedLeaveDates')
        console.log(rowData?.leaveDates)
        axios.post('api/leave/UpdateLeave', {
            id: rowData.id,
            userId: auth?.userId,
            userName: auth?.userName,
            leaveType: updatedLeaveType,
            leaveTypeId: 1,
            reason: updatedReason,
            appliedDate: new Date(),
            leaveStatus: rowData.leaveStatus,
            leaveStatusId: 6,
            leaveDays: updatedLeaveDates?.length,
            leaveDates: updatedLeaveDates,
            attachedFileIDs: []
        })
            .then(response => {
                console.log(response?.data?.message[0]?.reason)
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
                handleGetLeaves()
                //setLoading(false)
            })
            .catch(error => {
                //setLoading(false)
                console.log(error)
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
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
        <div>
            <Tooltip title="Edit">
                <span>
                    <IconButton
                        variant="outlined"
                        onClick={handleClickOpen}
                    >
                        <EditIcon />
                    </IconButton>
                </span>
            </Tooltip>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle>Edit</DialogTitle>
                <Divider />
                <DialogContent>
                    <div
                    style={{
                        display: 'flex',
                        flexDirection: 'column',
                            gap: '20px',
                            minWidth: '400px'
                    }} >
                        <div
                            style={{
                                display: 'grid',
                                gridTemplateColumns: 'repeat(2,1fr)',
                                gap:'10px'
                            }} >
                            <LocalizationProvider dateAdapter={AdapterDayjs}>
                                {
                                    updatedLeaveDates.map((el, index) => (
                                        <DemoContainer key={index} components={['DateField']}>
                                            <DateField
                                                value={dayjs(el)}
                                                onChange={(newValue) => handleDateChange(newValue, index)}
                                                format="YYYY/MM/DD"
                                            />
                                        </DemoContainer>
                                    ))
                                }
                            </LocalizationProvider>
                        </div>
                        <FormControl className='leave-type'>
                            <InputLabel id="demo-select-small-label">Leave Type</InputLabel>
                            <Select
                                size='small'
                                label='Leave Type'
                                value={updatedLeaveType}
                                onChange={(e) => setUpdatedLeaveType(e.target.value)}
                                required
                            >
                                {
                                    leavetypes.map((el, index) => (
                                        <MenuItem key={index} value={el.name}>{el.name}</MenuItem>
                                    ))
                                }
                            </Select>
                        </FormControl>
                        <TextField
                            label="Reason"
                            value={updatedReason}
                            onChange={(e) => setUpdatedReason(e.target.value)}
                        />
                    </div>
                </DialogContent>
                <Divider />
                <Button
                    onClick={handleUpdate}
                    sx={{
                        width: '90%',
                        margin: 'auto',
                        mb: '20px',
                        mt:'10px'
                    } }                    
                  >
                    Save
                </Button>
            </Dialog>
            <ToastContainer />
        </div>
    );
}