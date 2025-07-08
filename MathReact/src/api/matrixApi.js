import axios from 'axios';

const API_BASE_URL = 'https://localhost:7285/api/matrix';

export const postSingleMatrix = async (endpoint, matrix) => {
    const response = await axios.post(`${API_BASE_URL}/${endpoint}`, {
        A: matrix, 
    });
    return response.data;
};


export const postMatrixPair = async (endpoint, matrixA, matrixB) => {
    const response = await axios.post(`${API_BASE_URL}/${endpoint}`, {
        a: matrixA,
        b: matrixB
    });
    return response.data;
};

export const postPowerMatrix = async (matrix, exponent) => {
    const response = await axios.post(`${API_BASE_URL}/BasicOperations/Power`, {
        a: matrix,
        exponent 
    });
    return response.data;
};
