import React from 'react'
import './BottomNav.css'
import { BottomNavigationAction } from '@mui/material';
import Box from '@mui/material/Box';
import BottomNavigation from '@mui/material/BottomNavigation';
import FacebookIcon from '@mui/icons-material/Facebook';
import EmailIcon from '@mui/icons-material/Email';
import LocalPhoneIcon from '@mui/icons-material/LocalPhone';
import ChatBubbleIcon from '@mui/icons-material/ChatBubble';
import Paper from '@mui/material/Paper';

const BottomNav = () => {
  const [value, setValue] = React.useState(0);

  const handlePhoneClick = () => {
    window.location.href = `tel:7857068847`;
  };

  const handleEmailClick = () => {
    window.location.href = `mailto:your-email@example.com`;
  };

  const handleFavoritesClick = () => {
    window.location.href = 'https://www.facebook.com/people/Smartdust-Technologies/100071813210648/';
  };

  return (
    <div className='BottomNavigation-container'>
    <Box sx={{ width: 500 }}>
      <Paper sx={{ position: 'fixed', bottom: 0, left: 0, right: 0 }} elevation={3}>

        <BottomNavigation
          showLabels
          // value={value}
          // onChange={(event, newValue) => {
          //   setValue(newValue);
          // }}
          >
          <BottomNavigationAction label="Phone" icon={<LocalPhoneIcon sx={{color:'rgb(39, 197, 60)'}} />} onClick={handlePhoneClick} />
          <BottomNavigationAction label="Email" icon={<EmailIcon sx={{color:'rgb(161, 34, 34)'}} />} onClick={handleEmailClick} />
          <BottomNavigationAction label="Facebook" icon={<FacebookIcon sx={{color:'rgb(11, 83, 207)'}} />} onClick={handleFavoritesClick}/>
          {/* <BottomNavigationAction label="Chat" icon={<ChatBubbleIcon sx={{color:'rgb(58, 88, 237)'}} />} /> */}
        </BottomNavigation>
      </Paper>
    </Box>
    </div>
  )
}

export default BottomNav
