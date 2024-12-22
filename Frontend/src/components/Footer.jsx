import React from 'react';
import './Footer.css'; // Make sure this path is correct
import github from '../assets/github-mark-white.svg';

const Footer = () => (
  <footer className="footer">
    <p>&copy; 2024 Serverland</p>
    <div className="footer-icons flex justify-center gap-4">
      <a className="link" href="https://github.com/Matauskas/Serverland" target="_blank" rel="noopener noreferrer">
        <img src={github} alt="X" />
      </a>
      
    </div>
  </footer>
);

export default Footer;
