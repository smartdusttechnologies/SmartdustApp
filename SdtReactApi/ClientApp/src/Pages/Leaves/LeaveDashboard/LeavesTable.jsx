import * as React from 'react';
import Box from '@mui/material/Box';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TablePagination from '@mui/material/TablePagination';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';

function createData(id,name, calories, fat, carbs, protein) {
  return {
    id,
    name,
    calories,
    fat,
    carbs,
    protein,
  };
}

const rows = [
  createData(1,'Medical Leave', '30/02/2023' , '03/03/2023', 'Fever', 'Pending'),
  createData(2,'Paid Leave', '30/02/2023' , '03/03/2023', 'Personal Work', 'Approved'),
  createData(3,'Unpaid Leave', '30/02/2023' , '03/03/2023', 'Vcation', 'Pending'),
  createData(4,'Medical Leave', '30/02/2023' , '03/03/2023', 'abc', 'Pending'),
  createData(5,'Paid Leave', '30/02/2023' , '03/03/2023', 'xyz', 'Pending'),
  createData(5,'Paid Leave', '30/02/2023' , '03/03/2023', 'xyz', 'Pending'),
  createData(5,'Paid Leave', '30/02/2023' , '03/03/2023', 'xyz', 'Pending'),
  createData(5,'Paid Leave', '30/02/2023' , '03/03/2023', 'xyz', 'Pending'),
  createData(5,'Paid Leave', '30/02/2023' , '03/03/2023', 'xyz', 'Pending'),
  createData(5,'Paid Leave', '30/02/2023' , '03/03/2023', 'xyz', 'Pending'),
  createData(5,'Paid Leave', '30/02/2023' , '03/03/2023', 'xyz', 'Pending'),
  createData(5,'Paid Leave', '30/02/2023' , '03/03/2023', 'xyz', 'Pending'),
  createData(5,'Paid Leave', '30/02/2023' , '03/03/2023', 'xyz', 'Pending'),
  createData(5,'Paid Leave', '30/02/2023' , '03/03/2023', 'xyz', 'Pending'),
];

function descendingComparator(a, b, orderBy) {
  if (b[orderBy] < a[orderBy]) {
    return -1;
  }
  if (b[orderBy] > a[orderBy]) {
    return 1;
  }
  return 0;
}

function getComparator(order, orderBy) {
  return order === 'desc'
    ? (a, b) => descendingComparator(a, b, orderBy)
    : (a, b) => -descendingComparator(a, b, orderBy);
}

// Since 2020 all major browsers ensure sort stability with Array.prototype.sort().
// stableSort() brings sort stability to non-modern browsers (notably IE11). If you
// only support modern browsers you can replace stableSort(exampleArray, exampleComparator)
// with exampleArray.slice().sort(exampleComparator)
function stableSort(array, comparator) {
  const stabilizedThis = array.map((el, index) => [el, index]);
  stabilizedThis.sort((a, b) => {
    const order = comparator(a[0], b[0]);
    if (order !== 0) {
      return order;
    }
    return a[1] - b[1];
  });
  return stabilizedThis.map((el) => el[0]);
}

export default function LeavesDataTable() {
  const [order, setOrder] = React.useState('asc');
  const [orderBy, setOrderBy] = React.useState('calories');
  const [selected, setSelected] = React.useState([]);
  const [page, setPage] = React.useState(0);
  const [dense, setDense] = React.useState(false);
  const [rowsPerPage, setRowsPerPage] = React.useState(5);

  const handleClick = (event, name) => {
    const selectedIndex = selected.indexOf(name);
    let newSelected = [];

    if (selectedIndex === -1) {
      newSelected = newSelected.concat(selected, name);
    } else if (selectedIndex === 0) {
      newSelected = newSelected.concat(selected.slice(1));
    } else if (selectedIndex === selected.length - 1) {
      newSelected = newSelected.concat(selected.slice(0, -1));
    } else if (selectedIndex > 0) {
      newSelected = newSelected.concat(
        selected.slice(0, selectedIndex),
        selected.slice(selectedIndex + 1),
      );
    }

    setSelected(newSelected);
  };

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const isSelected = (name) => selected.indexOf(name) !== -1;

  // Avoid a layout jump when reaching the last page with empty rows.
  const emptyRows =
    page > 0 ? Math.max(0, (1 + page) * rowsPerPage - rows.length) : 0;

  const visibleRows = React.useMemo(
    () =>
      stableSort(rows, getComparator(order, orderBy)).slice(
        page * rowsPerPage,
        page * rowsPerPage + rowsPerPage,
      ),
    [order, orderBy, page, rowsPerPage],
  );

  return (
    <Box sx={{ width: '100%' }}>
      <Paper sx={{ width: '100%', mb: 2 }}>
        <TableContainer>
          <Table
            sx={{ minWidth: 750 ,maxWidth:'100%'}}
            aria-labelledby="tableTitle"
            size={dense ? 'small' : 'medium'}
          >
            <TableHead>
                <TableRow > 
                <TableCell align="right">id</TableCell>
                <TableCell align="left">Leave Type</TableCell>
                <TableCell align="right">Leave From</TableCell>
                <TableCell align="right">Leave To</TableCell>
                <TableCell align="right">Reason/Comments</TableCell>
                <TableCell align="right">Status</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
              {visibleRows.map((row, index) => {

                return (
                  <TableRow
                    hover
                    key={index}
                  >
                    <TableCell  align="right">{index+1}</TableCell>
                    <TableCell>{row.name}</TableCell>
                    <TableCell align="right">{row.calories}</TableCell>
                    <TableCell align="right">{row.fat}</TableCell>
                    <TableCell align="right">{row.carbs}</TableCell>
                    <TableCell align="right">{row.protein}</TableCell>
                  </TableRow>
                );
              })}
              {emptyRows > 0 && (
                <TableRow
                  style={{
                    height: (dense ? 33 : 53) * emptyRows,
                  }}
                >
                  <TableCell colSpan={6} />
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
        <TablePagination
          rowsPerPageOptions={[5, 10]}
          component="div"
          count={rows.length}
          rowsPerPage={rowsPerPage}
          page={page}
          onPageChange={handleChangePage}
          onRowsPerPageChange={handleChangeRowsPerPage}
        />
      </Paper>
    </Box>
  );
}