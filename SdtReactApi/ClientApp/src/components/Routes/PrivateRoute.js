import React, { useContext } from 'react'
import AuthContext from '../../context/AuthProvider';
import { Navigate } from 'react-router-dom';

const PrivateRoute = ({children}) => {
    const {auth} = useContext(AuthContext);

    if(!auth.isAuthenticated){
        return <Navigate to={'/login'} />
    }

  return children;
}

export default PrivateRoute
