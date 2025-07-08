import { useState } from 'react';

export const useResultAnimation = () => {
    const [animationClass, setAnimationClass] = useState('');
    const [visible, setVisible] = useState(true);

    const trigger = (callback) => {
        setAnimationClass('fade-out-up');
        setVisible(false); 

        setTimeout(() => {
            callback();        
            setAnimationClass('fade-in-down');
            setVisible(true);   
        }, 300); 
    };

    return { animationClass, visible, trigger };
};
