import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import config from '../config';
import { useAuth } from '../context/AuthContext';
import './ForumPage.css';
import dayjs from 'dayjs';

const ForumPage = () => {
  const [categories, setCategories] = useState([]);
  const [editingCategory, setEditingCategory] = useState(null);
  const [editManufacturer, setEditManufacturer] = useState('');
  const [newCategoryManufacturer, setNewCategoryManufacturer] = useState('');
  const [newCategoryServerType, setNewCategoryServerType] = useState('');
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const { accessToken, waitForAccessToken, loading } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const token = await waitForAccessToken();
        const response = await axios.get(`${config.apiBaseUrl}/api/category`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        console.log(response.data);
        setCategories(response.data);
      } catch (error) {
        console.error('Error fetching categories:', error);
      }
    };
    fetchCategories();
  }, [waitForAccessToken]);

  const handleDelete = async (categoryId) => {
    try {
      const token = await waitForAccessToken();
      await axios.delete(`${config.apiBaseUrl}/api/category/${categoryId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setCategories(categories.filter((category) => category.id !== categoryId));
    } catch (error) {
      if (error.response && error.response.status === 403) {
        alert('Unauthorized access');
      } else {
        console.error('Error deleting category:', error);
      }
    }
  };

  const handleEditSave = async () => {
    try {
      const token = await waitForAccessToken();
      await axios.put(
        `${config.apiBaseUrl}/api/category/${editingCategory.id}`,
        { manifacturer: editManufacturer },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setCategories(categories.map((category) =>
        category.id === editingCategory.id
          ? { ...category, manifacturer: editManufacturer }
          : category
      ));
      closeEditModal();
    } catch (error) {
      if (error.response && error.response.status === 403) {
        alert('Unauthorized access');
      } else {
        console.error('Error saving edits:', error);
      }
    }
  };

  const openEditModal = (category) => {
    setEditingCategory(category);
    setEditManufacturer(category.manifacturer);
  };

  const closeEditModal = () => {
    setEditingCategory(null);
  };

  const openCreateModal = () => {
    setNewCategoryManufacturer('');
    setNewCategoryServerType('');
    setIsCreateModalOpen(true);
  };

  const closeCreateModal = () => {
    setIsCreateModalOpen(false);
  };

  const handleCreateSave = async () => {
    try {
      const token = await waitForAccessToken();
      const response = await axios.post(
        `${config.apiBaseUrl}/api/category`,
        {
          manifacturer: newCategoryManufacturer,
          serverType: newCategoryServerType,
        },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setCategories([response.data, ...categories]);
      closeCreateModal();
    } catch (error) {
      console.error('Error creating category:', error);
    }
  };

  const handleCategoryClick = (categoryId) => {
    navigate(`/category/${categoryId}`);
  };

  if (loading) return <p>Loading...</p>;

  return (
    <div className="category-list">
      <h2>Welcome to Server Shop!</h2>
      <p>Choose the server category you want to configure</p>

      {accessToken && (
        <div className="create-new-button">
          <button className="edit-button" onClick={openCreateModal}>
            Create New Category
          </button>
        </div>
      )}

      <div className="category-grid">
        {categories.map((category) => (
          <div
            key={category.id}
            className="category-item"
            onClick={() => handleCategoryClick(category.id)}
          >
            <div className="category-header">
              <h3>Manufacturer: {category.manifacturer}</h3>
              <p>Server Type: {category.serverType}</p>
            </div>
            {accessToken && (
              <div className="category-actions">
                <button
                  className="edit-button"
                  onClick={(e) => {
                    e.stopPropagation();
                    openEditModal(category);
                  }}
                >
                  Edit
                </button>
                <button
                  className="delete-button"
                  onClick={(e) => {
                    e.stopPropagation();
                    handleDelete(category.id);
                  }}
                >
                  Delete
                </button>
              </div>
            )}
          </div>
        ))}
      </div>

      {editingCategory && (
        <div className="modal">
          <div className="modal-content">
            <h2>Edit Category</h2>
            <label>Manufacturer</label>
            <input
              type="text"
              value={editManufacturer}
              onChange={(e) => setEditManufacturer(e.target.value)}
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
            <h2>Create New Category</h2>
            <label>Manufacturer</label>
            <input
              type="text"
              value={newCategoryManufacturer}
              onChange={(e) => setNewCategoryManufacturer(e.target.value)}
            />
            <label>Server Type</label>
            <input
              type="text"
              value={newCategoryServerType}
              onChange={(e) => setNewCategoryServerType(e.target.value)}
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

export default ForumPage;
