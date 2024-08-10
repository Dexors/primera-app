import React, { useState, useEffect } from 'react';
import { getApprovalWorkflows } from '../api';

function ApprovalWorkflowList() {
  const [approvalWorkflows, setApprovalWorkflows] = useState([]);

  useEffect(() => {
    const fetchApprovalWorkflows = async () => {
      const data = await getApprovalWorkflows();
      setApprovalWorkflows(data);
    };
    fetchApprovalWorkflows();
  }, []);

  return (
    <div>
      <h2>Approval Workflow List</h2>
      <ul>
        {approvalWorkflows.map(approvalWorkflow => (
          <li key={approvalWorkflow.id}>{approvalWorkflow.requestId} - {approvalWorkflow.status}</li>
        ))}
      </ul>
    </div>
  );
}

export default ApprovalWorkflowList;
