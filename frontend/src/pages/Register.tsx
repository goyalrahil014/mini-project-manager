import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import api from '../api/apiClient';

export default function Register() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [name, setName] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  async function submit(e: React.FormEvent) {
    e.preventDefault();
    try {
      await api.post('/api/auth/register', { name, email, password });
      navigate('/login');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Registration failed');
    }
  }

  return (
    <div className="flex items-center justify-center min-h-screen bg-gradient-to-br from-indigo-100 via-white to-indigo-50">
      <div className="bg-white shadow-2xl rounded-2xl p-8 w-full max-w-md border border-indigo-100">
        <h2 className="text-3xl font-bold text-center text-indigo-600 mb-2">
          Create Account
        </h2>
        <p className="text-center text-gray-500 mb-8">
          Register to manage your projects easily
        </p>

        <form onSubmit={submit} className="space-y-5">
          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Name
            </label>
            <input
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-400 focus:outline-none"
              placeholder="Enter your name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
            />
          </div>

          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Email
            </label>
            <input
              type="email"
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-400 focus:outline-none"
              placeholder="Enter your email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Password
            </label>
            <input
              type="password"
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-400 focus:outline-none"
              placeholder="Create a password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          {error && <p className="text-red-500 text-sm">{error}</p>}

          <button
            type="submit"
            className="w-full py-2 bg-indigo-600 text-white rounded-lg hover:bg-indigo-700 transition duration-200 font-semibold"
          >
            Sign Up
          </button>
        </form>

        <p className="text-sm text-gray-500 text-center mt-5">
          Already have an account?{' '}
          <Link to="/login" className="text-indigo-600 hover:underline">
            Login
          </Link>
        </p>
      </div>
    </div>
  );
}
