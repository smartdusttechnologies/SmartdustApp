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
import AuthContext from '../../../../context/AuthProvider';

export default function SingleLeaveDetail({ rows }) {
    const [open, setOpen] = React.useState(false);
    const { auth } = useContext(AuthContext);

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const DownloadButton = ({ documentID, index }) => {
        const handleDownloadClick = () => {
            const downloadUrl = `/api/document/DownloadDocument/${documentID}`;

            // Get the accessToken from wherever it's stored in your component
            const accessToken = auth.accessToken; // Replace with the actual method to retrieve the access token

            const headers = new Headers({
                'Authorization': `${accessToken}`,
            });

            // Create a request with the headers
            const request = new Request(downloadUrl, {
                method: 'GET',
                headers: headers,
            });

            // Fetch the document with the access token in the headers
            fetch(request)
                .then(response => {
                    if (response.status === 200) {
                        // Create a blob from the response data
                        return response.blob();
                    } else {
                        // Handle other response statuses (e.g., error handling)
                        throw new Error('Failed to download document');
                    }
                })
                .then(blob => {
                    // Create a temporary link element to trigger the download
                    const url = window.URL.createObjectURL(blob);
                    const a = document.createElement('a');
                    a.style.display = 'none';
                    a.href = url;
                    a.download = `Document_${index + 1}.png`; // Set the desired file name here
                    document.body.appendChild(a);

                    // Trigger a click event to initiate the download
                    a.click();

                    // Clean up
                    window.URL.revokeObjectURL(url);
                    document.body.removeChild(a);
                })
                .catch(error => {
                    // Handle any errors
                });
        };

        return (
            <Chip
                icon={<DownloadIcon />}
                label={`Document ${index + 1}`}
                variant="outlined"
                onClick={handleDownloadClick}
                key={index}
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
                        <Typography>Manager Comment : {rows.comment}</Typography>
                    </div>
                </DialogContent>
            </Dialog>
        </div>
    );
}