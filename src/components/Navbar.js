import React from 'react';
import { Link } from 'react-router-dom';

function Navbar() {
  return (
    <nav>
      <ul>
        <li><Link to="/">Home</Link></li>
        <li><Link to="/users">Users</Link></li>
        <li><Link to="/pto-requests">PTO Requests</Link></li>
        <li><Link to="/approval-workflows">Approval Workflows</Link></li>
      </ul>
    </nav>
  );
}

export default Navbar;
