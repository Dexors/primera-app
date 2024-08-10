import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import '../styles/LoginForm.css';
import authService from '../services/authService';

const LoginForm = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const { login } = useAuth();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setIsLoading(true);
    try {
      const userData = await login(username, password);
      console.log('Inicio de sesión exitoso:', userData);
      if (!authService.isAuthenticated()) {
        throw new Error('No se pudo autenticar correctamente');
      }
      navigate('/pto-requests');
    } catch (error) {
      console.error('Error de inicio de sesión:', error);
      setError('Error al iniciar sesión: ' + (error.response?.data?.message || error.message || 'Ocurrió un error desconocido'));
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="login-container">
      <form onSubmit={handleSubmit} className="login-form">
        <h2>Iniciar Sesión</h2>
        <div className="form-group">
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            placeholder="Correo electrónico o nombre de usuario"
            required
            className="form-input"
          />
        </div>
        <div className="form-group">
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Contraseña"
            required
            className="form-input"
          />
        </div>
        <button type="submit" className="submit-btn" disabled={isLoading}>
          {isLoading ? 'Iniciando sesión...' : 'Iniciar sesión'}
        </button>
        {error && <p className="error-message">{error}</p>}
      </form>
    </div>
  );
};

export default LoginForm;
