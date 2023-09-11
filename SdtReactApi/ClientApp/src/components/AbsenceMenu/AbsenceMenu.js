import * as React from 'react';
import Box from '@mui/material/Box';
import Avatar from '@mui/material/Avatar';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import ListItemIcon from '@mui/material/ListItemIcon';
import Divider from '@mui/material/Divider';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Tooltip from '@mui/material/Tooltip';
import PersonAdd from '@mui/icons-material/PersonAdd';
import Settings from '@mui/icons-material/Settings';
import Logout from '@mui/icons-material/Logout';
import { Link } from 'react-router-dom';
import AuthContext from '../../context/AuthProvider';
import Button from '@mui/material/Button';

export default function AbsenceMenu() {
    const { setAuth, auth } = React.useContext(AuthContext);

    const [anchorEl, setAnchorEl] = React.useState(null);
    const open = Boolean(anchorEl);
    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };
    return (
        <React.Fragment>
            <Box sx={{ display: 'flex', alignItems: 'center', textAlign: 'center' }}>
                <Tooltip title="Leave/Absence">
                    <Typography
                        onClick={handleClick}
                        size="small"
                        aria-controls={open ? 'account-menu' : undefined}
                        aria-haspopup="true"
                        aria-expanded={open ? 'true' : undefined}
                        sx={{ cursor: "pointer", fontWeight:"600"}}
                    >
                        Absence
                    </Typography>
                </Tooltip>
            </Box>
            <Menu
                anchorEl={anchorEl}
                id="account-menu"
                open={open}
                onClose={handleClose}
                onClick={handleClose}
                PaperProps={{
                    elevation: 0,
                    sx: {
                        overflow: 'visible',
                        filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.32))',
                        mt: 1.5,
                        '& .MuiAvatar-root': {
                            width: 32,
                            height: 32,
                            ml: -0.5,
                            mr: 1,
                        },
                        '&:before': {
                            content: '""',
                            display: 'block',
                            position: 'absolute',
                            top: 0,
                            right: 14,
                            width: 10,
                            height: 10,
                            bgcolor: 'background.paper',
                            transform: 'translateY(-50%) rotate(45deg)',
                            zIndex: 0,
                        },
                    },
                }}
                transformOrigin={{ horizontal: 'right', vertical: 'top' }}
                anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
            >
                {
                    auth.isAuthenticated &&
                    (<Link to={'/leavedashboard'} style={{ textDecoration: "none", color: "grey" }}>
                        <MenuItem >
                            Leave Dashboard
                        </MenuItem>
                    </Link>)
                }
                {
                    auth.isAuthenticated && (<Link to={'/leaveapplication'} style={{ textDecoration: "none", color: "grey" }}>
                        <MenuItem >
                            Apply Leave
                        </MenuItem>
                    </Link>)
                }
                {
                    auth.isAuthenticated && auth.roleId == 4 &&
                    (<Link to={'/leavereport'} style={{ textDecoration: "none", color: "grey" }}>
                        <MenuItem >
                            Leave Report
                        </MenuItem>
                    </Link>)
                }
                {
                    auth.isAuthenticated && auth.roleId == 4 &&
                    (<Link to={'/leavebalance'} style={{ textDecoration: "none", color: "grey" }}>
                        <MenuItem >
                            Leave Balance
                        </MenuItem>
                    </Link>)
                }
                <Divider />
            </Menu>
        </React.Fragment>
    );
}