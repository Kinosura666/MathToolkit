import axios from 'axios';

const API_BASE_URL = 'https://localhost:7285/api/Sort';

export const sortArray = async ({ array, method, direction = 'Ascending', logSteps = true }) => {
    const algorithm = method.toLowerCase().replace(' sort', '').replace(/\s/g, '');

    const response = await axios.post(API_BASE_URL, {
        array,
        algorithm,
        direction,
        logSteps
    });

    return response.data;
};
