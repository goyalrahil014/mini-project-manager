import React, { useEffect, useState } from "react";
import api from "../api/apiClient";
import { Link } from "react-router-dom";

export default function Dashboard() {
  const [projects, setProjects] = useState<any[]>([]);
  const [title, setTitle] = useState("");
  const [desc, setDesc] = useState("");
  const [error, setError] = useState("");

  async function load() {
    try {
      const res = await api.get("/api/projects");
      setProjects(res.data);
    } catch (err: any) {
      setError("Failed to load projects");
    }
  }

  useEffect(() => {
    load();
  }, []);

  async function create(e: React.FormEvent) {
    e.preventDefault();
    try {
      await api.post("/api/projects", { title, description: desc });
      setTitle("");
      setDesc("");
      load();
    } catch (err: any) {
      setError("Failed to create project");
    }
  }

  async function del(id: number) {
    if (!confirm("Delete project?")) return;
    await api.delete(`/api/projects/${id}`);
    load();
  }

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-3xl mx-auto bg-white p-6 rounded-2xl shadow-md">
        <h2 className="text-2xl font-bold mb-4 text-center text-gray-800">
          Project Dashboard
        </h2>

        <form
          onSubmit={create}
          className="flex flex-col md:flex-row gap-3 mb-6 items-center"
        >
          <input
            placeholder="Title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className="border border-gray-300 rounded-lg px-3 py-2 w-full md:w-1/3 focus:ring-2 focus:ring-blue-500 outline-none"
          />
          <input
            placeholder="Description"
            value={desc}
            onChange={(e) => setDesc(e.target.value)}
            className="border border-gray-300 rounded-lg px-3 py-2 w-full md:w-1/2 focus:ring-2 focus:ring-blue-500 outline-none"
          />
          <button
            className="bg-blue-600 hover:bg-blue-700 text-white px-5 py-2 rounded-lg transition duration-200"
            type="submit"
          >
            Create
          </button>
        </form>

        {error && <p className="text-red-600 text-center mb-4">{error}</p>}

        <ul className="space-y-3">
          {projects.map((p) => (
            <li
              key={p.id}
              className="flex justify-between items-center bg-gray-100 hover:bg-gray-200 px-4 py-2 rounded-lg transition"
            >
              <Link
                to={`/projects/${p.id}`}
                className="font-medium text-blue-700 hover:underline"
              >
                {p.title}
              </Link>
              <button
                onClick={() => del(p.id)}
                className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded-md text-sm transition"
              >
                Delete
              </button>
            </li>
          ))}
        </ul>

        {projects.length === 0 && (
          <p className="text-gray-500 text-center mt-6">
            No projects yet. Create one above!
          </p>
        )}
      </div>
    </div>
  );
}
