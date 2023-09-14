import React, { useContext, useState, useEffect } from 'react'
import axios from 'axios'
import { ToastContainer, toast } from 'react-toastify';
import Button from '@mui/joy/Button';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import { DemoItem } from '@mui/x-date-pickers/internals/demo';
import Divider from '@mui/material/Divider';
import DialogTitle from '@mui/material/DialogTitle';
import { TextField, FormControl, InputLabel, MenuItem, Select } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';

export default function EditLeaveBalance({ data, leavetypes, employeeDetails, handleUpdate }) {
    const [open, setOpen] = React.useState(false);
    const [isLoading, setLoading] = useState(false);
    const [formData, setFormData] = useState({
        Id: data?.id,
        user: data?.userID,
        leaveType: data?.leaveType,
        balance: data?.available,
    });

    const handleClickOpen = () => {
        setOpen(true);
        console.log(data, 'EditLeaveBalance Data')
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        //const parsedValue = name === 'balance' ? parseInt(value, 10) : value;
        setFormData({
            ...formData,
            [name]: value,
        });
    };
    const handleUpdateClick = () => {
        console.log(formData, 'formData create leavebalance')
        handleUpdate(formData);
        handleClose();
    };


    return (
        <div>
            <Tooltip title="Edit">
                <span>
                    <IconButton
                        //disabled={rowData.leaveStatus !== "Pending" && rowData.leaveStatus !== "Deny"}
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
                            minWidth: '500px'
                        }}
                    >
                        <FormControl className='user'>
                            <InputLabel id="demo-select-small-label">User</InputLabel>
                            <Select
                                label='User'
                                name='user'
                                value={formData.user}
                                onChange={(e) => handleChange(e)}
                                required
                            >
                                {
                                    employeeDetails.map((el, index) => (
                                        <MenuItem key={index} value={el.id}>{el.userName}</MenuItem>
                                    ))
                                }
                            </Select>
                        </FormControl>
                        <FormControl className='leave-type'>
                            <InputLabel id="demo-select-small-label">Leave Type</InputLabel>
                            <Select
                                label='Leave Type'
                                name='leaveType'
                                name="leaveType"
                                value={formData.leaveType}
                                onChange={(e) => handleChange(e)}
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
                            label='Give Balance'
                            type='number'
                            name='balance'
                            value={formData.balance}
                            onChange={(e) => handleChange(e)}
                            required
                        />
                        <Divider />
                        <Button
                            type='submit'
                            onClick={handleUpdateClick}
                        >
                            Update
                        </Button>
                    </div>
                </DialogContent>
            </Dialog>
            <ToastContainer />
        </div>
    );
}