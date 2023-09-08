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
import Chip from '@mui/material/Chip';
import axios from 'axios'
import dayjs from 'dayjs';
import DownloadIcon from '@mui/icons-material/Download';

export default function SingleLeaveDetail({ rows }) {
    const [open, setOpen] = React.useState(false);

    const handleClickOpen = () => {
        setOpen(true);
        console.log(rows, 'Single Leave Details')
    };

    const handleClose = () => {
        setOpen(false);
    };

    const DownloadButton = ({ documentID, index }) => {
        const handleDownloadClick = () => {
            const downloadUrl = `/api/leave/DownloadDocument/${documentID}`;

            const link = document.createElement('a');
            link.href = downloadUrl;
            link.style.display = 'none';

            document.body.appendChild(link);

            link.click();

            document.body.removeChild(link);
        };

        return (
            <Chip
                icon={<DownloadIcon />}
                label={`Document ${index + 1}`}
                variant="outlined"
                onClick={handleDownloadClick}
            />
        );
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
                <DialogTitle sx={{ width:'350px' }}>Leave Details</DialogTitle>
                <Divider />
                <DialogContent>
                    {rows.attachedFileIDs != null &&
                        rows.attachedFileIDs.length > 0 &&
                        (
                            <div>
                                <Typography variant="subtitle1" style={{ marginTop: '5px' }}>Attached Documents:</Typography>
                                <div style={{ display: 'grid', gridTemplateColumns: "repeat(3,1fr)", gap: '3px', margin: '5px' }}>
                                    {
                                        rows.attachedFileIDs.map((el, index) => (
                                            <DownloadButton documentID={el} index={index} />
                                        ))
                                    }
                                </div>
                                <Divider />
                            </div>
                        )}
                    <Typography variant="subtitle1" style={{ marginTop: '5px' }}>Leave Dates:</Typography>
                    <ol
                        style={{
                            display: 'grid',
                            gridTemplateColumns: 'repeat(2,1fr)',
                            gap: '5px'
                        }}
                    >
                        {
                            rows.leaveDates.map((el, index) => (
                                <li
                                    key={index}>
                                    <Chip
                                        label={dayjs(el).format('YYYY-MM-DD')}
                                        variant="outlined"
                                    //onDelete={() => handleDeleteDate(index)}
                                    />
                                </li>
                            ))
                        }
                    </ol>
                    <Divider />
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