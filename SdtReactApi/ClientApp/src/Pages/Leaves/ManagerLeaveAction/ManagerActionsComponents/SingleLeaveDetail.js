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
        console.log(rows, 'Single Leave Details')
        console.log(rows.attachedFile, 'Base 64')
    };

    const handleClose = () => {
        setOpen(false);
    };

    const downloadFile = (fileData, fileName) => {
        const blob = new Blob([fileData], { type: 'image/png' });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = fileName;
        link.click();
        // Clean up the URL.createObjectURL
        URL.revokeObjectURL(url);
    };

    return (
        <div>
            <Tooltip title="Details">
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
                <DialogTitle sx={{ width:'350px' }} >Leave Details</DialogTitle>
                <Divider />
                {rows.attachedFile && (
                    <div>
                        <Button
                            sx={{
                                maxWidth: '200px',
                                margin: '15px'
                            }}
                            onClick={() => downloadFile(rows.attachedFile, 'attached-file.png')}
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