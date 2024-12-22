import React, { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import config from '../config';
import './ForumPage.css';


const HomePage = () => {
  return (
    <div className="home-page">
      <h2>Welcome to the Serverland</h2>
    </div>
  );
};

export default HomePage;
