import React, { useState } from 'react';
import { postMatrixPair } from "../../../api/matrixApi";
import './MatrixOperationBar.css';

const MatrixOperationBar = ({
    matrixA,
    matrixB,
    setMatrixA,
    setMatrixB,
    setResultMatrix
}) => {
    const [error, setError] = useState('');

    const handleOp = async (op) => {
        setError('');
        try {
            const endpoint =
                op === '+' ? 'BasicOperations/Add' :
                    op === '-' ? 'BasicOperations/Subtract' :
                        op === '×' ? 'BasicOperations/Multiply' : '';

            if (!endpoint) return;

            const result = await postMatrixPair(endpoint, matrixA, matrixB);
            setResultMatrix(result);
        } catch (e) {
            setError(e.message || 'Error');
        }
        setTimeout(() => {
            setError('');
        }, 5000);
    };

    const handleMatrixTransfer = (type) => {
        const copyA = matrixA.map(row => [...row]);
        const copyB = matrixB.map(row => [...row]);

        switch (type) {
            case 'AtoB':
                setMatrixB(copyA);
                break;
            case 'BtoA':
                setMatrixA(copyB);
                break;
            case 'Swap':
                setMatrixA(copyB);
                setMatrixB(copyA);
                break;
        }
    };

    return (
        <div className="matrix-operations">
            <div className="matrix-buttons">
                <button onClick={() => handleOp('+')} title="Add A and B">A + B</button>
                <button onClick={() => handleOp('-')} title="Subtract B from A">A - B</button>
                <button onClick={() => handleOp('×')} title="Multiply A by B">A × B</button>
            </div>
            <hr style={{ width: '80%', borderColor: '#444' }} />
            <div className="matrix-buttons">
                <button onClick={() => handleMatrixTransfer('AtoB')} title="Copy A into B">A → B</button>
                <button onClick={() => handleMatrixTransfer('BtoA')} title="Copy B into A">A ← B</button>
                <button onClick={() => handleMatrixTransfer('Swap')} title="Swap A and B">A ↔ B</button>
            </div>
            {error && <p style={{ color: 'red' }}>{error}</p>}
        </div>
    );
};

export default MatrixOperationBar;
