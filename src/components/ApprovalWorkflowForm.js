import React, { useState } from 'react';
import { createApprovalWorkflow } from '../api';

function ApprovalWorkflowForm() {
  const [requestId, setRequestId] = useState('');
  const [status, setStatus] = useState('');

  const handleSubmit = async (event) => {
    event.preventDefault();
    await createApprovalWorkflow({ requestId, status });
    setRequestId('');
    setStatus('');
  };

  return (
    <div>
      <h2>Create Approval Workflow</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Request ID:</label>
          <input type="text" value={requestId} onChange={(e) => setRequestId(e.target.value)} />
        </div>
        <div>
          <label>Status:</label>
          <input type="text" value={status} onChange={(e) => setStatus(e.target.value)} />
        </div>
        <button type="submit">Create</button>
      </form>
    </div>
  );
}

export default ApprovalWorkflowForm;
