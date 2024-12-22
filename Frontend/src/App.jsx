import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ProtectedRoute from './components/ProtectedRoute';
import ForumPage from './pages/ForumPage';
import ServersPage from './pages/ServerPage';
import Layout from './components/layout';
import ContactPage from './pages/ContactPage'; 
import HomePage from './pages/HomePage';
import PartPage from './pages/PartsPage'

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          {/* Unauthenticated Routes */}
          <Route path="/" element={<Layout><HomePage /> </Layout>} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          
          {/* Routes with Layout */}
          <Route 
            path="/category" 
            element={
                <Layout>
                  <ForumPage />
                </Layout>
            } 
          />
          <Route 
            path="/category/:categoryId"  
            element={
                <Layout>
                  <ServersPage />
                </Layout>
            } 
          />
          <Route 
            path="/category/:categoryId/server/:serverId"  
            element={
                <Layout>
                  <PartPage />
                </Layout>
            } 
          />
          {/* Fallback 404 */}
          <Route path="*" element={<Layout><div>404 Page Not Found</div></Layout>} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}


export default App;
