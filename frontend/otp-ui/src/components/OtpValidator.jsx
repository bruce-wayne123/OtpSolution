import { useState } from 'react';
import { validateOtp } from '../api/otpApi';

function OtpValidator({ userId }) {
  const [password, setPassword] = useState('');
  const [result, setResult] = useState(null);
  const [error, setError] = useState('');

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError('');
    setResult(null);

    try {
      setResult(await validateOtp(userId, password));
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <section className="card">
      <h2>Validate password</h2>

      <form onSubmit={handleSubmit}>
        <label htmlFor="password">Password for user "{userId || '-'}"</label>
        <input
          id="password"
          type="text"
          inputMode="numeric"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />

        <button type="submit" disabled={!userId}>Validate</button>
      </form>

      {error && <p className="message error">{error}</p>}

      {result && (
        <p className={`message ${result.isValid ? 'success' : 'error'}`}>
          {result.message}
        </p>
      )}
    </section>
  );
}

export default OtpValidator;
