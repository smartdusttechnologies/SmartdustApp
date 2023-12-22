import * as React from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import Divider from '@mui/material/Divider';
import DialogTitle from '@mui/material/DialogTitle';
import { Table, TableBody, TableCell, TableHead, TableRow } from '@mui/material';

const NoDataTableRows = (rows) => {
    if (rows.length === 0) {
        return (
            <TableRow>
                <TableCell colSpan={6} align="center">
                    No data available
                </TableCell>
            </TableRow>
        );
    }
};

export default function LeaveBalanceMenu({rows}) {
    const [open, setOpen] = React.useState(false);

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    return (
        <div>
            <Button variant="outlined" onClick={handleClickOpen}
            >
                Leave Balance
            </Button>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle>Leave Balance</DialogTitle>
                <Divider />
                <DialogContent>
                    <Table size="small">
                        <TableHead>
                            <TableRow>
                                <TableCell>Leave Type</TableCell>
                                <TableCell>Available</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {
                                rows.length === 0 ? NoDataTableRows(rows) : rows.map((row, index) => (
                                    <TableRow key={index}>
                                        <TableCell align="center">{row.leaveType}</TableCell>
                                        <TableCell align="center">{row.available}</TableCell>
                                    </TableRow>
                                ))
                            }
                        </TableBody>
                    </Table>
                </DialogContent>
            </Dialog>
        </div>
    );
}