import React, { useState } from 'react';
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    IconButton,
    Tooltip,
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';

const DeleteComponent = ({ onDelete, id }) => {
    const [open, setOpen] = useState(false);

    const handleDeleteClick = () => {
        setOpen(true);
    };

    const handleConfirmDelete = () => {
        setOpen(false);
        onDelete(id);
    };

    const handleClose = () => {
        setOpen(false);
    };

    return (
        <>
            <Tooltip title="Delete">
                <IconButton onClick={handleDeleteClick}>
                    <DeleteIcon />
                </IconButton>
            </Tooltip>

            <Dialog open={open} onClose={handleClose}>
                <DialogTitle>Confirm Delete</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Are you sure you want to delete this?
                    </DialogContentText>
                </DialogContent>
                <div style={{
                    display: 'flex',
                    justifyContent: 'space-around',
                    marginBottom:'15px'
                } } >
                    <Button onClick={handleConfirmDelete} color="error" variant="contained">
                        Delete
                    </Button>
                    <Button onClick={handleClose} color="primary" variant="contained">
                        Cancel
                    </Button>
                </div>
            </Dialog>
        </>
    );
};

export default DeleteComponent;
