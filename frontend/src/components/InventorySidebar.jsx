import { Boxes, ClipboardList, LogOut, PackageCheck } from 'lucide-react'
import { useNavigate } from 'react-router-dom'
import { authApi, clearAuth, getRole } from '../services/api'

function InventorySidebar() {
  const navigate = useNavigate()
  const role = getRole()

  async function handleLogout() {
    try { await authApi.logout() } catch { /* JWT logout is stateless. */ }
    clearAuth()
    navigate('/login')
  }

  return (
    <aside className="customer-sidebar">
      <div>
        <div className="sidebar-brand">
          <div className="sidebar-brand-icon"><Boxes size={20} /></div>
          <div><strong>INVENTORY</strong><span>MANAGEMENT PORTAL</span></div>
        </div>
        <nav className="sidebar-navigation">
          <button className="sidebar-link active" onClick={() => navigate('/inventory/spare-parts')}>
            <span className="sidebar-link-icon"><ClipboardList size={17} /></span>
            Spare Parts
          </button>
          <button className="sidebar-link" onClick={() => navigate('/inventory/part-requests')}>
            <span className="sidebar-link-icon"><PackageCheck size={17} /></span>
            Part Requests
          </button>
          {role === 'Administrator' && (
            <button className="sidebar-link" onClick={() => navigate('/admin')}>Administrator Portal</button>
          )}
        </nav>
      </div>
      <div className="sidebar-bottom">
        <div className="sidebar-user-pill">
          <div className="sidebar-user-avatar">{role === 'Administrator' ? 'A' : 'I'}</div>
          <div className="sidebar-user-info"><strong>{role || 'Inventory Officer'}</strong><span>Staff Portal</span></div>
        </div>
        <button className="sidebar-logout" onClick={handleLogout}><LogOut size={16} />Logout</button>
      </div>
    </aside>
  )
}

export default InventorySidebar
