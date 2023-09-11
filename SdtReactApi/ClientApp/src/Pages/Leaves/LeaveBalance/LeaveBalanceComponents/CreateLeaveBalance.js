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
import EditIcon from '@mui/icons-material/Edit';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';

const CreateLeaveBalance = ({ leavetypes, handleCreate, employeeDetails }) => {
    const [open, setOpen] = React.useState(false);
    const [formData, setFormData] = useState({
        user: '',
        leaveType: '',
        balance: null,
    });
    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        const parsedValue = name === 'balance' ? parseInt(value, 10) : value;
        setFormData({
            ...formData,
            [name]: parsedValue,
        });
    };
    const handleCreateClick = () => {
        console.log(formData, 'formData create leavebalance')
        handleCreate(formData);
        //handleClose();
    };
    return (
        <div>
            <Tooltip title="Create">
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
                <DialogTitle>Create</DialogTitle>
                <Divider />
                <DialogContent>
                    <form
                        onSubmit={handleCreateClick }
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
                            inputProps={{
                                inputMode: 'numeric',
                                pattern: '^[1-9]\\d*$',
                            }}
                            required
                        />
                        <Divider />
                        <Button
                            type='submit'
                            //onClick={handleCreateClick}
                            //loading={isLoading}
                        >
                            Create
                        </Button>
                    </form>
                </DialogContent>
            </Dialog>
            <ToastContainer />
        </div>
    )
}


export default CreateLeaveBalance