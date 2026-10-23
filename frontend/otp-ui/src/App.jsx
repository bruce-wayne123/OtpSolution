import { useState } from 'react';
import OtpGenerator from './components/OtpGenerator';
import OtpValidator from './components/OtpValidator';

function App() {
  const [userId, setUserId] = useState('');

  return (
    <main className="container">
      <h1>One-Time Password</h1>
      <OtpGenerator userId={userId} onUserIdChange={setUserId} />
      <OtpValidator userId={userId} />
    </main>
  );
}

export default App;
