import React, { useContext, useState, useEffect } from 'react'
import './LeaveReport.css'
import axios from 'axios'
import { useNavigate } from 'react-router-dom'
import { Box } from '@mui/material'
import { DemoItem } from '@mui/x-date-pickers/internals/demo';
import { ToastContainer, toast } from 'react-toastify';
import { DatePicker } from '@mui/x-date-pickers';
import AuthContext from '../../../context/AuthProvider'
import LoadingProgress from '../../../components/LoadingProgress/LoadingProgress';
import EmployeeLeaveTable from './LeaveReportComponents/EmployeeLeaveTable';

const LeaveReport = () => {
    const navigate = useNavigate();

    const [isLoading, setLoading] = useState(false);
    const [employeeRows, setEmployeeRows] = useState([]);
    const [actionRows, setActionsRows] = useState([]);
    const [selectedStartDate, setSelectedStartDate] = useState(null);
    const [selectedEndDate, setSelectedEndDate] = useState(null);
    const { auth, setNotification, notification } = useContext(AuthContext);

    const filterEmployeeRows = () => {
        const filteredRows = employeeRows.filter(employee => {
            return employee.leaveDates.some(leaveDate => {
                const date = new Date(leaveDate);
                return (
                    (!selectedStartDate || date >= selectedStartDate) &&
                    (!selectedEndDate || date <= selectedEndDate)
                );
            });
        });

        setEmployeeRows(filteredRows);
    };

    const handleGetEmployeeLeave = () => {
        setLoading(true)
        axios.get(`api/leave/GetEmployeeLeave/${auth.userId}`, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                setEmployeeRows(response?.data?.requestedObject)
                setLoading(false)
            })
            .catch(error => {
                if (error.response && error.response.status === 401) {
                    // Handle 401 Unauthorized error
                    navigate('/unauthorizedpage')
                }
                setLoading(false)
            })
    }
    const handleGetManagerLeaveStatusActions = () => {
        axios.get('api/leave/GetManagerLeaveStatusActions', {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                setActionsRows(response?.data?.requestedObject)
            })
            .catch(error => {
            })
    }

    const handleUpdatestatus = (LeaveID, StatusID, comment) => {
        axios.post('api/leave/UpdateLeaveStatus', {
            leaveID: LeaveID,
            statusID: StatusID,
            comment: comment
        }, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                handleGetEmployeeLeave()
                const messages = response?.data?.message;

                if (messages && messages.length > 0) {
                    const newNotifications = [];
                    for (let i = 0; i < messages.length; i++) {
                        if (i < 3) {
                            toast.success(messages[i].reason, { position: "bottom-center", theme: "colored" });
                        }
                        newNotifications.push({ message: messages[i].reason, success: true });
                    }
                    setNotification([...notification, ...newNotifications]);
                }
            })
            .catch(error => {
                const messages = error?.response?.data?.message;

                if (messages && messages.length > 0) {
                    const newNotifications = [];
                    for (let i = 0; i < messages.length; i++) {
                        if (i < 3) {
                            toast.error(messages[i].reason, { position: "bottom-center", theme: "colored" });
                        }
                        newNotifications.push({ message: messages[i].reason, success: false });
                    }
                    setNotification([...notification, ...newNotifications]);
                }
            })
    };

    useEffect(() => {
        handleGetEmployeeLeave()
        handleGetManagerLeaveStatusActions()
    }, [])

    useEffect(() => {
        if (selectedStartDate !== null && selectedEndDate !== null) {
            filterEmployeeRows();
        }
    }, [selectedStartDate, selectedEndDate]);

    return (
        <div className='leave-report-page'>
            <div className='heading'>
                <h1>Leave Report</h1>
            </div>
            <div className='filtering-options'>
                    <h4>
                        DATE RANGE
                    </h4>
                <div className='range-pickers'>
                    <DemoItem label="From" className='date-range-pickers'>
                        <DatePicker
                            label={'Select Dates'}
                            format="YYYY/MM/DD"
                            closeOnSelect={false}
                            value={selectedStartDate}
                            onChange={(e) => setSelectedStartDate(new Date(e?.$d))}
                        />
                    </DemoItem>
                    <DemoItem label="To" className='date-range-pickers'>
                        <DatePicker
                            label={'Select Dates'}
                            format="YYYY/MM/DD"
                            closeOnSelect={false}
                            value={selectedEndDate}
                            onChange={(e) => setSelectedEndDate(new Date(e?.$d))}
                        />
                    </DemoItem>
                </div>
            </div>
            <div>
                {
                    isLoading ? <Box><LoadingProgress /></Box> : <EmployeeLeaveTable rows={[...employeeRows].reverse()} actionRows={actionRows} handleUpdatestatus={handleUpdatestatus} />
                }
            </div>
            <ToastContainer />
        </div>
    )
}

export default LeaveReport
