import React from 'react';
import { Link } from 'react-router-dom';
import '../styles/WelcomePage.css';

const WelcomePage = () => {
  return (
    <div className="welcome-container">
      <nav className="welcome-nav">
        <div className="nav-logo">VacationTrack</div>
        <div className="nav-links">
          <Link to="/about">Sobre Nosotros</Link>
          <Link to="/contact">Contacto</Link>
        </div>
      </nav>

      <header className="welcome-header">
        <h1>VacationTrack</h1>
        <p>Gestiona tus días libres con facilidad</p>
      </header>
      
      <main className="welcome-content">
        <section className="features">
          <div className="feature-item">
            <div className="feature-icon">🗓️</div>
            <h2>Control de Vacaciones</h2>
            <p>Planifica y gestiona tus días libres de forma eficiente</p>
          </div>
          <div className="feature-item">
            <div className="feature-icon">⏱️</div>
            <h2>Seguimiento de Tiempo</h2>
            <p>Monitorea tus horas trabajadas y descansos</p>
          </div>
          <div className="feature-item">
            <div className="feature-icon">👥</div>
            <h2>Colaboración en Equipo</h2>
            <p>Coordina ausencias con tu equipo fácilmente</p>
          </div>
        </section>
        
        <section className="testimonials">
          <h2>Lo que dicen nuestros usuarios</h2>
          <div className="testimonial-container">
            <div className="testimonial">
              <p>"VacationTrack ha simplificado enormemente la gestión de mis vacaciones."</p>
              <span>- María G., Desarrolladora</span>
            </div>
            <div className="testimonial">
              <p>"Una herramienta indispensable para nuestro equipo de recursos humanos."</p>
              <span>- Carlos R., Gerente de RRHH</span>
            </div>
          </div>
        </section>

        <section className="cta">
          <h2>¿Listo para optimizar tu tiempo libre?</h2>
          <div className="cta-buttons">
            <Link to="/register" className="btn btn-primary">Registrarse</Link>
            <Link to="/login" className="btn btn-secondary">Iniciar Sesión</Link>
          </div>
        </section>
      </main>
      
      <footer className="welcome-footer">
        <p>&copy; 2024 VacationTrack. Todos los derechos reservados.</p>
      </footer>
    </div>
  );
};

export default WelcomePage;