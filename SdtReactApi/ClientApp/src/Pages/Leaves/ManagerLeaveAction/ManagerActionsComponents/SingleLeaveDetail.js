import React, { useContext, useState } from 'react'
import Button from '@mui/joy/Button';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import Divider from '@mui/material/Divider';
import DialogTitle from '@mui/material/DialogTitle';
import { TextField } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import VisibilityIcon from '@mui/icons-material/Visibility';
import Typography from '@mui/material/Typography';

export default function SingleLeaveDetail({ rows }) {
    const [open, setOpen] = React.useState(false);
    const [comment, setComment] = React.useState('');

    const handleClickOpen = () => {
        setOpen(true);
        console.log(rows , 'Single Leave Details')
    };

    const handleClose = () => {
        setOpen(false);
    };

    return (
        <div>
            <Tooltip title="Edit Status">
                <IconButton variant="outlined" onClick={handleClickOpen}
                >
                    <VisibilityIcon />
                </IconButton>
            </Tooltip>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle sx={{ width:'300px' }} >Leave Details</DialogTitle>
                <Divider />
                {rows.attachedFile && (
                    <div>
                        <Button
                            sx={{
                                maxWidth: '200px',
                                margin: '20px'
                            }}
                        >
                            Download Document
                        </Button>
                        <Divider />
                    </div>
                )}
                <DialogContent>
                    <div>
                        <Typography>User Name : {rows.userName}</Typography>
                    </div>
                    <div>
                        <Typography>Applied Date : {new Date(rows.appliedDate).toLocaleDateString()}</Typography>
                        <Typography>Leave in Days : {rows.leaveDays} {rows.leaveDays > 1 ? 'Days' : 'Day' }</Typography>
                        <Typography>Leave Type : {rows.leaveType}</Typography>
                        <Typography>Leave Status : {rows.leaveStatus}</Typography>
                        <Typography>Reason : {rows.reason}</Typography>
                    </div>
                </DialogContent>
            </Dialog>
        </div>
    );
}