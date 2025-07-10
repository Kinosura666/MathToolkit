import React, { useState } from 'react';
import styles from './SortingPanel.module.css';

const SortPanel = ({ onGenerate }) => {
    const [arrayText, setArrayText] = useState('');
    const [size1, setSize1] = useState(10);
    const [size2, setSize2] = useState(10);
    const [sortedPercent, setSortedPercent] = useState(10);
    const [size3, setSize3] = useState(10);
    const [dupPercent, setDupPercent] = useState(50);
    const [useDecimal, setUseDecimal] = useState(false);
    const [min, setMin] = useState(0);
    const [max, setMax] = useState(100);

    const handleGenerate = (type) => {
        const size = type === 'full' ? size1 : type === 'partial' ? size2 : size3;
        const result = [];

        for (let i = 0; i < size; i++) {
            let val;
            if (useDecimal) {
                val = (Math.random() * (max - min) + min).toFixed(2);
            } else {
                val = Math.floor(Math.random() * (max - min + 1)) + min;
            }
            result.push(Number(val));
        }

        if (type === 'partial') {
            const sortedCount = Math.floor((sortedPercent / 100) * size);
            result.sort((a, b) => a - b);
            for (let i = sortedCount; i < result.length; i++) {
                const j = Math.floor(Math.random() * result.length);
                [result[i], result[j]] = [result[j], result[i]];
            }
        }

        if (type === 'duplicates') {
            const dupCount = Math.floor((dupPercent / 100) * size);
            for (let i = 0; i < dupCount; i++) {
                const idx = Math.floor(Math.random() * size);
                result[idx] = result[Math.floor(Math.random() * size)];
            }
        }

        setArrayText(result.join(', '));
        onGenerate && onGenerate(result);
    };

    const handleClear = () => {
        setArrayText('');
        onGenerate && onGenerate([]);
    };

    return (
        <div className={styles.sortPanel}>
            <label>Array to sort:</label>
            <textarea
                className={styles.textarea}
                value={arrayText}
                onChange={(e) => setArrayText(e.target.value)}
                rows={2}
            />

            <div className={styles.controlRow}>
                <div className={styles.controlGroup}>
                    <button onClick={() => handleGenerate('full')}>Generate Full Random</button>
                    <span>Size:</span>
                    <input type="number" value={size1} onChange={e => setSize1(+e.target.value)} className={styles.input} />
                </div>

                <div className={styles.controlGroup}>
                    <button onClick={() => handleGenerate('partial')}>Generate Partially Sorted</button>
                    <span>Size:</span>
                    <input type="number" value={size2} onChange={e => setSize2(+e.target.value)} className={styles.input} />
                    <span>Sorted %</span>
                    <input type="number" value={sortedPercent} onChange={e => setSortedPercent(+e.target.value)} className={styles.input} />
                </div>

                <div className={styles.controlGroup}>
                    <button onClick={() => handleGenerate('duplicates')}>Generate With Duplicates</button>
                    <span>Size:</span>
                    <input type="number" value={size3} onChange={e => setSize3(+e.target.value)} className={styles.input} />
                    <span>Dup. %</span>
                    <input type="number" value={dupPercent} onChange={e => setDupPercent(+e.target.value)} className={styles.input} />
                </div>
            </div>

            <div className={styles.sortCheckbox}>
                <label>
                    <input type="checkbox" checked={useDecimal} onChange={() => setUseDecimal(!useDecimal)} />
                    Use decimal (double)
                </label>
            </div>

            <div className={styles.rangeInputs}>
                <span>Min:</span>
                <input type="number" value={min} onChange={e => setMin(+e.target.value)} className={styles.input} />
                <span>Max:</span>
                <input type="number" value={max} onChange={e => setMax(+e.target.value)} className={styles.input} />
            </div>

            <div style={{ marginTop: '10px' }}>
                <button onClick={handleClear}>Clear</button>
            </div>
        </div>
    );
};

export default SortPanel;
