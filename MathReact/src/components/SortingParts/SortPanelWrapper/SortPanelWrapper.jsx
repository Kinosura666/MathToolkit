import React, { useState } from 'react';
import SortPanel from '../SortingPanel/SortingPanel';
import { sortArray } from '../../../api/sortApi';
import styles from './SortPanelWrapper.module.css';

const SortPanelWrapper = ({ selectedMethod, setGlobalResult, setExecutedMethod }) => {
    const [arrayToSort, setArrayToSort] = useState([]);
    const [sortResult, setSortResult] = useState(null); 

    const handleSort = async () => {
        try {
            const data = await sortArray({
                array: arrayToSort,
                method: selectedMethod
            });

            setSortResult(data); 
            setGlobalResult({ matrix: null, error: '' });
            setExecutedMethod?.(selectedMethod);
        } catch (err) {
            setSortResult(null); 
            setGlobalResult({ matrix: null, error: err.message });
        }
    };

  

    return (
        <div className={styles.sortPanelWrapper}>
            <SortPanel onGenerate={setArrayToSort} />

            <div className="executor" style={{ marginTop: '12px' }}>
                <button
                    onClick={handleSort}
                    disabled={!selectedMethod || arrayToSort.length === 0}
                >
                    Sort
                </button>
            </div>

            {sortResult && (
                <div className={styles.sortOutputSection}>
                    <div className={styles.sortedArrayBox}>
                        <strong>Sorted array:</strong><br />
                        {sortResult.sortedArray.join(', ')}
                    </div>

                    <div className={styles.sortStepsBox}>
                        <strong>Steps:</strong><br />
                        {sortResult.steps?.length > 0
                            ? sortResult.steps.map((step, index) => (
                                <div key={index}>{step}</div>
                            ))
                            : 'No steps recorded.'}
                    </div>

                    <div className={styles.sortMetaBox}>
                        <div><strong>Comparisons:</strong> {sortResult.comparisons}</div>
                        <div><strong>Swaps:</strong> {sortResult.swaps}</div>
                        <div>
                            <strong>Time:</strong>{' '}
                            {sortResult.duration
                                ? `${sortResult.duration.milliseconds?.toFixed(2)} ms`
                                : 'N/A'}
                        </div>

                    </div>
                </div>
            )}
        </div>
    );
};

export default SortPanelWrapper;
