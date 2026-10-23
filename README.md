# One-Time Password Solution

Generates a one-time password from a User Id and a DateTime. Each password is valid
for a 30 second window. The UI shows the password, the seconds left, and lets you validate it.

<img width="1896" height="886" alt="image" src="https://github.com/user-attachments/assets/c83c259e-17ac-4a39-b83f-83a2cc1cf0c8" />


## Structure

```
OtpSolution.sln
src/
  Otp.Core/    business logic (interfaces, models, OtpService)
  Otp.Api/     ASP.NET Core Web API (controller, request/response models)
frontend/
  otp-ui/      React (Vite) app
```

## How the password works

- Time is split into 30 second windows.
- Password = HMAC-SHA256(secret, "userId:window") truncated to 6 digits
- The same user and window always give the same password.
- Validation checks the password against the current server time window.
  Generate with the default (current) date time for it to validate successfully.

## Run the API

```
cd src/Otp.Api
dotnet run
```

Runs on http://localhost:5000

## API Endpoints

POST /api/otp/generate `{ "userId": "john", "dateTime": "2026-01-01T12:00:00Z" }`
-> `{ "password": "123456", "expiresAtUtc": "...", "remainingSeconds": 20 }`

POST /api/otp/validate `{ "userId": "john", "password": "123456" }`
-> `{ "isValid": true, "message": "Password is valid." }`

## Run the UI

Requires Node 18+.

```

cd frontend/otp-ui
npm install
npm run dev
```

Open http://localhost:5173. The API url is set in `.env` (`VITE_API_URL`).
