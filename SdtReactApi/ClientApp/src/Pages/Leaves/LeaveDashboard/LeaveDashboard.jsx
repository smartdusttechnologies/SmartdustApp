import React, { useEffect, useState } from 'react'
import './LeaveDashboard.css'
import LeaveBalanceMenu from './LeaveBalance'
import { Button,Box } from '@mui/material'
import { useNavigate } from 'react-router-dom'
import axios from 'axios'
import LeavesDataTable from './LeavesTable'
import CircularProgress from '@mui/material/CircularProgress';
import LoadingProgress from '../../../components/LoadingProgress/LoadingProgress';

const LeaveDashboard = () => {
    const navigate = useNavigate();
    const [rows, setRows] = useState([]);
    const [isLoading, setLoading] = useState(false);

    const handleGetLeaves = () => {
        setLoading(true)
        axios.get('api/leave/GetLeave')
            .then(response => {
                console.log(response.data.requestedObject)
                setRows(response?.data?.requestedObject)
                setLoading(false)
            })
            .catch(error => {
                console.log(error)
                setLoading(false)
            })
    }

    useEffect(() => {
        handleGetLeaves()
    }, [])

  return (
    <div className='leave-dashboard'>
        <div className='leaveboard-head'>
          <div className='leaveboard-options'>
            <h1>LeaveBoard</h1>
            <div> <LeaveBalanceMenu/> </div>
            <div>
             <Button variant="outlined" 
              sx={{color:'black' , border:'1px solid rgb(128, 127, 127)'}}
              onClick={()=> navigate('/leaveapplication')}
             >
              Apply a Leave
             </Button>
            </div>
          </div>
          </div>
          <div>
              {
                  isLoading ? <Box><LoadingProgress /></Box> : <LeavesDataTable rows={rows} />
              }
          </div>
    </div>
  )
}

export default LeaveDashboard
