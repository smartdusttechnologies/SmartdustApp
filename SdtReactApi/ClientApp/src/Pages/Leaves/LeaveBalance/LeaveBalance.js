import React, { useContext, useState, useEffect } from 'react'
import axios from 'axios'
import './LeaveBalance.css'
import { useNavigate } from 'react-router-dom'
import { Box} from '@mui/material'
import { ToastContainer, toast } from 'react-toastify';
import AuthContext from '../../../context/AuthProvider'
import LoadingProgress from '../../../components/LoadingProgress/LoadingProgress';
import CreateLeaveBalance from './LeaveBalanceComponents/CreateLeaveBalance';
import LeavesBalanceTable from './LeaveBalanceComponents/LeaveBalanceTable';

const LeaveBalancePage = () => {
    const navigate = useNavigate();

    const [isLoading, setLoading] = useState(false);
    const [leavetypes, setLeaveTypes] = useState([]);
    const [employeeDetails, setEmployeeDetails] = useState([]);
    const [employeeLeaveBalance, setEmployeeLeaveBalance] = useState([]);
    const { auth, setNotification, notification } = useContext(AuthContext);

    const handleCreate = (formData) => {
        axios.post('api/leave/CreateLeaveBalance', {
            id: 0,
            userId: formData.user,
            leavetype: formData.leaveType,
            available: formData.balance,
            userName: ''
        }, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                handleGetEmployeeLeaveBalance()
                const messages = response?.data?.message;

                if (messages && messages.length > 0) {
                    const newNotifications = [];
                    for (let i = 0; i < messages.length; i++) {
                        if (i < 3) {
                            toast.success(messages[i].reason, { position: "bottom-center", theme: "dark" });
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
                            toast.error(messages[i].reason, { position: "bottom-center", theme: "dark" });
                        }
                        newNotifications.push({ message: messages[i].reason, success: false });
                    }
                    setNotification([...notification, ...newNotifications]);
                }
            })
    }
    const handleUpdate = (formData) => {

        axios.post('api/leave/UpdateLeaveBalance', {
            id: formData.Id,
            userId: formData.user,
            leavetype: formData.leaveType,
            available: formData.balance,
            userName: ''
        }, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                handleGetEmployeeLeaveBalance()
                const messages = response?.data?.message;

                if (messages && messages.length > 0) {
                    const newNotifications = [];
                    for (let i = 0; i < messages.length; i++) {
                        if (i < 3) {
                            toast.success(messages[i].reason, { position: "bottom-center", theme: "dark" });
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
                            toast.error(messages[i].reason, { position: "bottom-center", theme: "dark" });
                        }
                        newNotifications.push({ message: messages[i].reason, success: false });
                    }
                    setNotification([...notification, ...newNotifications]);
                }
            })
    }
    const handleDelete = (id) => {
        axios.post(`api/leave/DeleteLeaveBalance`, {
            id: id,
            userId: 0,
            leavetype: '',
            available: 0,
            userName: ''
        }, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                handleGetEmployeeLeaveBalance()
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
            })
            .catch(error => {
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
            })
    }
    const handleGetLeaveTypes = () => {
        axios.get('api/leave/GetLeaveTypes', {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                setLeaveTypes(response?.data?.requestedObject)
            })
            .catch(error => {
            })
    }
    const handleGetEmployeeDetails = () => {
        axios.get(`api/leave/GetEmployeeDetails/${auth.userId}`, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                setEmployeeDetails(response?.data?.requestedObject)
            })
            .catch(error => {
            })
    }
    const handleGetEmployeeLeaveBalance = () => {
        setLoading(true)
        axios.get(`api/leave/GetEmployeeLeaveBalance/${auth.userId}`, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                setEmployeeLeaveBalance(response?.data?.requestedObject)
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

    useEffect(() => {
        handleGetLeaveTypes()
        handleGetEmployeeDetails()
        handleGetEmployeeLeaveBalance()
    }, [])
    return (
        <div className='leave-balance-page'>
            <div className='heading'>
                <h1>Leave Balance</h1>
            </div>
            <div style={{
                display: 'flex',
                justifyContent: 'right',
                width: '90%',
                margin: '5px auto',
            } }>
                <CreateLeaveBalance
                    leavetypes={leavetypes}
                    handleCreate={handleCreate}
                    employeeDetails={employeeDetails}
                />
            </div>
            <div>
                {
                    isLoading ? <Box><LoadingProgress /></Box> : <LeavesBalanceTable rows={employeeLeaveBalance} leavetypes={leavetypes} employeeDetails={employeeDetails} handleUpdate={handleUpdate} handleDelete={handleDelete } />
                }
            </div>
            <ToastContainer />
        </div>
    )
}

export default LeaveBalancePage
