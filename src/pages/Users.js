import React, { useState } from 'react';

  function Users() {
    const [users] = useState([
      { id: 1, name: 'Juan Pérez', email: 'juan@example.com', role: 'Empleado' },
      { id: 2, name: 'María García', email: 'maria@example.com', role: 'Supervisor' },
      { id: 3, name: 'Carlos Rodríguez', email: 'carlos@example.com', role: 'RRHH' },
    ]);

    return (
      <div className="container mx-auto px-4 py-8">
        <h1 className="text-2xl font-bold mb-4">Gestión de Usuarios</h1>
        <table className="min-w-full bg-white">
          <thead>
            <tr>
              <th className="py-2 px-4 border-b">Nombre</th>
              <th className="py-2 px-4 border-b">Email</th>
              <th className="py-2 px-4 border-b">Rol</th>
              <th className="py-2 px-4 border-b">Acciones</th>
            </tr>
          </thead>
          <tbody>
            {users.map((user) => (
              <tr key={user.id}>
                <td className="py-2 px-4 border-b">{user.name}</td>
                <td className="py-2 px-4 border-b">{user.email}</td>
                <td className="py-2 px-4 border-b">{user.role}</td>
                <td className="py-2 px-4 border-b">
                  <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-1 px-2 rounded mr-2">
                    Editar
                  </button>
                  <button className="bg-red-500 hover:bg-red-700 text-white font-bold py-1 px-2 rounded">
                    Eliminar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    );
  }

  export default Users;