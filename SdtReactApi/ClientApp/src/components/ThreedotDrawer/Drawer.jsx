import * as React from 'react';
import './Drawer.css'
import Box from '@mui/material/Box';
import SwipeableDrawer from '@mui/material/SwipeableDrawer';
import Button from '@mui/material/Button';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { Link } from 'react-router-dom';
import AuthContext from '../../context/AuthProvider';
import UserAccountMenu from '../UserProfile/UserAccountMenu';
import NotificationBellMenu from '../NotificationBell/NotificationBellMenu';

export default function Drawer() {
  const {auth} = React.useContext(AuthContext);

  const [state, setState] = React.useState({
    top: false,
    left: false,
    bottom: false,
    right: false,
  });

  const toggleDrawer = (anchor, open) => (event) => {
    if (
      event &&
      event.type === 'keydown' &&
      (event.key === 'Tab' || event.key === 'Shift')
    ) {
      return;
    }

    setState({ ...state, [anchor]: open });
  };

  const list = (anchor) => (
    <Box
      sx={{ width: anchor === 'top' || anchor === 'bottom' ? 'auto' : 250 , backgroundColor:'black' }}
      role="presentation"
      // onClick={toggleDrawer(anchor, false)}
      onKeyDown={toggleDrawer(anchor, false)}
    >
      <List className='drawer-menu'>
        <Link to={'/'}><ListItem>Home</ListItem></Link>
        {!auth.isAuthenticated && (<Link to={'/login'}><ListItem>Sign in</ListItem></Link>)}
        <Link to={'/contact'}><ListItem>Contact</ListItem></Link>
        <Link to={'/about'}><ListItem>About</ListItem></Link>
        <NotificationBellMenu/>
        <UserAccountMenu/>
        
      </List>
    </Box>
  );

  return (
    <div className='Three-dot'>
      {['top'].map((anchor) => (
        <React.Fragment key={anchor}>
          <Button onClick={toggleDrawer(anchor, true)}><MoreVertIcon fontSize='large'/></Button>
          <SwipeableDrawer
            anchor={anchor}
            open={state[anchor]}
            onClose={toggleDrawer(anchor, false)}
            onOpen={toggleDrawer(anchor, true)}
          >
            {list(anchor)}
          </SwipeableDrawer>
        </React.Fragment>
      ))}
    </div>
  );
}
