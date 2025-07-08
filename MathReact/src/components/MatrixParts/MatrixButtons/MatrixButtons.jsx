import React, { useState } from 'react';
import { postSingleMatrix, postPowerMatrix } from '../../../api/matrixApi';
import './MatrixButtons.css';

const MatrixButtons = ({ matrix, setMatrix, onResult, onError }) => {
    const [exponent, setExponent] = useState(2);

    const handleClear = () => {
        const defaultMatrix = [
            ['', '', ''],
            ['', '', ''],
            ['', '', '']
        ];
        setMatrix(defaultMatrix);
        onResult(null);
    };


    const handleTranspose = async () => {
        try {
            const result = await postSingleMatrix("BasicOperations/Transpose", matrix);
            onResult(result);
        } catch (error) {
            const msg = error.response?.data?.error || error.message;
            onError(`Transpose failed: ${msg}`);
        }
    };

    const handleInverse = async () => {
        try {
            const result = await postSingleMatrix('BasicOperations/Inverse', matrix);
            onResult(result);
        } catch (error) {
            const msg = error.response?.data?.error || error.message;
            onError(`Inverse failed: ${msg}`);
        }
    };

    const handleDeterminant = async () => {
        try {
            const result = await postSingleMatrix('BasicOperations/Determinant', matrix);
            onResult([[result]]); 
        } catch (error) {
            const msg = error.response?.data?.error || error.message;
            onError(`Determinant failed: ${msg}`);
        }
    };

    const handleAdd = () => {
        const numCols = matrix[0]?.length || 0;
        const extendedRows = matrix.map(row => [...row, '']);
        const newRow = Array(numCols + 1).fill('');
        setMatrix([...extendedRows, newRow]);
    };

    const handleRemove = () => {
        if (matrix.length <= 1 || matrix[0].length <= 1) return;
        const trimmedRows = matrix.map(row => row.slice(0, -1));
        const trimmedMatrix = trimmedRows.slice(0, -1);
        setMatrix(trimmedMatrix);
    };

    const handlePower = async () => {
        if (!Number.isInteger(+exponent)) {
            onError("Invalid exponent");
            return;
        }
        try {
            const result = await postPowerMatrix(matrix, parseInt(exponent));
            onResult(result);
        } catch (error) {
            const msg = error.response?.data?.error || error.message;
            onError(`Power failed: ${msg}`);
        }
    };
    return (
        <div className="matrix-buttons">
            <div className="matrix-button-row">
                <button onClick={handleAdd}>+</button>
                <button onClick={handleRemove}>-</button>
                <button onClick={handleTranspose}>T</button>
                <button onClick={handleDeterminant}>det</button>
                <button onClick={handleInverse}>inv</button>
                <button onClick={handleClear}>clear</button>
            </div>

            <div className="matrix-power-row">
                <button onClick={handlePower}>To the power of →</button>
                <input
                    type="number"
                    value={exponent}
                    onChange={(e) => setExponent(e.target.value)}
                    className="matrix-power-input"
                />
            </div>
        </div>
    );
};

export default MatrixButtons;
