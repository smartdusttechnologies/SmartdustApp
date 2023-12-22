import { createContext, useState } from "react";

const AuthContext = createContext({});

export const AuthProvider = ({children}) => {
    const [auth , setAuth] = useState({})
    const [notification , setNotification] = useState([])

    return (
        <AuthContext.Provider value={{auth , setAuth , notification , setNotification}}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthContext