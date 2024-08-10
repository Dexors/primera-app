import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import ptoService from '../services/ptoService';
import authService from '../services/authService';
import '../styles/PtoRequestForm.css';

const PtoRequestForm = () => {
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [error, setError] = useState('');
  const [availableDays, setAvailableDays] = useState(0);
  const navigate = useNavigate();

  useEffect(() => {
    const checkAuthAndFetchDays = async () => {
      if (!authService.isAuthenticated()) {
        navigate('/login');
        return;
      }
      try {
        const days = await ptoService.getAvailableDays();
        setAvailableDays(days);
      } catch (error) {
        console.error('Error al obtener días disponibles:', error);
        setError('No se pudieron obtener los días disponibles. Por favor, intenta de nuevo más tarde.');
      }
    };
    checkAuthAndFetchDays();
  }, [navigate]);

  const formatDate = (date) => {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const day = String(d.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const start = new Date(startDate);
      const end = new Date(endDate);
      
      if (isNaN(start.getTime()) || isNaN(end.getTime())) {
        throw new Error('Las fechas ingresadas no son válidas');
      }
      
      if (start > end) {
        throw new Error('La fecha de inicio debe ser anterior o igual a la fecha de fin');
      }
      
      const formattedStartDate = formatDate(startDate);
      const formattedEndDate = formatDate(endDate);
      
      console.log('Enviando solicitud con fechas:', formattedStartDate, formattedEndDate);
      
      await ptoService.requestPTO(formattedStartDate, formattedEndDate);
      alert('Solicitud de PTO enviada con éxito');
      navigate('/pto-requests');
    } catch (error) {
      console.error('Error al enviar la solicitud de PTO:', error);
      setError(`Error al enviar la solicitud: ${error.message || 'Ocurrió un error desconocido'}`);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="pto-form">
      <input
        type="date"
        placeholder="Fecha de inicio"
        value={startDate}
        onChange={(e) => setStartDate(e.target.value)}
        className="pto-input"
        required
      />
      <input
        type="date"
        placeholder="Fecha de fin"
        value={endDate}
        onChange={(e) => setEndDate(e.target.value)}
        className="pto-input"
        required
      />
      {error && <div className="error-message">{error}</div>}
      <div>Días disponibles: {availableDays}</div>
      <button type="submit" className="pto-button">
        Enviar solicitud
      </button>
    </form>
  );
};

export default PtoRequestForm;
