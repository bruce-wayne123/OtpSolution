const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000';

async function post(path, body) {
  const response = await fetch(`${BASE_URL}/api/otp/${path}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body)
  });

  if (!response.ok) {
    throw new Error('Request failed. Please check the values and try again.');
  }

  return response.json();
}

export function generateOtp(userId, dateTime) {
  return post('generate', { userId, dateTime });
}

export function validateOtp(userId, password) {
  return post('validate', { userId, password });
}
