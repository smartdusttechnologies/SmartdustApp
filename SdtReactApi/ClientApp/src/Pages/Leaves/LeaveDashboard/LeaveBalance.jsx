import * as React from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogContent from '@mui/material/DialogContent';
import { Table, TableBody, TableCell, TableHead, TableRow } from '@mui/material';

export default function LeaveBalanceMenu() {
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
       sx={{color:'black' , border:'1px solid rgb(128, 127, 127)'}}
      >
        Leave Balance
      </Button>
      <Dialog
        open={open}
        onClose={handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogContent>
        <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell>Leave Type</TableCell>
            <TableCell>Used</TableCell>
            <TableCell>Available</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          <TableRow>
            <TableCell>Medical Leave</TableCell>
            <TableCell>5</TableCell>
            <TableCell>7</TableCell>
          </TableRow>
          <TableRow>
            <TableCell>Paid Leave</TableCell>
            <TableCell>6</TableCell>
            <TableCell>6</TableCell>
          </TableRow>
        </TableBody>
      </Table>
        </DialogContent>
      </Dialog>
    </div>
  );
}