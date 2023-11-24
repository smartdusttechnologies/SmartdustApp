import React, { useState } from 'react'
import { ToastContainer } from 'react-toastify';
import Button from '@mui/joy/Button';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import Divider from '@mui/material/Divider';
import DialogTitle from '@mui/material/DialogTitle';
import {  FormControl, InputLabel, MenuItem, Select } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';

export default function Edit({ data, users, handleUpdate }) {
    const [open, setOpen] = React.useState(false);
    const [formData, setFormData] = useState({
        Id: data?.id,
        setemployee: data?.employeeID,
        setmanager: data?.managerID
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
    const handleUpdateClick = () => {
        handleUpdate(formData);
    };


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
                            minWidth: '500px'
                        }}
                    >
                        <FormControl>
                            <InputLabel id="demo-select-small-label">Employee</InputLabel>
                            <Select
                                label='Select Employee'
                                name='setemployee'
                                value={formData.setemployee}
                                onChange={(e) => handleChange(e)}
                                required
                            >
                                {
                                    users.map((el, index) => (
                                        <MenuItem key={index} value={el.id}>{el.userName}</MenuItem>
                                    ))
                                }
                            </Select>
                        </FormControl>
                        <FormControl>
                            <InputLabel id="demo-select-small-label">Manager</InputLabel>
                            <Select
                                label='Select Manager'
                                name='setmanager'
                                value={formData.setmanager}
                                onChange={(e) => handleChange(e)}
                                required
                            >
                                {
                                    users.map((el, index) => (
                                        <MenuItem key={index} value={el.id}>{el.userName}</MenuItem>
                                    ))
                                }
                            </Select>
                        </FormControl>
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