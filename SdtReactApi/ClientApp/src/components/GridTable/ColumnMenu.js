import * as React from 'react';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import Divider from '@mui/material/Divider';
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import Box from '@mui/material/Box';
import { TextField } from '@mui/material'

const ColumnMenu = ({ Id, label, handleSearchChange, searchTerms, createSortHandler }) => {
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
                <Tooltip title="Menu">
                    <IconButton
                        onClick={handleClick}
                        size="small"
                        aria-controls={open ? 'account-menu' : undefined}
                        aria-haspopup="true"
                        aria-expanded={open ? 'true' : undefined}
                        sx={{
                            opacity: 0,
                            '&:hover': {
                                opacity: 0.9,
                            },
                        }}
                    >
                        <MoreVertIcon sx={{ width: 25, height: 25 }}></MoreVertIcon>
                    </IconButton>
                </Tooltip>
            </Box>
            <Menu
                anchorEl={anchorEl}
                id="account-menu"
                open={open}
                onClose={handleClose}
                //onClick={handleClose}
                PaperProps={{
                    elevation: 0,
                    sx: {
                        overflow: 'visible',
                        filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.12))',
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
                            zIndex: 0,
                        },
                    },
                }}
                transformOrigin={{ horizontal: 'right', vertical: 'top' }}
                anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
            >
                <MenuItem sx={{ fontSize: "20px", fontWeight: "500", display: 'block' }} onClick={handleClose}>Column Options</MenuItem>
                <Divider />
                <MenuItem
                    onClick={createSortHandler(Id)}
                    sx={{ fontSize: "18px" }}
                >
                    <ArrowUpwardIcon sx={{ width: 18, height: 18 , mb:0.4 }} /> Sort
                </MenuItem>
                <Divider />
                <MenuItem >
                    <TextField
                        variant="outlined"
                        size="small"
                        type="text"
                        value={searchTerms[Id] || ''}
                        onChange={(event) => handleSearchChange(event, Id)}
                        placeholder={`Search ${label}`}
                    />
                </MenuItem>
            </Menu>
        </React.Fragment>
    )

};

export default ColumnMenu