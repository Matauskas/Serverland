import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import config from '../config';
import { useAuth } from '../context/AuthContext';
import './ServerPage.css';

const ServersPage = () => {
  const { categoryId } = useParams(); // Extract categoryId from the URL
  const [servers, setServers] = useState([]);
  const [editingServer, setEditingServer] = useState(null);
  const [editForm, setEditForm] = useState({ model: '', disk_count: '', generation: '', weight: '' });
  const [newServer, setNewServer] = useState({ model: '', disk_count: '', generation: '', weight: '' });
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const { waitForAccessToken } = useAuth();
  const navigate = useNavigate();

  // Fetch servers when categoryId changes
  useEffect(() => {
    const fetchServers = async () => {
      try {
        const token = await waitForAccessToken();
        const response = await axios.get(
          `${config.apiBaseUrl}/api/category/${categoryId}/server`,
          { headers: { Authorization: `Bearer ${token}` } }
        );
        setServers(response.data);
      } catch (error) {
        console.error('Error fetching servers:', error);
      }
    };

    if (categoryId) {
      fetchServers();
    }
  }, [categoryId, waitForAccessToken]);

  // Open Edit Modal
  const openEditModal = (server) => {
    setEditingServer(server);
    setEditForm({
      model: server.model,
      disk_count: server.disk_count,
      generation: server.generation,
      weight: server.weight,
    });
  };

  // Close Edit Modal
  const closeEditModal = () => setEditingServer(null);

  // Save Edited Server
  const handleEditSave = async () => {
    try {
      const token = await waitForAccessToken();
      await axios.put(
        `${config.apiBaseUrl}/api/category/${categoryId}/server/${editingServer.id}`,
        editForm,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setServers(servers.map((server) =>
        server.id === editingServer.id ? { ...server, ...editForm } : server
      ));
      closeEditModal();
    } catch (error) {
      console.error('Error saving edited server:', error);
    }
  };

  // Open Create Modal
  const openCreateModal = () => {
    setNewServer({ model: '', disk_count: '', generation: '', weight: '' });
    setIsCreateModalOpen(true);
  };

  // Close Create Modal
  const closeCreateModal = () => setIsCreateModalOpen(false);

  // Create a New Server
  const handleCreateSave = async () => {
    try {
      const token = await waitForAccessToken();
      const response = await axios.post(
        `${config.apiBaseUrl}/api/category/${categoryId}/server`,
        newServer,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setServers([response.data, ...servers]);
      closeCreateModal();
    } catch (error) {
      console.error('Error creating server:', error);
    }
  };

  // Handle Server Deletion
  const handleDelete = async (serverId) => {
    try {
      const token = await waitForAccessToken();
      await axios.delete(
        `${config.apiBaseUrl}/api/category/${categoryId}/server/${serverId}`,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setServers(servers.filter((server) => server.id !== serverId));
    } catch (error) {
      console.error('Error deleting server:', error);
    }
  };

  // Navigate to PartPages on click
  const handleServerClick = (serverId) => navigate(`/category/${categoryId}/server/${serverId}`);

  return (
    <div className="servers-page">
      <h2>Servers in Category {categoryId}</h2>
      <button className="create-new-button" onClick={openCreateModal}>Create New Server</button>

      <div className="server-list">
        {servers.length === 0 ? (
          <p>No servers available in this category.</p>
        ) : (
          servers.map((server) => (
            <div
              key={server.id}
              className="server-item"
              onClick={() => handleServerClick(server.id)}
              style={{ cursor: 'pointer' }}
            >
              <h3>{server.model}</h3>
              <p>Disk Count: {server.disk_count}</p>
              <p>Generation: {server.generation}</p>
              <p>Weight: {server.weight}</p>
              <button onClick={(e) => { e.stopPropagation(); openEditModal(server); }}>Edit</button>
              <button onClick={(e) => { e.stopPropagation(); handleDelete(server.id); }}>Delete</button>
            </div>
          ))
        )}
      </div>

      {editingServer && (
        <div className="modal">
          <div className="modal-content">
            <h2>Edit Server</h2>
            <label>Model</label>
            <input
              type="text"
              value={editForm.model}
              onChange={(e) => setEditForm({ ...editForm, model: e.target.value })}
            />
            <label>Disk Count</label>
            <input
              type="number"
              value={editForm.disk_count}
              onChange={(e) => setEditForm({ ...editForm, disk_count: e.target.value })}
            />
            <label>Generation</label>
            <input
              type="text"
              value={editForm.generation}
              onChange={(e) => setEditForm({ ...editForm, generation: e.target.value })}
            />
            <label>Weight</label>
            <input
              type="number"
              value={editForm.weight}
              onChange={(e) => setEditForm({ ...editForm, weight: e.target.value })}
            />
            <div className="modal-actions">
              <button onClick={handleEditSave}>Save</button>
              <button onClick={closeEditModal}>Cancel</button>
            </div>
          </div>
        </div>
      )}

      {isCreateModalOpen && (
        <div className="modal">
          <div className="modal-content">
            <h2>Create New Server</h2>
            <label>Model</label>
            <input
              type="text"
              value={newServer.model}
              onChange={(e) => setNewServer({ ...newServer, model: e.target.value })}
            />
            <label>Disk Count</label>
            <input
              type="number"
              value={newServer.disk_count}
              onChange={(e) => setNewServer({ ...newServer, disk_count: e.target.value })}
            />
            <label>Generation</label>
            <input
              type="text"
              value={newServer.generation}
              onChange={(e) => setNewServer({ ...newServer, generation: e.target.value })}
            />
            <label>Weight</label>
            <input
              type="number"
              value={newServer.weight}
              onChange={(e) => setNewServer({ ...newServer, weight: e.target.value })}
            />
            <div className="modal-actions">
              <button onClick={handleCreateSave}>Create</button>
              <button onClick={closeCreateModal}>Cancel</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ServersPage;
