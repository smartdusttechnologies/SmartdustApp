import * as React from 'react';
import './NotificationBellMenu.css'
import Button from '@mui/material/Button';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import NotificationsIcon from '@mui/icons-material/Notifications';
import AuthContext from '../../context/AuthProvider';

export default function NotificationBellMenu() {
  const {setNotification,notification} = React.useContext(AuthContext);
  const [readStatus, setReadStatus] = React.useState(Array(notification.length).fill(false));
  

  const [anchorEl, setAnchorEl] = React.useState(null);
  const open = Boolean(anchorEl);

  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
    setReadStatus(Array(notification.length).fill(true))
  };

  const handleReadStatus = (i)=>{
    const UpdatedReadStatus = [...readStatus]
    console.log(UpdatedReadStatus)
    console.log(i)
    UpdatedReadStatus[i] = !UpdatedReadStatus[i]
    setReadStatus(UpdatedReadStatus)
  };

  const handleViewAll = ()=>{
   setReadStatus(Array(notification.length).fill(true))
  }

  const NotificationCount = ()=>{
    const notificationCount = notification.length - readStatus.filter(el => el == true).length
    if(notificationCount < 0){
      return 0
    }else{
      return notificationCount
    }
  }

  return (
    <div>
      <button
        id="basic-button"
        aria-controls={open ? 'basic-menu' : undefined}
        aria-haspopup="true"
        aria-expanded={open ? 'true' : undefined}
        onClick={handleClick}
        className='NotificationBell-button'
        current-count={NotificationCount()}
      >
        <NotificationsIcon
         sx={{ width: 26, height: 26 ,color:'gray'}}      
        />
      </button>
      <Menu
        id="basic-menu"
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
        MenuListProps={{
          'aria-labelledby': 'basic-button',
        }}
        sx={{textAlign:"center",alignItems:"center"}}
      >
        <MenuItem sx={{fontSize:"22px" , fontWeight:"500" ,display:'block'}} onClick={handleClose} className='Notification-header' >Notification</MenuItem>
        <MenuItem sx={{color:"rgb(28, 120, 239)",pt:0}} onClick={()=>setNotification([]) }>clear</MenuItem>
        <hr />
        {
          notification.length == 0 ?  <MenuItem sx={{display:'block'}}>No Notification</MenuItem> : <span></span>
        }
        {[...notification].reverse().map((el,i)=>(
            <MenuItem 
              onClick={handleClose} 
              key={i} 
              style={{
                fontWeight: !readStatus[notification.length-1-i] ? 'bold' : 500 ,
                backgroundColor: !readStatus[notification.length-1-i] ? "rgb(245, 235, 235)":'white',
                color: el.success ? 'rgb(55, 169, 87)' : 'rgb(249, 64, 64)',
                // marginTop:1,
                // marginBottom:1,
                height:'55px',
                borderBottom:'1px solid rgb(171, 171, 171)'
              }}
            > {el.message} </MenuItem> 
        ))}
        {
          notification.length > 0 ?  <MenuItem 
          sx={{
            fontSize:"18px" , fontWeight:"500" ,display:'block',marginBottom:'-8px',bgcolor:"rgb(55, 169, 87)",color:"rgb(238, 238, 238)",
            '&:hover':{bgcolor:"rgb(55, 159, 87)"}
          }}
          onClick={handleViewAll}
          
         >View All</MenuItem> : <span></span>
        }
        
      </Menu>
    </div>
  );
}