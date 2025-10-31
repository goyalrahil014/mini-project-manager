# Project Manager - Full Stack (Minimal)

## Overview
Minimal project management app implemented to satisfy the assignment:
- Backend: .NET 8 Web API + EF Core + SQLite + JWT Authentication
- Frontend: React + TypeScript (Vite) minimal UI

## How to run

### Backend
1. `cd backend`
2. `dotnet restore`
3. `dotnet run`
   - By default runs on https://localhost:5001 and http://localhost:5000 (Kestrel). If using http, update `VITE_API_BASE` in frontend.

### Frontend
1. `cd frontend`
2. `npm install`
3. Create a `.env` in `frontend` with:
   ```
   VITE_API_BASE=http://localhost:5000
   ```
4. `npm start`

## Notes
- The backend uses SQLite file `projectmanager.db` in the backend folder.
- Default JWT secret is in `appsettings.json`. For production, change it.
- This is a minimal scaffold for the assignment. You can extend validation, UI, and error handling as needed.
