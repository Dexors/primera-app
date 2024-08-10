import axios from 'axios';
import authService from './authService';

const API_URL = 'http://localhost:5192/api';

const axiosInstance = axios.create({
  baseURL: API_URL,
  timeout: 10000,
});

axiosInstance.interceptors.request.use(
  async (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
      console.log('Token añadido a la solicitud:', token);
    } else {
      console.log('No se encontró token en localStorage');
      // Si no hay token, intenta refrescarlo
      const newToken = await authService.refreshToken();
      if (newToken) {
        config.headers['Authorization'] = `Bearer ${newToken}`;
        console.log('Nuevo token obtenido y añadido a la solicitud:', newToken);
      } else {
        console.log('No se pudo obtener un nuevo token');
      }
    }
    return config;
  },
  (error) => {
    console.error('Error en el interceptor de solicitud:', error);
    return Promise.reject(error);
  }
);

axiosInstance.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response && error.response.status === 401) {
      // Token expirado, intenta refrescarlo
      const newToken = await authService.refreshToken();
      if (newToken) {
        // Reintenta la solicitud original con el nuevo token
        const config = error.config;
        config.headers['Authorization'] = `Bearer ${newToken}`;
        return axiosInstance(config);
      } else {
        // Si no se puede refrescar el token, redirige al login
        authService.logoutUser();
        window.location.href = '/login';
      }
    }
    return Promise.reject(error);
  }
);

const ptoService = {
  getAvailableDays: async () => {
    try {
      const response = await axiosInstance.get('/ptorequests/available-days');
      return response.data;
    } catch (error) {
      console.error('Error al obtener días disponibles:', error.response || error);
      throw error;
    }
  },

  getPTORequests: async () => {
    try {
      const response = await axiosInstance.get('/ptorequests');
      return response.data;
    } catch (error) {
      console.error('Error al obtener solicitudes de PTO:', error.response || error);
      throw error;
    }
  },

  requestPTO: async (startDate, endDate) => {
    try {
      console.log('Enviando solicitud de PTO:', { startDate, endDate });
      const response = await axiosInstance.post('/ptorequests', { startDate, endDate });
      return response.data;
    } catch (error) {
      console.error('Error al enviar solicitud de PTO:', error.response?.data || error.message);
      if (error.response?.data?.message) {
        throw new Error(error.response.data.message);
      } else if (error.response?.status === 400) {
        throw new Error('La solicitud no es válida. Por favor, verifica las fechas y asegúrate de que tienes suficientes días disponibles.');
      } else if (error.response?.status === 401) {
        throw new Error('No estás autorizado. Por favor, inicia sesión nuevamente.');
      } else {
        throw new Error('Ocurrió un error al procesar la solicitud. Por favor, intenta de nuevo más tarde.');
      }
    }
  },
};

export default ptoService;
