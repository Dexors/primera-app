import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5192/api',
});

// Interceptor para manejar errores
api.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error('API Error:', error.response);
    return Promise.reject(error);
  }
);

// Autenticación
export const login = async (credentials) => {
  const response = await api.post('/auth/login', credentials);
  return response.data;
};

export const register = async (userData) => {
  const response = await api.post('/auth/register', userData);
  return response.data;
};

// Usuarios
export const getUsers = async () => {
  const response = await api.get('/users');
  return response.data;
};

export const getUserById = async (id) => {
  const response = await api.get(`/users/${id}`);
  return response.data;
};

export const createUser = async (user) => {
  const response = await api.post('/users', user);
  return response.data;
};

export const updateUser = async (id, user) => {
  const response = await api.put(`/users/${id}`, user);
  return response.data;
};

export const deleteUser = async (id) => {
  const response = await api.delete(`/users/${id}`);
  return response.data;
};

// Solicitudes de PTO
export const getPtoRequests = async () => {
  const response = await api.get('/ptorequests');
  return response.data;
};

export const getPtoRequestById = async (id) => {
  const response = await api.get(`/ptorequests/${id}`);
  return response.data;
};

export const createPtoRequest = async (ptoRequest) => {
  const response = await api.post('/ptorequests', ptoRequest);
  return response.data;
};

export const updatePtoRequest = async (id, ptoRequest) => {
  const response = await api.put(`/ptorequests/${id}`, ptoRequest);
  return response.data;
};

export const deletePtoRequest = async (id) => {
  const response = await api.delete(`/ptorequests/${id}`);
  return response.data;
};

// Flujos de aprobación
export const getApprovalWorkflows = async () => {
  const response = await api.get('/approvalworkflows');
  return response.data;
};

export const getApprovalWorkflowById = async (id) => {
  const response = await api.get(`/approvalworkflows/${id}`);
  return response.data;
};

export const createApprovalWorkflow = async (approvalWorkflow) => {
  const response = await api.post('/approvalworkflows', approvalWorkflow);
  return response.data;
};

export const updateApprovalWorkflow = async (id, approvalWorkflow) => {
  const response = await api.put(`/approvalworkflows/${id}`, approvalWorkflow);
  return response.data;
};

export const deleteApprovalWorkflow = async (id) => {
  const response = await api.delete(`/approvalworkflows/${id}`);
  return response.data;
};

export default api;