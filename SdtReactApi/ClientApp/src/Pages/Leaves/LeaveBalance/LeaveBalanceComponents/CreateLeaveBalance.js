import React, { useContext, useState, useEffect } from 'react'
import axios from 'axios'
import { Divider, InputLabel, TextField, Box, FormControl, MenuItem, Select } from '@mui/material'
import { DemoItem } from '@mui/x-date-pickers/internals/demo';
import Button from '@mui/joy/Button';
import { ToastContainer, toast } from 'react-toastify';
import dayjs from 'dayjs';
import Chip from '@mui/material/Chip';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import AddIcon from '@mui/icons-material/Add';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';

const CreateLeaveBalance = ({ leavetypes, handleCreate, employeeDetails }) => {
    const [open, setOpen] = React.useState(false);
    const [formData, setFormData] = useState({
        user: '',
        leaveType: '',
        balance: undefined,
    });
    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };
    const handleCreateClick = () => {
        console.log(formData, 'formData create leavebalance')
        // Validate the form data
        if (formData.user && formData.leaveType && formData.balance > 0) {
            handleCreate(formData);
            handleClose();
        } else {
            toast.warn('Please fill in all required fields and ensure the balance is not smaller than 0.', { position: "bottom-center", theme: "dark" });
        }
    };
    return (
        <div>
            <Tooltip title="Create">
                <span>
                    <IconButton
                        variant="outlined"
                        onClick={handleClickOpen}
                    >
                        <AddIcon />
                    </IconButton>
                </span>
            </Tooltip>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle>Create</DialogTitle>
                <Divider />
                <DialogContent>
                    <div
                        //onSubmit={() => handleCreateClick }
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
                            inputProps={{
                                inputMode: 'numeric',
                                pattern: '^[1-9]\\d*$',
                            }}
                            required
                        />
                        <Divider />
                        <Button
                            type='submit'
                            onClick={() => handleCreateClick()}
                            //loading={isLoading}
                        >
                            Create
                        </Button>
                    </div>
                </DialogContent>
            </Dialog>
            <ToastContainer />
        </div>
    )
}


export default CreateLeaveBalance