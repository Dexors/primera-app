import React, { createContext, useState, useContext, useEffect } from 'react';
import authService from '../services/authService';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const checkAuthStatus = async () => {
      try {
        const currentUser = authService.getCurrentUser();
        if (currentUser) {
          // Intentar obtener datos actualizados del servidor
          const updatedUser = await authService.fetchCurrentUser();
          setUser(updatedUser || currentUser);
        }
      } catch (error) {
        console.error('Error al verificar la autenticación:', error);
        authService.logoutUser(); // Esto eliminará el token y los datos del usuario
      } finally {
        setLoading(false);
      }
    };

    checkAuthStatus();
  }, []);

  const login = async (username, password) => {
    try {
      const userData = await authService.loginUser(username, password);
      setUser(userData.user);
      return userData.user;
    } catch (error) {
      console.error('Error de inicio de sesión:', error);
      throw error;
    }
  };

  const logout = () => {
    authService.logoutUser();
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, loading, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
