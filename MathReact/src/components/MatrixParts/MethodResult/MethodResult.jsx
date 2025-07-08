import React from 'react';
import './MethodResult.css';

const renderMatrixTable = (matrix, title = "Matrix result") => {
    return (
        <div style={{ marginTop: '6px' }}>
            {title && <div style={{ marginBottom: '4px' }}><strong>{title}:</strong></div>}
            <table className="matrix-table clean-table">
                <tbody>
                    {matrix.map((row, rowIndex) => (
                        <tr key={rowIndex}>
                            {row.map((val, colIndex) => (
                                <td key={colIndex} style={{ padding: '6px 12px', textAlign: 'right' }}>
                                    {val.toFixed(5)}
                                </td>
                            ))}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

const MethodResult = ({ result, error, animation, visible = true }) => {
    if (!visible) return null;
    if (!result && !error) return null;

    if (error) {
        return (
            <div className="result-box error-box">
                <p style={{ color: 'red', margin: 0 }}>
                    ❌ <strong>Error:</strong> {error}
                </p>
            </div>
        );
    }

    if (typeof result === 'string') {
        try {
            result = JSON.parse(result);
        } catch {
            return <pre>{result}</pre>;
        }
    }
    // Handle single number result
    if (typeof result === 'number') {
        return (
            <div className={`result-box ${animation || ''}`}>
                <strong>Result:</strong> {result.toFixed(5)}
            </div>
        );
    }

    if (Array.isArray(result)) {
        if (typeof result[0] === 'number') {
            return (
                <div className={`result-box ${animation || ''}`}>
                    <strong>Values:</strong> [{result.map(x => x.toFixed(5)).join(', ')}]
                </div>
            );
        }

        return (
            <div className={`result-box ${animation || ''}`}>
                <div><strong>Matrix result:</strong></div>
                <table className="matrix-table clean-table" style={{ marginTop: '6px' }}>
                    <tbody>
                        {result.map((row, rowIndex) => (
                            <tr key={rowIndex}>
                                {row.map((val, colIndex) => (
                                    <td key={colIndex} style={{ padding: '6px 12px', textAlign: 'right' }}>
                                        {val.toFixed(5)}
                                    </td>
                                ))}
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        );


    }
    // Eigenvalue and Eigenvector results
    if (result.eigenvalue !== undefined && result.eigenvector) {
        return (
            <div className={`result-box ${animation || ''}`}>
                <div><strong>Eigenvalue:</strong> {result.eigenvalue.toFixed(5)}</div>
                <div><strong>Eigenvector:</strong> [{result.eigenvector.map(v => v.toFixed(5)).join(', ')}]</div>
                <div><strong>Iterations:</strong> {result.iterations}</div>
                <div><strong>Converged:</strong> <span style={{ color: result.converged ? 'lightgreen' : 'red' }}>{result.converged ? '✅ true' : '❌ false'}</span></div>
            </div>
        );
    }

    if (result.eigenvalues) {
        return (
            <div className={`result-box ${animation || ''}`}>
                <div><strong>Eigenvalues:</strong> [{result.eigenvalues.map(e => e.toFixed(5)).join(', ')}]</div>
                {result.iterations !== undefined && (
                    <div><strong>Iterations:</strong> {result.iterations}</div>
                )}
                {result.converged !== undefined && (
                    <div><strong>Converged:</strong> <span style={{ color: result.converged ? 'lightgreen' : 'red' }}>{result.converged ? '✅ true' : '❌ false'}</span></div>
                )}
            </div>
        );
    }
    // Characteristic Polynomial results
    if (result?.coefficients) {
        const coeffs = result.coefficients;
        const polyString = coeffs
            .map((coef, idx) => {
                const power = coeffs.length - 1 - idx;
                const formatted = `${coef >= 0 && idx > 0 ? '+' : ''}${coef.toFixed(3)}`;
                return power === 0
                    ? ` ${formatted}`
                    : ` ${formatted}λ${power > 1 ? `^${power}` : ''}`;
            })
            .join(' ');

        return (
            <div className={`result-box ${animation || ''}`}>
                <div><strong>Characteristic Polynomial:</strong></div>
                <div style={{ marginTop: '6px' }}>P(λ) = {polyString}</div>

                <div style={{ marginTop: '12px' }}>
                    <strong>Coefficients:</strong>
                    <ul style={{ paddingLeft: '20px', marginTop: '4px' }}>
                        {coeffs.map((c, i) => (
                            <li key={i}>a<sub>{i}</sub> = {c.toFixed(5)}</li>
                        ))}
                    </ul>
                </div>
            </div>
        );
    }

    // Decomposition results 
    if (result?.matrix1 && result?.matrix2) {
        return (
            <>
                <div className={`result-box ${animation || ''}`}>
                    {renderMatrixTable(result.matrix1, result.matrix1Name || 'Matrix 1')}
                </div>
                <div className={`result-box ${animation || ''}`}>
                    {renderMatrixTable(result.matrix2, result.matrix2Name || 'Matrix 2')}
                </div>
            </>
        );
    }

    // Cholesky
    if (result?.matrix1 && !result?.matrix2) {
        return (
            <div className={`result-box ${animation || ''}`}>
                <div><strong>{result.matrix1Name || 'Matrix'}:</strong></div>
                {renderMatrixTable(result.matrix1)}
            </div>
        );
    }
    // SVD result
    if (result?.u && result?.s && result?.vt) {
        return (
            <>
                <div className={`result-box ${animation || ''}`}>
                    <div><strong>U:</strong></div>
                    {renderMatrixTable(result.u, null)}
                </div>

                <div className={`result-box ${animation || ''}`}>
                    <div><strong>S:</strong></div>
                    {renderMatrixTable(result.s, null)}
                </div>

                <div className={`result-box ${animation || ''}`}>
                    <div><strong>Vᵀ:</strong></div>
                    {renderMatrixTable(result.vt, null)}
                </div>
            </>
        );
    }

    // Gershgorin Discs
    if (result?.discs && Array.isArray(result.discs)) {
        return (
            <div className={`result-box ${animation || ''}`}>
                <div><strong>Gershgorin Discs:</strong></div>
                <ul style={{ paddingLeft: '20px', marginTop: '6px' }}>
                    {result.discs.map((disc, i) => (
                        <li key={i}>
                            D<sub>{i + 1}</sub>: center = {disc.center.toFixed(5)}, radius = {disc.radius.toFixed(5)}
                        </li>
                    ))}
                </ul>
                <div style={{ marginTop: '10px' }}>
                    <strong>Bounds for Eigenvalues:</strong><br />
                    Min: {result.minBound.toFixed(5)}<br />
                    Max: {result.maxBound.toFixed(5)}
                </div>
            </div>
        );
    }
    // Matrix Norms
    if (
        result?.frobenius !== undefined &&
        result?.oneNorm !== undefined &&
        result?.infNorm !== undefined &&
        result?.twoNorm !== undefined
    ) {
        return (
            <div className={`result-box ${animation || ''}`}>
                <div><strong>Matrix Norms:</strong></div>
                <ul style={{ paddingLeft: '20px', marginTop: '6px' }}>
                    <li>‣ Frobenius (Euclidean) norm: ‖A‖<sub>F</sub> ≈ {result.frobenius.toFixed(5)}</li>
                    <li>‣ 1-norm: ‖A‖<sub>1</sub> ≈ {result.oneNorm.toFixed(5)}</li>
                    <li>‣ ∞-norm: ‖A‖<sub>∞</sub> ≈ {result.infNorm.toFixed(5)}</li>
                    <li>‣ 2-norm: ‖A‖<sub>2</sub> ≈ {result.twoNorm.toFixed(5)}</li>
                </ul>
            </div>
        );
    }

    return <pre>{JSON.stringify(result, null, 2)}</pre>;
};

export default MethodResult;
