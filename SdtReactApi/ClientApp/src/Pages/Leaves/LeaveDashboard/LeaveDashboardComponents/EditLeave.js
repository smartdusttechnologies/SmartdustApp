import React, { useContext, useState, useEffect } from 'react'
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
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { MobileDatePicker, StaticDatePicker, DatePicker } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { DateField } from '@mui/x-date-pickers/DateField';
import dayjs from 'dayjs';
import AuthContext from '../../../../context/AuthProvider';
import Chip from '@mui/material/Chip';
import DownloadIcon from '@mui/icons-material/Download';

const isWeekend = (date) => {
    const day = date.day();

    return day === 0 || day === 6;
};

export default function EditLeave({ rowData, handleGetLeaves, leavetypes }) {
    const [open, setOpen] = React.useState(false);
    const [isLoading, setLoading] = useState(false);
    const { auth, setNotification, notification } = useContext(AuthContext);
    const [updatedLeaveDates, setUpdatedLeaveDates] = useState(rowData?.leaveDates);
    const [updatedAttachedFileIDs, setUpdatedAttachedFileIDs] = useState(rowData?.attachedFileIDs);
    const [updatedLeaveType, setUpdatedLeaveType] = useState(rowData.leaveTypeID);
    const [updatedReason, setUpdatedReason] = useState(rowData.reason);
    const [file, setFile] = useState([]);

    const handleClickOpen = () => {
        setOpen(true);
        console.log(rowData,'editable data')
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleFileChange = (e) => {
        const filesArray = Array.from(e.target.files);
        setFile(filesArray);
        console.log(filesArray);
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
        setLoading(true)
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
                        setLoading(false)
                    }
                } else {
                    toast.warn('Wrong File Type!', { position: "bottom-center" });
                    setLoading(false)
                }
            });

            if (formData.has('files')) {
                // Upload files and get AttachedFileIDs
                axios.post('api/leave/FileUpload', formData)
                    .then(response => {
                        console.log(response.data);
                        // Call ApplyLeave with AttachedFileIDs
                        UpdateLeave([...updatedAttachedFileIDs, ...response.data ] );
                    })
                    .catch(error => {
                        console.error(error);
                    });
            }
        } else {
            // Call ApplyLeave without AttachedFileIDs
            UpdateLeave(updatedAttachedFileIDs);
        }

    };

    const UpdateLeave = (attachedFileIDs) => {
        axios.post('api/leave/UpdateLeave', {
            id: rowData.id,
            userId: auth?.userId,
            userName: auth?.userName,
            leaveType: '',
            leaveTypeId: updatedLeaveType,
            reason: updatedReason,
            appliedDate: new Date(),
            leaveStatus: rowData.leaveStatus,
            leaveStatusId: 6,
            leaveDays: updatedLeaveDates?.length,
            leaveDates: updatedLeaveDates,
            attachedFileIDs: attachedFileIDs
        })
            .then(response => {
                console.log(response?.data?.message[0]?.reason)
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center"});
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
                handleGetLeaves()
                setLoading(false)
            })
            .catch(error => {
                setLoading(false)
                console.log(error)
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
            })

    }
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
                                        //value={dayjs(leaveData.leaveDates[0])}
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
                    loading={isLoading}
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