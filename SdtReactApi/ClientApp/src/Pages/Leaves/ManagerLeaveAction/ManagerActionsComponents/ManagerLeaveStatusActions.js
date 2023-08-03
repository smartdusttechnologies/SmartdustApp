import React, { useContext, useState } from 'react'
import Button from '@mui/joy/Button';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import Divider from '@mui/material/Divider';
import DialogTitle from '@mui/material/DialogTitle';
import { TextField } from '@mui/material'

export default function ManagerLeaveStatusActionsMenu({ rows, leaveStatus, LeaveID, handleUpdatestatus }) {
    const [open, setOpen] = React.useState(false);
    const [comment, setComment] = React.useState('');

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    return (
        <div>
            <Button variant="outlined" onClick={handleClickOpen}
            >
                {leaveStatus}
            </Button>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle>Take the necessary action accordingly</DialogTitle>
                <Divider />
                <DialogContent>
                    <div style={{ display: 'flex', justifyContent: 'center'}}>
                        <TextField
                            label='Reason/Comments'
                            required
                            type='text'
                            size='small'
                            name='comment'
                            value={comment}
                            onChange={(e) => setComment(e.target.value)}
                        />
                    </div>
                    <div style={{ display: 'flex', justifyContent: 'space-around', marginTop: '10px'}}>
                       {
                        rows.filter((row) => row.id !== 6).map((row, index) => (
                            <Button
                                key={index}
                                color={row.id === 5 ? 'success' : 'danger'}
                                onClick={() => handleUpdatestatus(LeaveID, row.id, comment )}
                            >
                                {row.name}
                            </Button>
                        ))
                        }
                    </div>
                </DialogContent>
            </Dialog>
        </div>
    );
}