import React from 'react'
import { Routes, Route, Link } from 'react-router-dom'
import Login from './pages/Login'
import Register from './pages/Register'
import Dashboard from './pages/Dashboard'
import ProjectDetails from './pages/ProjectDetails'

export default function App(){
  return (
    <div style={{padding:20, fontFamily:'Arial'}}>
      <nav style={{marginBottom:20}}>
        <Link to="/">Dashboard</Link> | <Link to="/login">Login</Link> | <Link to="/register">Register</Link>
      </nav>
      <Routes>
        <Route path='/' element={<Dashboard/>} />
        <Route path='/login' element={<Login/>} />
        <Route path='/register' element={<Register/>} />
        <Route path='/projects/:id' element={<ProjectDetails/>} />
      </Routes>
    </div>
  )
}
