import React, { useContext } from 'react'
import './Navbar.css'
import { Link } from 'react-router-dom'
import Drawer from '../ThreedotDrawer/Drawer'
import UserAccountMenu from '../UserProfile/UserAccountMenu'
import AbsenceMenu from '../AbsenceMenu/AbsenceMenu'
import AuthContext from '../../context/AuthProvider'
import NotificationBellMenu from '../NotificationBell/NotificationBellMenu'
import logo from '../../assets/images/Smartdust_logo.webp'

const Navbar = () => {
  const {auth} = useContext(AuthContext);

  return (
    <div className='Navbar-body'>
      <div className='Navbar'>
        <div className='left'>
          <Link to={'/'}>
          <div>
            <img src={logo} alt="" />
          </div>
          </Link>
        </div>
        <div className='right none'>
                  <Link to={'/'}><span><a>Home</a> </span></Link> 
                  {!auth.isAuthenticated && (<Link to={'/login'}><span>Sign in </span></Link>)} 
                  <Link to={'/contact'}><span><a>Contact</a> </span></Link> 
                  <Link to={'/about'}><span><a>About</a> </span></Link>
        {auth.isAuthenticated && <AbsenceMenu/>}
        <NotificationBellMenu/>
        <UserAccountMenu/>
        </div>
      </div>
          <Drawer/>
    </div>
  )
}

export default Navbar
