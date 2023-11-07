import React, { useContext, useState, useEffect } from 'react'
import axios from 'axios'
import './EmployeeDashboard.css'
import { useNavigate } from 'react-router-dom'
import { Box} from '@mui/material'
import { ToastContainer, toast } from 'react-toastify';
import AuthContext from '../../../context/AuthProvider'
import LoadingProgress from '../../../components/LoadingProgress/LoadingProgress';
import EmployeeManagerTable from './EmployeeDashboardcomponents/EmployeeManagerTable';
import Create from './EmployeeDashboardcomponents/Create';

const EmployeeDashboardPage = () => {
    const navigate = useNavigate();

    const [isLoading, setLoading] = useState(false);
    const [users, setUsers] = useState([]);
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
                handleGetManagerAndEmployeeData()
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
            })
            .catch(error => {
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
                handleGetManagerAndEmployeeData()
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
            })
            .catch(error => {
                toast.error(error?.response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: error?.response?.data?.message[0]?.reason, success: false }])
            })
    }
    const handleDelete = (id) => {
        axios.post(`api/leave/DeleteManagerAndEmployeeData`, {
            id: id,
            managerId: 1,
            managerName: '',
            employeeId: 1,
            employeeName: ''
        }, {
            headers: {
                'Authorization': `${auth.accessToken}`
            }
        })
            .then(response => {
                handleGetManagerAndEmployeeData()
                toast.success(response?.data?.message[0]?.reason, { position: "bottom-center", theme: "dark" });
                setNotification([...notification, { message: response?.data?.message[0]?.reason, success: true }])
            })
            .catch(error => {
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
                setUsers(response?.data?.requestedObject)
            })
            .catch(error => {
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
                setManagerAndEmployeeData(response?.data?.requestedObject)
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
