import React, { useState } from 'react';
import { createUser } from '../api';



function UserForm() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');

  const handleSubmit = async (event) => {
    event.preventDefault();
    await createUser({ name, email });
    setName('');
    setEmail('');
  };

  return (
    <div>
      <h2>Create User</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Name:</label>
          <input type="text" value={name} onChange={(e) => setName(e.target.value)} />
        </div>
        <div>
          <label>Email:</label>
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
        </div>
        <button type="submit">Create</button>
      </form>
    </div>
  );
}

export default UserForm;
