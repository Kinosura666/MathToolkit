import React, { useState } from 'react';
import Sidebar from '../components/Shared/Sidebar/Sidebar';
import MatrixInput from '../components/MatrixParts/MatrixInput/MatrixInput';
import MatrixButtons from '../components/MatrixParts/MatrixButtons/MatrixButtons';
import MatrixOperationBar from '../components/MatrixParts/MatrixOperationBar/MatrixOperationBar';
import MatrixExecutor from '../components/MatrixParts/MatrixExecutor/MatrixExecutor';
import MethodResult from '../components/MatrixParts/MethodResult/MethodResult';
import MatrixBox from '../components/MatrixParts/MatrixBox/MatrixBox';
import { useResultAnimation } from '../hooks/useResultAnimation';
import '../styles/global.css';
import SortPanelWrapper from '../components/SortingParts/SortPanelWrapper/SortPanelWrapper'

const MainPage = () => {
    const [selectedMethod, setSelectedMethod] = useState(null);
    const [executedMethod, setExecutedMethod] = useState(null);
    const { animationClass, visible, trigger } = useResultAnimation();
    const [selectedCategory, setSelectedCategory] = useState(null);

    const [matrixA, setMatrixA] = useState([
        [2, 1, 3],
        [1, 2, 1],
        [1, 0, 1]
    ]);

    const [matrixB, setMatrixB] = useState([
        [-2, -1, -3],
        [-1, -2, -1],
        [-1, 0, -1]
    ]);

    const [globalResult, setGlobalResult] = useState({ matrix: null, error: '' });

    const handleGlobalResult = (matrix) => {
        setGlobalResult({ matrix, error: '' });
        setExecutedMethod(selectedMethod);
        trigger(() => {
            setGlobalResult({ matrix, error: '' });
        });
    };

    const handleGlobalError = (message) => {
        setGlobalResult({ matrix: null, error: message });
    };

    const handlePairResult = (matrix) => {
        setGlobalResult({ matrix, error: '' });
    };

    return (
        <div className="matrix-page">
            <Sidebar
                selectedMethod={selectedMethod}
                setSelectedMethod={setSelectedMethod}
                onCategoryChange={setSelectedCategory}
            />

            <div className="main-content">
                <h2>
                    {executedMethod ? `Last executed: ${executedMethod}` : 'Select a Method'}
                </h2>

                {selectedCategory === 'Sorting' ? (
                    <SortPanelWrapper
                        selectedMethod={selectedMethod}
                        setGlobalResult={setGlobalResult}
                        setExecutedMethod={setExecutedMethod}
                    />
                ) : (
                    <>
                        <div className="matrix-table-wrapper">
                            <div className="matrix-row">
                                <MatrixBox label="Matrix A">
                                    <MatrixInput matrix={matrixA} setMatrix={setMatrixA} />
                                    <MatrixButtons
                                        matrix={matrixA}
                                        setMatrix={setMatrixA}
                                        onResult={handleGlobalResult}
                                        onError={handleGlobalError}
                                    />
                                </MatrixBox>

                                <MatrixOperationBar
                                    matrixA={matrixA}
                                    matrixB={matrixB}
                                    setMatrixA={setMatrixA}
                                    setMatrixB={setMatrixB}
                                    setResultMatrix={handlePairResult}
                                />

                                <MatrixBox label="Matrix B">
                                    <MatrixInput matrix={matrixB} setMatrix={setMatrixB} />
                                    <MatrixButtons
                                        matrix={matrixB}
                                        setMatrix={setMatrixB}
                                        onResult={handleGlobalResult}
                                        onError={handleGlobalError}
                                    />
                                </MatrixBox>
                            </div>
                        </div>

                        <MatrixExecutor
                            method={selectedMethod}
                            matrixA={matrixA}
                            matrixB={matrixB}
                            exponent={2}
                            onResult={handleGlobalResult}
                            onError={handleGlobalError}
                        />
                    </>
                )}

                <MethodResult
                    result={globalResult.matrix}
                    error={globalResult.error}
                    animation={animationClass}
                    visible={visible}
                />
            </div>
        </div>
    );
};

export default MainPage;
