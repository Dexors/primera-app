const BASE_URL = 'http://localhost:5192/api';

const authService = {
  registerUser: async (email, password, username) => {
    try {
      const response = await fetch(`${BASE_URL}/auth/register`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password, username }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Error en el registro');
      }

      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Error en el registro:', error);
      throw error;
    }
  },

  loginUser: async (username, password) => {
    try {
      const response = await fetch(`${BASE_URL}/auth/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, password }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Error en el inicio de sesión');
      }

      const data = await response.json();
      if (data.token) {
        localStorage.setItem('token', data.token);
        localStorage.setItem('user', JSON.stringify({ username }));
        return { token: data.token, user: { username } };
      } else {
        throw new Error('Token no recibido del servidor');
      }
    } catch (error) {
      console.error('Error en el inicio de sesión:', error);
      throw error;
    }
  },

  logoutUser: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },

  getCurrentUser: () => {
    const userString = localStorage.getItem('user');
    if (!userString) {
      console.log('No se encontró usuario en localStorage');
      return null;
    }
    try {
      return JSON.parse(userString);
    } catch (error) {
      console.error('Error al parsear el usuario:', error);
      localStorage.removeItem('user');
      return null;
    }
  },

  fetchCurrentUser: async () => {
    const token = localStorage.getItem('token');
    if (!token) {
      return null;
    }

    try {
      const response = await fetch(`${BASE_URL}/users/me`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) {
        throw new Error('Error al obtener los datos del usuario');
      }

      const userData = await response.json();
      localStorage.setItem('user', JSON.stringify(userData));
      return userData;
    } catch (error) {
      console.error('Error al obtener los datos del usuario:', error);
      return null;
    }
  },

  refreshToken: async () => {
    const token = localStorage.getItem('token');
    if (!token) return null;

    try {
      const response = await fetch(`${BASE_URL}/auth/refresh-token`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
      });

      if (!response.ok) {
        throw new Error('Error al refrescar el token');
      }

      const data = await response.json();
      localStorage.setItem('token', data.token);
      return data.token;
    } catch (error) {
      console.error('Error al refrescar el token:', error);
      return null;
    }
  },

  isAuthenticated: () => {
    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');
    return !!token && !!user;
  },
};

export default authService;
