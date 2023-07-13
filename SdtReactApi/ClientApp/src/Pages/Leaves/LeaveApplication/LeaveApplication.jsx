import React from 'react'
import './LeaveApplication.css'
import { Button, Divider, FormControl, InputLabel, MenuItem, Select, TextField } from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { DemoItem } from '@mui/x-date-pickers/internals/demo';

const LeaveApplication = () => {
  return (
    <div className='leave-application-page'>
      <div className='leave-application-header'> 
        <h1>Leave Application</h1>
        <Divider/>
      </div>
      <form className='leave-application'>
        <div className='date-pickers'>
            <DemoItem label="Leave From">
                <DatePicker
                    label={'Leave From'}
                    format="DD/MM/YYYY"
                    onChange={(e)=>console.log(`${e.$D}-${e.$M}-${e.$y}`)}
                />
            </DemoItem>
            <DemoItem label="To">
                <DatePicker
                    label={'Leave To'}
                    format="DD/MM/YYYY"
                />
            </DemoItem>
        </div>
        
        <FormControl className='leave-type'>
          <InputLabel id="demo-select-small-label">Leave Type</InputLabel>
            <Select size='small' label='Leave Type' required>
              <MenuItem value="medicalleave">Medical Leave</MenuItem>
            </Select>
        </FormControl>
        
        <TextField label='Reason/Comments' required type='text' sx={{height:'50px'}}/>

        <Button
          type='submit'
          variant="outlined" 
        >
          Submit
        </Button>
      </form>
    </div>
  )
}

export default LeaveApplication
