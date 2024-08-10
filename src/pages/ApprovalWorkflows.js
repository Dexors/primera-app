import React, { useState } from 'react';

function ApprovalWorkflows() {
  const [workflows] = useState([
    { id: 1, name: 'Solicitud de vacaciones', steps: ['Empleado', 'Supervisor', 'RRHH'] },
    { id: 2, name: 'Permiso por enfermedad', steps: ['Empleado', 'RRHH'] },
    { id: 3, name: 'Licencia especial', steps: ['Empleado', 'Supervisor', 'Gerente', 'RRHH'] },
  ]);

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-2xl font-bold mb-4">Flujos de Aprobación</h1>
      <div className="grid gap-4">
        {workflows.map((workflow) => (
          <div key={workflow.id} className="bg-white p-4 rounded shadow">
            <h2 className="text-xl font-semibold mb-2">{workflow.name}</h2>
            <ol className="list-decimal list-inside">
              {workflow.steps.map((step, index) => (
                <li key={index} className="mb-1">{step}</li>
              ))}
            </ol>
          </div>
        ))}
      </div>
    </div>
  );
}

export default ApprovalWorkflows;