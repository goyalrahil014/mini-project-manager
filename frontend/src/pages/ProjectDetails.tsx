import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../api/apiClient";

type Task = {
  id: number;
  title: string;
  dueDate?: string | null;
  isCompleted: boolean;
};

type Project = {
  id: number;
  title: string;
  description?: string | null;
  createdAt?: string;
  tasks: Task[];
};

export default function ProjectDetails(): JSX.Element {
  const { id } = useParams<{ id?: string }>();
  const navigate = useNavigate();
  const [project, setProject] = useState<Project | null>(null);
  const [taskTitle, setTaskTitle] = useState<string>("");
  const [error, setError] = useState<string>("");

  async function load() {
    setError("");
    if (!id) return;
    try {
      const res = await api.get<Project>(`/api/projects/${id}`);
      console.log("Loaded project:", res.data);
      setProject(res.data);
    } catch (err: any) {
      setError("Failed to load project details");
    }
  }

  useEffect(() => {
    load();
  }, [id]);

  async function addTask(e: React.FormEvent) {
    e.preventDefault();
    setError("");
    if (!taskTitle.trim()) {
      setError("Task title required");
      return;
    }
    try {
      await api.post(`/api/projects/${id}/tasks`, { title: taskTitle });
      setTaskTitle("");
      await load();
    } catch (err: any) {
      setError(err?.response?.data?.message ?? "Failed to add task");
    }
  }

  async function toggle(task: Task) {
    setError("");
    console.log("Toggling task:", task.id, task.isCompleted);
    try {
      await api.put(`/api/tasks/${task.id}`, {
        title: task.title,
        dueDate: task.dueDate ?? null,
        isCompleted: !task.isCompleted,
      });
      await load();
    } catch (err: any) {
      setError(err?.response?.data?.message ?? "Failed to update task");
    }
  }

  async function delTask(taskId: number) {
    setError("");
    console.log("Deleting task:", taskId);
    try {
      await api.delete(`/api/tasks/${taskId}`);
      await load();
    } catch (err: any) {
      setError(err?.response?.data?.message ?? "Failed to delete task");
    }
  }

  if (!project) {
    return (
      <div className="flex items-center justify-center h-40 text-gray-600">
        Loading project details...
      </div>
    );
  }

  return (
    <div className="max-w-2xl mx-auto bg-white shadow-lg rounded-2xl p-6 mt-10">
      <button
        onClick={() => navigate("/")}
        className="text-sm text-blue-500 hover:text-blue-700 mb-4"
        type="button"
      >
        ← Back to Dashboard
      </button>

      <h2 className="text-2xl font-bold text-gray-800 mb-2">
        {project.title}
      </h2>
      <p className="text-gray-600 mb-6">
        {project.description ?? "No description provided."}
      </p>

      <h3 className="text-xl font-semibold mb-3 text-gray-800">Tasks</h3>

      <form onSubmit={addTask} className="flex mb-4">
        <input
          value={taskTitle}
          onChange={(e) => setTaskTitle(e.target.value)}
          placeholder="Enter new task title..."
          className="flex-grow border border-gray-300 rounded-l-lg p-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
        />
        <button
          type="submit"
          className="bg-blue-600 text-white px-4 rounded-r-lg hover:bg-blue-700 transition"
        >
          Add
        </button>
      </form>

      {error && <p className="text-red-500 text-sm mb-3">{error}</p>}

      <ul className="divide-y divide-gray-200 border border-red-500 rounded-lg p-2">
        {project.tasks.length === 0 && (
          <p className="text-gray-500 text-sm mt-3">No tasks yet.</p>
        )}
        {project.tasks.map((t) => (
          <li
            key={t.id}
            className="flex items-center justify-between py-3 px-4 bg-gray-50 hover:bg-gray-100 rounded-lg shadow-sm mt-2"
          >
            <div className="flex items-center gap-2">
              <input
                type="checkbox"
                checked={t.isCompleted}
                onChange={() => toggle(t)}
                className="accent-green-600 h-5 w-5 cursor-pointer"
              />
              <span
                className={
                  t.isCompleted
                    ? "line-through text-gray-500"
                    : "text-gray-800 font-medium"
                }
              >
                {t.title}
              </span>
            </div>

            <div className="flex items-center gap-4">
              {t.dueDate && (
                <span className="text-xs text-gray-500">
                  {new Date(t.dueDate).toLocaleDateString()}
                </span>
              )}
              <button
                onClick={() => delTask(t.id)}
                type="button"
                className="px-3 py-1 bg-red-500 text-white text-sm rounded-md hover:bg-red-600 transition"
              >
                Delete
              </button>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}
