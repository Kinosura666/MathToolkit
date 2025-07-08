import React from 'react';
import './MatrixBox.css';

const MatrixBox = ({ label, children }) => {
    return (
        <div className="matrix-box">
            <h4>{label}</h4>
            {children}
        </div>
    );
};

export default MatrixBox;
