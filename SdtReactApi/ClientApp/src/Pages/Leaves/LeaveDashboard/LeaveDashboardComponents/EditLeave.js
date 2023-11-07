import React, { useContext, useState } from 'react'
import axios from 'axios'
import { ToastContainer, toast } from 'react-toastify';
import Button from '@mui/joy/Button';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import { DemoItem } from '@mui/x-date-pickers/internals/demo';
import Divider from '@mui/material/Divider';
import DialogTitle from '@mui/material/DialogTitle';
import { TextField, FormControl, InputLabel, MenuItem, Select } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { MobileDatePicker, StaticDatePicker, DatePicker } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs from 'dayjs';
import AuthContext from '../../../../context/AuthProvider';
import Chip from '@mui/material/Chip';
import DownloadIcon from '@mui/icons-material/Download';

const isWeekend = (date) => {
    const day = date.day();

    return day === 0 || day === 6;
};

export default function EditLeave({ rowData, leavetypes, UpdateLeave }) {
    const [open, setOpen] = React.useState(false);
    const { auth} = useContext(AuthContext);
    const [updatedLeaveDates, setUpdatedLeaveDates] = useState(rowData?.leaveDates);
    const [updatedAttachedFileIDs, setUpdatedAttachedFileIDs] = useState(rowData?.attachedFileIDs);
    const [updatedLeaveType, setUpdatedLeaveType] = useState(rowData.leaveTypeID);
    const [updatedReason, setUpdatedReason] = useState(rowData.reason);
    const [file, setFile] = useState([]);

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleFileChange = (e) => {
        const filesArray = Array.from(e.target.files);
        setFile(filesArray);
    };
    const handleDeleteFile = (indexToDelete) => {
        setFile(file?.filter((file, index) => index !== indexToDelete));
    };

    const handleLeaveDates = (e) => {
        const selectedDate = new Date(e?.$d);

        // Check if the selected date already exists in the leaveDates array
        const dateExists = updatedLeaveDates.some((date) =>
            isSameDay(new Date(date), selectedDate)
        );

        // Apply IST timezone offset to the selected date
        selectedDate.setHours(selectedDate.getHours() + 5); // Add 5 hours for UTC+5
        selectedDate.setMinutes(selectedDate.getMinutes() + 30); 

        // If the date doesn't exist, add it to the leaveDates array
        if (!dateExists) {
            setUpdatedLeaveDates([...updatedLeaveDates, selectedDate]);
        }
    };

    const handleDeleteDate = (indexToDelete) => {
        setUpdatedLeaveDates(updatedLeaveDates?.filter((date, index) => index !== indexToDelete));
    };

    // Utility function to check if two dates have the same day, month, and year (ignoring time)
    const isSameDay = (date1, date2) =>
        date1.getDate() === date2.getDate() &&
        date1.getMonth() === date2.getMonth() &&
        date1.getFullYear() === date2.getFullYear();


    const handleDeleteFileID = (indexToDelete) => {
        setUpdatedAttachedFileIDs(updatedAttachedFileIDs?.filter((date, index) => index !== indexToDelete));
    };

    const handleUpdate = () => {
        const formData = new FormData();

        // Check if there are files to upload
        if (file && file.length > 0) {
            file.forEach(file => {
                const fileName = file.name;
                const fileExtension = fileName.split('.').pop().toLowerCase();
                // Check for allowed file extensions
                if (['jpg', 'jpeg', 'png', 'xlsx', 'pdf'].includes(fileExtension)) {
                    if (file.size <= 1024 * 1024) { // Check file size
                        formData.append('files', file);
                    } else {
                        toast.warn('File size should not exceed 1MB.', { position: "bottom-center" });
                    }
                } else {
                    toast.warn('Wrong File Type!', { position: "bottom-center" });
                }
            });

            if (formData.has('files')) {
                // Upload files and get AttachedFileIDs
                axios.post('api/document/FileUpload', formData)
                    .then(response => {
                        // Call ApplyLeave with AttachedFileIDs
                        UpdateLeave([...updatedAttachedFileIDs, ...response.data], rowData.id, updatedLeaveType, updatedReason, updatedLeaveDates );
                    })
                    .catch(error => {
                        console.error(error);
                    });
            }
        } else {
            // Call ApplyLeave without AttachedFileIDs
            UpdateLeave(updatedAttachedFileIDs, rowData.id, updatedLeaveType, updatedReason, updatedLeaveDates);
        }

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
                onDelete={() => handleDeleteFileID(index)}
                key={index}
            />
        );
    };

    return (
        <div>
            <Tooltip title="Edit">
                <span>
                    <IconButton
                        disabled={rowData.leaveStatus !== "Pending" && rowData.leaveStatus !== "Deny"}
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
                        minWidth: '400px'
                    }} >
                        <div
                            style={{
                                gap:'10px'
                            }} >
                            <LocalizationProvider dateAdapter={AdapterDayjs}>
                                <DemoItem label="Select Dates">
                                    <MobileDatePicker
                                        label={'Select Dates'}
                                        format="YYYY/MM/DD"
                                        disablePast
                                        shouldDisableDate={isWeekend}
                                        closeOnSelect={false}
                                        onChange={(e) => handleLeaveDates(e)}
                                    />
                                </DemoItem>
                            </LocalizationProvider>
                                <ol
                                    style={{
                                        display: 'grid',
                                        gridTemplateColumns: 'repeat(2,1fr)',
                                        gap: '5px'
                                    }}
                                >
                                    {
                                        updatedLeaveDates.map((el, index) => (
                                            <li
                                                key={index}>
                                                <Chip
                                                    label={dayjs(el).format('YYYY-MM-DD')}
                                                    variant="outlined"
                                                    onDelete={() => handleDeleteDate(index)}
                                                />
                                            </li>
                                        ))
                                    }
                                </ol>
                        </div>
                        <Divider />
                        <div className="documentupload">
                            <label htmlFor="fileupload">Choose files</label>
                            <span>
                                {file.length > 0
                                    ? <div className="fileslist">
                                        {
                                            file?.map((el, index) => (
                                                <div key={index}>
                                                    <Chip
                                                        label={el.name}
                                                        variant="outlined"
                                                        onDelete={() => handleDeleteFile(index)}
                                                    />
                                                </div>
                                            ))
                                        }
                                    </div>
                                    : "No files chosen"}
                            </span>
                            <input
                                type="file"
                                id="fileupload"
                                multiple
                                onChange={(e) => handleFileChange(e)}
                            />
                        </div>
                        {updatedAttachedFileIDs != null &&
                            updatedAttachedFileIDs.length > 0 &&
                            (
                            <div>
                                <div style={{ display: 'grid', gridTemplateColumns: "repeat(3,1fr)", gap: '3px', margin: '5px', marginBottom:'15px' }}>
                                        {
                                        updatedAttachedFileIDs.map((el, index) => (
                                                <DownloadButton documentID={el} index={index} />
                                            ))
                                        }
                                    </div>
                                    <Divider />
                                </div>
                            )
                        }

                        <FormControl className='leave-type'>
                            <InputLabel id="demo-select-small-label">Leave Type</InputLabel>
                            <Select
                                size='small'
                                label='Leave Type'
                                value={updatedLeaveType}
                                onChange={(e) => setUpdatedLeaveType(e.target.value)}
                                required
                            >
                                {
                                    leavetypes.map((el, index) => (
                                        <MenuItem key={index} value={el.id}>{el.name}</MenuItem>
                                    ))
                                }
                            </Select>
                        </FormControl>
                        <TextField
                            label="Reason"
                            multiline
                            value={updatedReason}
                            onChange={(e) => setUpdatedReason(e.target.value)}
                        />
                    </div>
                </DialogContent>
                <Divider />
                <Button
                    onClick={handleUpdate}
                    sx={{
                        width: '90%',
                        margin: 'auto',
                        mb: '20px',
                        mt:'10px'
                    } }                    
                  >
                    Save
                </Button>
            </Dialog>
            <ToastContainer />
        </div>
    );
}