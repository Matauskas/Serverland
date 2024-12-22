import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import config from '../config';
import { useAuth } from '../context/AuthContext';
import './PartsPage.css'; // Ensure this is the correct path to the CSS file

const PartsPage = () => {
  const { categoryId, serverId } = useParams(); // Extract categoryId and serverId from the URL
  const [parts, setParts] = useState([]);
  const [editingPart, setEditingPart] = useState(null);
  const [editForm, setEditForm] = useState({
    cpu: '',
    ram: '',
    raid: '',
    network: '',
    ssd: '',
    hdd: '',
    psu: '',
    rails: '',
  });
  const [newPart, setNewPart] = useState({
    cpu: '',
    ram: '',
    raid: '',
    network: '',
    ssd: '',
    hdd: '',
    psu: '',
    rails: '',
  });
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const { waitForAccessToken } = useAuth();

  // Fetch parts when serverId changes
  useEffect(() => {
    const fetchParts = async () => {
      try {
        const token = await waitForAccessToken();
        const response = await axios.get(
          `${config.apiBaseUrl}/api/category/${categoryId}/server/${serverId}/part`,
          { headers: { Authorization: `Bearer ${token}` } }
        );
        setParts(response.data);
      } catch (error) {
        console.error('Error fetching parts:', error);
      }
    };

    if (serverId) {
      fetchParts();
    }
  }, [serverId, categoryId, waitForAccessToken]);

  // Open Edit Modal
  const openEditModal = (part) => {
    setEditingPart(part);
    setEditForm({
      cpu: part.cpu,
      ram: part.ram,
      raid: part.raid,
      network: part.network,
      ssd: part.ssd,
      hdd: part.hdd,
      psu: part.psu,
      rails: part.rails,
    });
  };

  // Close Edit Modal
  const closeEditModal = () => setEditingPart(null);

  // Save Edited Part
  const handleEditSave = async () => {
    try {
      const token = await waitForAccessToken();
      await axios.put(
        `${config.apiBaseUrl}/api/category/${categoryId}/server/${serverId}/part/${editingPart.id}`,
        editForm,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setParts(parts.map((part) =>
        part.id === editingPart.id ? { ...part, ...editForm } : part
      ));
      closeEditModal();
    } catch (error) {
      console.error('Error saving edited part:', error);
    }
  };

  // Open Create Modal
  const openCreateModal = () => {
    setNewPart({
      cpu: '',
      ram: '',
      raid: '',
      network: '',
      ssd: '',
      hdd: '',
      psu: '',
      rails: '',
    });
    setIsCreateModalOpen(true);
  };

  // Close Create Modal
  const closeCreateModal = () => setIsCreateModalOpen(false);

  // Create a New Part
  const handleCreateSave = async () => {
    try {
      const token = await waitForAccessToken();
      const partData = {
        cpu: newPart.cpu,
        ram: newPart.ram,
        raid: newPart.raid,
        network: newPart.network,
        ssd: newPart.ssd,
        hdd: newPart.hdd,
        psu: newPart.psu,
        rails: newPart.rails === 'true' || newPart.rails === true, // Ensure boolean type
        serverId: parseInt(serverId, 10), // Ensure numeric type
      };
  
      const response = await axios.post(
        `${config.apiBaseUrl}/api/category/${categoryId}/server/${serverId}/part`,
        partData,
        { headers: { Authorization: `Bearer ${token}` } }
      );
  
      setParts([response.data, ...parts]);
      closeCreateModal();
    } catch (error) {
      console.error('Error creating part:', error);
    }
  };
  

  // Handle Part Deletion
  const handleDelete = async (partId) => {
    try {
      const token = await waitForAccessToken();
      await axios.delete(
        `${config.apiBaseUrl}/api/category/${categoryId}/server/${serverId}/part/${partId}`,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setParts(parts.filter((part) => part.id !== partId));
    } catch (error) {
      console.error('Error deleting part:', error);
    }
  };

  return (
    <div className="parts-page">
      <h2>Parts for Server {serverId}</h2>
      <button className="create-new-button" onClick={openCreateModal}>Create New Part</button>

      <div className="part-list">
        {parts.length === 0 ? (
          <p>No parts available for this server.</p>
        ) : (
          parts.map((part) => (
            <div key={part.id} className="part-item">
              <p>CPU: {part.cpu}</p>
              <p>RAM: {part.ram} GB</p>
              <p>RAID: {part.raid}</p>
              <p>Network: {part.network}</p>
              <p>SSD: {part.ssd} GB</p>
              <p>HDD: {part.hdd} GB</p>
              <p>PSU: {part.psu} W</p>
              <p>Rails: {part.rails}</p>
              <button className="edit-button" onClick={() => openEditModal(part)}>Edit</button>
              <button className="delete-button" onClick={() => handleDelete(part.id)}>Delete</button>
            </div>
          ))
        )}
      </div>

      {editingPart && (
        <div className="modal">
          <div className="modal-content">
            <h2>Edit Part</h2>
            <label>CPU</label>
            <input
              type="text"
              value={editForm.cpu}
              onChange={(e) => setEditForm({ ...editForm, cpu: e.target.value })}
            />
            <label>RAM</label>
            <input
              type="number"
              value={editForm.ram}
              onChange={(e) => setEditForm({ ...editForm, ram: e.target.value })}
            />
            <label>RAID</label>
            <input
              type="text"
              value={editForm.raid}
              onChange={(e) => setEditForm({ ...editForm, raid: e.target.value })}
            />
            <label>Network</label>
            <input
              type="text"
              value={editForm.network}
              onChange={(e) => setEditForm({ ...editForm, network: e.target.value })}
            />
            <label>SSD</label>
            <input
              type="number"
              value={editForm.ssd}
              onChange={(e) => setEditForm({ ...editForm, ssd: e.target.value })}
            />
            <label>HDD</label>
            <input
              type="number"
              value={editForm.hdd}
              onChange={(e) => setEditForm({ ...editForm, hdd: e.target.value })}
            />
            <label>PSU</label>
            <input
              type="number"
              value={editForm.psu}
              onChange={(e) => setEditForm({ ...editForm, psu: e.target.value })}
            />
            <label>Rails</label>
            <input
              type="text"
              value={editForm.rails}
              onChange={(e) => setEditForm({ ...editForm, rails: e.target.value })}
            />
            <div className="modal-actions">
              <button className="save-button" onClick={handleEditSave}>Save</button>
              <button className="cancel-button" onClick={closeEditModal}>Cancel</button>
            </div>
          </div>
        </div>
      )}

      {isCreateModalOpen && (
        <div className="modal">
          <div className="modal-content">
            <h2>Create New Part</h2>
            <label>CPU</label>
            <input
              type="text"
              value={newPart.cpu}
              onChange={(e) => setNewPart({ ...newPart, cpu: e.target.value })}
            />
            <label>RAM</label>
            <input
              type="number"
              value={newPart.ram}
              onChange={(e) => setNewPart({ ...newPart, ram: e.target.value })}
            />
            <label>RAID</label>
            <input
              type="text"
              value={newPart.raid}
              onChange={(e) => setNewPart({ ...newPart, raid: e.target.value })}
            />
            <label>Network</label>
            <input
              type="text"
              value={newPart.network}
              onChange={(e) => setNewPart({ ...newPart, network: e.target.value })}
            />
            <label>SSD</label>
            <input
              type="number"
              value={newPart.ssd}
              onChange={(e) => setNewPart({ ...newPart, ssd: e.target.value })}
            />
            <label>HDD</label>
            <input
              type="number"
              value={newPart.hdd}
              onChange={(e) => setNewPart({ ...newPart, hdd: e.target.value })}
            />
            <label>PSU</label>
            <input
              type="number"
              value={newPart.psu}
              onChange={(e) => setNewPart({ ...newPart, psu: e.target.value })}
            />
            <label>Rails</label>
            <input
              type="text"
              value={newPart.rails}
              onChange={(e) => setNewPart({ ...newPart, rails: e.target.value })}
            />
            <div className="modal-actions">
              <button className="save-button" onClick={handleCreateSave}>Create</button>
              <button className="cancel-button" onClick={closeCreateModal}>Cancel</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default PartsPage;
