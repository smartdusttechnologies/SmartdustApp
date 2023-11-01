import React, { useContext, useState, useEffect } from 'react'
import axios from 'axios'
import './EmployeeDashboard.css'
import { useNavigate } from 'react-router-dom'
import { Divider, InputLabel, TextField, Box, FormControl, MenuItem, Select } from '@mui/material'
import { DemoItem } from '@mui/x-date-pickers/internals/demo';
import Button from '@mui/joy/Button';
import { ToastContainer, toast } from 'react-toastify';
import dayjs from 'dayjs';
import Chip from '@mui/material/Chip';
import AuthContext from '../../../context/AuthProvider'
import LoadingProgress from '../../../components/LoadingProgress/LoadingProgress';
import EmployeeManagerTable from './EmployeeDashboardcomponents/EmployeeManagerTable';
import Create from './EmployeeDashboardcomponents/Create';

const EmployeeDashboardPage = () => {
    const navigate = useNavigate();

    const [isLoading, setLoading] = useState(false);
    const [users, setUsers] = useState([]);
    const [employeeDetails, setEmployeeDetails] = useState([]);
    const [managerAndEmployeeData, setManagerAndEmployeeData] = useState([]);
    const { auth, setNotification, notification } = useContext(AuthContext);

    const handleCreate = (formData) => {

        axios.post('api/leave/CreateManagerAndEmployeeData', {
            id:0,
            managerId: formData.setmanager,
            managerName: '',
            employeeId: formData.setemployee,
            employeeName: ''
        }, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                console.log(response?.data?.message[0]?.reason)
                handleGetManagerAndEmployeeData()
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
            })
            .catch(error => {
                console.log(error)
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
            })
    }
    const handleUpdate = (formData) => {

        axios.post('api/leave/EditManagerAndEmployeeData', {
            id: formData.Id,
            managerId: formData.setmanager,
            managerName: '',
            employeeId: formData.setemployee,
            employeeName: ''
        }, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                console.log(response?.data?.message[0]?.reason)
                handleGetManagerAndEmployeeData()
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
            })
            .catch(error => {
                console.log(error)
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
            })
    }
    const handleDelete = (id) => {
        console.log(id , 'asdfsdf')
        axios.post(`api/leave/DeleteManagerAndEmployeeData/${id}`, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                console.log(response)
                handleGetManagerAndEmployeeData()
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
            })
            .catch(error => {
                console.log(error)
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
            })
    }
    const handleGetUsers = () => {
        axios.get('api/leave/GetUsers', {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                console.log(response?.data?.requestedObject, 'handleGetUsers')
                setUsers(response?.data?.requestedObject)
            })
            .catch(error => {
                console.log(error)
            })
    }
    const handleGetManagerAndEmployeeData = () => {
        setLoading(true)
        axios.get(`api/leave/GetManagerAndEmployeeData`, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                console.log(response?.data?.requestedObject, 'GetManagerAndEmployeeData')
                setManagerAndEmployeeData(response?.data?.requestedObject)
                setLoading(false)
            })
            .catch(error => {
                console.log(error)
                if (error.response && error.response.status === 401) {
                    // Handle 401 Unauthorized error
                    navigate('/unauthorizedpage')
                }
                setLoading(false)
            })
    }

    useEffect(() => {
        handleGetManagerAndEmployeeData()
        handleGetUsers()
    }, [])
    return (
        <div className='Employee-Dashboard-page'>
            <div className='heading'>
                <h1>Employee and Manager Dashboard</h1>
            </div>
            <div style={{
                display: 'flex',
                justifyContent: 'right',
                width: '90%',
                margin: '5px auto',
            }}>
                <Create
                    users={users}
                    handleCreate={handleCreate}
                />
            </div>
            <div>
                {
                    isLoading ? <Box><LoadingProgress /></Box> : <EmployeeManagerTable rows={managerAndEmployeeData} users={users} handleUpdate={handleUpdate} handleDelete={handleDelete} />
                }
            </div>
            <ToastContainer />
        </div>
    )
}

export default EmployeeDashboardPage
