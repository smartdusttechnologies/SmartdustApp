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

const Create = ({ users, handleCreate }) => {
    const [open, setOpen] = React.useState(false);
    const [formData, setFormData] = useState({
        setemployee: undefined,
        setmanager: undefined
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
        // Validate the form data
        if (formData.setemployee && formData.setmanager) {
            handleCreate(formData);
            handleClose();
        } else {
            toast.warn('Please Select all required fields.', { position: "bottom-center", theme: "dark" });
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
                        <FormControl>
                            <InputLabel id="demo-select-small-label">Select Employee</InputLabel>
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
                            <InputLabel id="demo-select-small-label">Select Manager</InputLabel>
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


export default Create