import React from 'react';
import PtoRequestForm from '../components/PtoRequestForm';
import PtoRequestList from '../components/PtoRequestList';
import '../styles/PtoRequests.css';

const PtoRequests = () => {
  return (
    <div className="pto-container">
      <h1 className="pto-title">Gestión de PTO</h1>
      <div className="pto-card">
        <h2 className="pto-card-title">Solicitar PTO</h2>
        <PtoRequestForm />
      </div>
      <div className="pto-card">
        <h2 className="pto-card-title">Mis solicitudes de PTO</h2>
        <PtoRequestList />
      </div>
    </div>
  );
};

export default PtoRequests;
