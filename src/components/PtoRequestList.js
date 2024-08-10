import React, { useState, useEffect } from 'react';
import ptoService from '../services/ptoService';
import '../styles/PtoRequestList.css';

const PtoRequestList = () => {
  const [ptoRequests, setPtoRequests] = useState([]);
  const [availableDays, setAvailableDays] = useState(0);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchPTOData = async () => {
      try {
        setLoading(true);
        const [requests, days] = await Promise.all([
          ptoService.getPTORequests(),
          ptoService.getAvailableDays()
        ]);
        setPtoRequests(requests);
        setAvailableDays(days);
        setError('');
      } catch (error) {
        console.error('Error al obtener datos de PTO:', error.response || error);
        setError(`No se pudieron cargar los datos de PTO. Error: ${error.response?.data?.message || error.message}`);
      } finally {
        setLoading(false);
      }
    };

    fetchPTOData();
  }, []);

  if (loading) {
    return <div>Cargando datos de PTO...</div>;
  }

  if (error) {
    return <div className="error-message">{error}</div>;
  }

  return (
    <div>
      <h2>Solicitudes de PTO</h2>
      <p>Días disponibles: {availableDays}</p>
      {ptoRequests.length === 0 ? (
        <p>No hay solicitudes de PTO pendientes.</p>
      ) : (
        <ul>
          {ptoRequests.map((request) => (
            <li key={request.id}>
              Desde: {new Date(request.startDate).toLocaleDateString()} - 
              Hasta: {new Date(request.endDate).toLocaleDateString()} - 
              Estado: {request.status}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default PtoRequestList;
