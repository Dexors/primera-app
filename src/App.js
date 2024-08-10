import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import WelcomePage from './pages/WelcomePage';
import LoginForm from './components/LoginForm';
import RegisterForm from './components/RegisterForm';
import ApprovalWorkflows from './pages/ApprovalWorkflows';
import PtoRequests from './pages/PtoRequests';
import Users from './pages/Users';

const ProtectedRoute = ({ children }) => {
  const isAuthenticated = !!localStorage.getItem('token');
  return isAuthenticated ? children : <Navigate to="/login" />;
};

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/" element={<WelcomePage />} />
          <Route path="/login" element={<LoginForm />} />
          <Route path="/register" element={<RegisterForm />} />
          <Route path="/approval-workflows" element={<ProtectedRoute><ApprovalWorkflows /></ProtectedRoute>} />
          <Route path="/pto-requests" element={<ProtectedRoute><PtoRequests /></ProtectedRoute>} />
          <Route path="/users" element={<ProtectedRoute><Users /></ProtectedRoute>} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
