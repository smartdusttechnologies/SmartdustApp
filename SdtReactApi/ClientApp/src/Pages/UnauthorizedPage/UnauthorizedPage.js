import React from 'react';

const UnauthorizedPage = () => {
    const pageStyle = {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        height: '90vh',
        fontFamily: 'Arial, sans-serif',
        backgroundColor: '#f8f8f8',
    };

    const headingStyle = {
        fontSize: '27px',
        fontWeight: 'bold',
        marginBottom: '20px',
        color: '#333',
    };

    const messageStyle = {
        fontSize: '18px',
        color: '#555',
    };

    return (
        <div style={pageStyle}>
            <h1 style={headingStyle}>Sorry, you are not allowed to access this page</h1>
            <p style={messageStyle}>
                Please contact the administrator for assistance or go back to the homepage.
            </p>
        </div>
    );
};

export default UnauthorizedPage;
