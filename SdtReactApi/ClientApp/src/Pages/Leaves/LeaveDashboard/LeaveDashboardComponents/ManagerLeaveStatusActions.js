import * as React from 'react';
import Button from '@mui/joy/Button';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import Divider from '@mui/material/Divider';
import DialogTitle from '@mui/material/DialogTitle';
import { Table, TableBody, TableCell, TableHead, TableRow } from '@mui/material';

export default function ManagerLeaveStatusActionsMenu({ rows }) {
    const [open, setOpen] = React.useState(false);

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
                Pending
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
                    <Button color="success">Approve</Button>
                    <Button color="danger">Decline</Button>
                </DialogContent>
            </Dialog>
        </div>
    );
}