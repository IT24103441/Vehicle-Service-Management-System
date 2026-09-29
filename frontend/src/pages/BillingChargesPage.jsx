import { useState } from 'react'
import { partChargeApi } from '../services/api'

export default function BillingChargesPage() {
  const [jobCardId, setJobCardId] = useState('')
  const [charges, setCharges] = useState([])
  const [error, setError] = useState('')
  async function load(e) {
    e.preventDefault(); setError('')
    try { setCharges(await partChargeApi.getByJob(Number(jobCardId))) } catch (err) { setError(err.message || 'Unable to load part charges.') }
  }
  return <main className="portal-main"><header className="portal-topbar"><div><span className="portal-eyebrow">BILLING</span><h1>Part Charges</h1></div></header><div className="portal-content">
    <section className="portal-card"><form className="inventory-form" onSubmit={load}><label>Job card ID<input required min="1" type="number" value={jobCardId} onChange={e => setJobCardId(e.target.value)} /></label><div className="inventory-form-actions"><button className="portal-primary-button">View charges</button></div></form></section>
    {error && <div className="portal-error">{error}</div>}<section className="portal-card"><div className="portal-table-wrapper"><table className="portal-table"><thead><tr><th>Part</th><th>Quantity</th><th>Unit price</th><th>Total</th></tr></thead><tbody>{charges.length ? charges.map(c => <tr key={c.id}><td>{c.sparePartName}</td><td>{c.quantity}</td><td>{Number(c.unitPrice).toFixed(2)}</td><td>{Number(c.totalAmount).toFixed(2)}</td></tr>) : <tr><td colSpan="4">No part charges found.</td></tr>}</tbody></table></div></section>
  </div></main>
}
