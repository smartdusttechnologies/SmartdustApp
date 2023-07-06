import React, { useContext } from 'react'
import './Navbar.css'
import { Link } from 'react-router-dom'
import Drawer from '../ThreedotDrawer/Drawer'
import UserAccountMenu from '../UserProfile/UserAccountMenu'
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
        <Link to={'/'}><div><a>Home</a> </div></Link> 
        {!auth.isAuthenticated && (<Link to={'/login'}><div><a>Sign in</a> </div></Link>)} 
        <Link to={'/contact'}><div><a>Contact</a> </div></Link> 
        <Link to={'/about'}><div><a>About</a> </div></Link>
        <NotificationBellMenu/>
        <UserAccountMenu/>
        </div>
      </div>
          <Drawer/>
    </div>
  )
}

export default Navbar
