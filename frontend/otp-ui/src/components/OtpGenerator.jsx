import { useEffect, useState } from "react";
import { generateOtp } from "../api/otpApi";

const pad = (value) => String(value).padStart(2, "0");

// datetime-local inputs need "yyyy-MM-ddTHH:mm:ss" in local time
function toInputValue(date) {
  return (
    `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}` +
    `T${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`
  );
}

function OtpGenerator({ userId, onUserIdChange }) {
  const [dateTime, setDateTime] = useState(toInputValue(new Date()));
  const [otp, setOtp] = useState(null);
  const [currentTime, setCurrentTime] = useState(Date.now());
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!otp) {
      return undefined;
    }

    const timer = window.setInterval(() => setCurrentTime(Date.now()), 1000);

    return () => window.clearInterval(timer);
  }, [otp]);

  const secondsLeft = otp
    ? Math.max(
        Math.ceil((new Date(otp.expiresAtUtc).getTime() - currentTime) / 1000),
        0,
      )
    : 0;

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");
    setLoading(true);

    try {
      const requestedDateTime = new Date(dateTime).toISOString();
      const result = await generateOtp(userId, requestedDateTime);
      setCurrentTime(Date.now());
      setOtp(result);
    } catch (err) {
      setOtp(null);
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <section className="card">
      <h2>Generate password</h2>

      <form onSubmit={handleSubmit}>
        <label htmlFor="userId">User Id</label>
        <input
          id="userId"
          type="text"
          value={userId}
          onChange={(e) => onUserIdChange(e.target.value)}
          required
        />

        <label htmlFor="dateTime">Date and time</label>
        <input
          id="dateTime"
          type="datetime-local"
          step="1"
          value={dateTime}
          onChange={(e) => setDateTime(e.target.value)}
          required
        />

        <button type="submit" disabled={loading}>
          {loading ? "Generating..." : "Generate"}
        </button>
      </form>

      {error && <p className="message error">{error}</p>}

      {otp && (
        <div className="result">
          <div className="password">{otp.password}</div>
          {secondsLeft > 0 ? (
            <p>Valid for another {secondsLeft} seconds</p>
          ) : (
            <p className="expired">This password has expired</p>
          )}
        </div>
      )}
    </section>
  );
}

export default OtpGenerator;
