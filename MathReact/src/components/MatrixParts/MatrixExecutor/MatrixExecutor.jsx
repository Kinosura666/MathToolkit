import React from 'react';
import { postSingleMatrix } from "../../../api/matrixApi";
import './MatrixExecutor.css';

const MatrixExecutor = ({ method, matrixA, onResult, onError }) => {
    const handleExecute = async () => {
        try {
            let res;

            switch (method) {
                //  Spectral Methods 
                case 'Power Iteration':
                    res = await postSingleMatrix('EigenvalueMethods/PowerIteration', matrixA);
                    break;
                case 'Inverse Iteration':
                    res = await postSingleMatrix('EigenvalueMethods/InversePowerIteration', matrixA);
                    break;
                case 'Rayleigh Quotient Iteration':
                    res = await postSingleMatrix('EigenvalueMethods/RayleighQuotientIteration', matrixA);
                    break;
                case 'Jacobi Method':
                    res = await postSingleMatrix('EigenvalueMethods/JacobiEigenSolver', matrixA);
                    break;
                case 'QR Method':
                    res = await postSingleMatrix('EigenvalueMethods/QREigenValues', matrixA);
                    break;
                case 'LR Method':
                    res = await postSingleMatrix('EigenvalueMethods/LREigenValues', matrixA);
                    break;

                //  Polynomial Methods 
                case 'Leverrier-Faddeev':
                    res = await postSingleMatrix('EigenvalueMethods/LeverrierFaddeev', matrixA);
                    break;
                case 'Krylov Method':
                    res = await postSingleMatrix('EigenvalueMethods/KrylovCharacteristicPolynomial', matrixA);
                    break;

                //  Decompositions 
                case 'LU Decomposition':
                    res = await postSingleMatrix('Decompositions/LUDecomposition', matrixA);
                    break;
                case 'QR Decomposition':
                    res = await postSingleMatrix('Decompositions/QRDecomposition', matrixA);
                    break;
                case 'Cholesky Decomposition':
                    res = await postSingleMatrix('Decompositions/CholeskyDecomposition', matrixA);
                    break;
                case 'SVD Decomposition':
                    res = await postSingleMatrix('Decompositions/SVD', matrixA);
                    break;

                // --- Matrix Analysis ---
                case 'Matrix Norms':
                    res = await postSingleMatrix('MatrixStats/Norms', matrixA);
                    break;
                case 'Condition Number':
                    res = await postSingleMatrix('MatrixStats/ConditionNumber2', matrixA);
                    break;
                case 'Singular Values':
                    res = await postSingleMatrix('MatrixStats/GetSingularValues', matrixA);
                    break;
                case 'Gershgorin Discs':
                    res = await postSingleMatrix('EigenvalueMethods/GershgorinDiscs', matrixA);
                    break;
                case 'Pseudo-Inverse':
                    res = await postSingleMatrix('BasicOperations/PseudoInverse', matrixA);
                    break;
                case 'Determinant':
                    res = await postSingleMatrix('BasicOperations/Determinant', matrixA);
                    break;
                case 'Inverse Matrix':
                    res = await postSingleMatrix('BasicOperations/Inverse', matrixA);
                    break;
                case 'Transpose Matrix':
                    res = await postSingleMatrix('BasicOperations/Transpose', matrixA);
                    break;
                case 'Symmetrize Matrix':
                    res = await postSingleMatrix('BasicOperations/Symmetrize', matrixA);
                    break;
                case 'Matrix Rank':
                    res = await postSingleMatrix('BasicOperations/Rank', matrixA);
                    break;

                default:
                    onError('Select a valid method.');
                    return;
            }
            onResult(res); 

        } catch (err) {
            const msg = err.response?.data?.error || err.message;
            onError(`Execute failed: ${msg}`);
        }
    };

    return (
        <div className="executor">
            <button onClick={handleExecute} disabled={!method}>
                Execute
            </button>
        </div>
    );
};

export default MatrixExecutor;
