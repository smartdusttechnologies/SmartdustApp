import React, { useContext, useState, useEffect } from 'react'
import './LeaveReport.css'
import axios from 'axios'
import { Divider, InputLabel, TextField, Box } from '@mui/material'
import { DemoItem } from '@mui/x-date-pickers/internals/demo';
import Button from '@mui/joy/Button';
import { ToastContainer, toast } from 'react-toastify';
import dayjs from 'dayjs';
import Chip from '@mui/material/Chip';
import { MobileDatePicker, StaticDatePicker, DatePicker } from '@mui/x-date-pickers';
import AuthContext from '../../../context/AuthProvider'
import LoadingProgress from '../../../components/LoadingProgress/LoadingProgress';
import EmployeeLeaveTable from './LeaveReportComponents/EmployeeLeaveTable';

const LeaveReport = () => {
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
        console.log('filtering')
    };

    const handleGetEmployeeLeave = () => {
        setLoading(true)
        axios.get(`api/leave/GetEmployeeLeave/${auth.userId}`, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                console.log(response?.data?.requestedObject, 'Employee Leaves')
                setEmployeeRows(response?.data?.requestedObject)
                setLoading(false)
            })
            .catch(error => {
                console.log(error)
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
                console.log(response?.data?.requestedObject, 'GetManagerLeaveStatusActions')
                setActionsRows(response?.data?.requestedObject)
            })
            .catch(error => {
                console.log(error)
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
                console.log(response?.data?.message[0]?.reason)
                handleGetEmployeeLeave()
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
            })
            .catch(error => {
                console.log(error)
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
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
