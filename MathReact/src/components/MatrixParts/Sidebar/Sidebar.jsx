import React, { useState } from 'react';
import './Sidebar.css';

const methods = {
    'Spectral Methods': ['Power Iteration', 'Inverse Iteration', 'Rayleigh Quotient Iteration', 'Jacobi Method', 'QR Method', 'LR Method'],
    'Polynomial Methods': ['Leverrier-Faddeev', 'Krylov Method'],
    'Decompositions': ['LU Decomposition', 'QR Decomposition', 'Cholesky Decomposition', 'SVD Decomposition'],
    'Matrix Analysis': ['Matrix Norms', 'Condition Number', 'Singular Values', 'Gershgorin Discs', 'Pseudo-Inverse', 'Determinant', 'Inverse Matrix', 'Transpose Matrix', 'Symmetrize Matrix', 'Matrix Rank'],
};

const Sidebar = ({ selectedMethod, setSelectedMethod }) => {
    const [expandedSections, setExpandedSections] = useState(() => {
        const initial = {};
        for (const key in methods) initial[key] = true;
        return initial;
    });

    const toggleSection = (section) => {
        setExpandedSections(prev => ({
            ...prev,
            [section]: !prev[section]
        }));
    };

    return (
        <div className="sidebar">
            {Object.entries(methods).map(([category, list]) => (
                <div key={category}>
                    <h4
                        style={{ cursor: 'pointer', marginBottom: '4px' }}
                        onClick={() => toggleSection(category)}
                    >
                        {expandedSections[category] ? '▾' : '▸'} {category}
                    </h4>

                    {expandedSections[category] && (
                        <ul style={{ marginBottom: '10px', paddingLeft: '14px' }}>
                            {list.map(method => (
                                <li
                                    key={method}
                                    onClick={() => setSelectedMethod(method)}
                                    className={`sidebar-method ${selectedMethod === method ? 'active' : ''}`}
                                >
                                    {method}
                                </li>

                            ))}
                        </ul>
                    )}
                </div>
            ))}
        </div>
    );
};

export default Sidebar;
