import React, { useRef } from 'react';
import './MatrixInput.css';

const MatrixInput = ({ matrix, setMatrix }) => {
    const inputRefs = useRef([]);

    const handleChange = (i, j, val) => {
        const updated = [...matrix];
        updated[i][j] = val === '' ? '' : parseFloat(val);
        setMatrix(updated);
    };

    const handleKeyDown = (e, i, j) => {
        const numRows = matrix.length;
        const numCols = matrix[0]?.length || 0;

        switch (e.key) {
            case 'ArrowUp':
                e.preventDefault();
                if (i > 0) inputRefs.current[(i - 1) * numCols + j]?.focus();
                break;
            case 'ArrowDown':
                e.preventDefault();
                if (i < numRows - 1) inputRefs.current[(i + 1) * numCols + j]?.focus();
                break;
            case 'ArrowLeft':
                if (j > 0) inputRefs.current[i * numCols + (j - 1)]?.focus();
                break;
            case 'ArrowRight':
                if (j < numCols - 1) inputRefs.current[i * numCols + (j + 1)]?.focus();
                break;
            case 'Enter':
                e.preventDefault();
                if (j < numCols - 1) {
                    inputRefs.current[i * numCols + (j + 1)]?.focus(); 
                } else if (i < numRows - 1) {
                    inputRefs.current[(i + 1) * numCols]?.focus(); 
                }
                break;

            default:
                break;
        }
    };


    return (
        <div className="matrix-table-wrapper">
            <table className="matrix-table">
                <tbody>
                    {matrix.map((row, i) => (
                        <tr key={i}>
                            {row.map((val, j) => {
                                const refIndex = i * matrix[0].length + j;
                                return (
                                    <td key={j}>
                                        <input
                                            type="number"
                                            value={val}
                                            ref={el => inputRefs.current[refIndex] = el}
                                            onChange={(e) => handleChange(i, j, e.target.value)}
                                            onKeyDown={(e) => handleKeyDown(e, i, j)}
                                        />
                                    </td>
                                );
                            })}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default MatrixInput;
