import React from 'react'
import './LeaveDashboard.css'
import LeavesDataTable from './LeavesTable'
import LeaveBalanceMenu from './LeaveBalance'
import { Button } from '@mui/material'
import { useNavigate } from 'react-router-dom'

const LeaveDashboard = () => {
  const navigate = useNavigate()

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
        <div><LeavesDataTable /></div>
        
    </div>
  )
}

export default LeaveDashboard
