import { useState, useEffect } from 'react'
import { Coffee } from '../../models/coffee'
import './Home.css'

const API_BASE = 'http://localhost:5000'

function Home() {
  const [coffees, setCoffees] = useState<Coffee[] | null>(null)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    fetch(`${API_BASE}/api/coffees`)
      .then(res => {
        if (!res.ok) throw new Error('Network response was not ok')
        return res.json()
      })
      .then((data: Coffee[]) => setCoffees(data))
      .catch(() => setError('Failed to load coffees'))
  }, [])

  return (
    <div className="container py-4">
      <h1 className="display-5 fw-semibold mb-3">Coffee2Live</h1>
      <p className="text-secondary mb-4">Curated coffees you will love.</p>

      {error && <div className="alert alert-danger">{error}</div>}
      {!coffees && !error && <div className="text-secondary">Loading…</div>}

      {coffees && (
        <div className="row g-3">
          {coffees.map(c => (
            <div key={c.id} className="col-12 col-md-6 col-lg-4">
              <div className="card coffee-card h-100">
                <div className="card-body">
                  <h5 className="card-title">{c.name}</h5>
                  <h6 className="card-subtitle mb-2 text-muted">{c.origin}</h6>
                  <p className="card-text small mb-2">{c.tastingNotes}</p>
                  <div className="small">
                    <span className="badge text-bg-light me-1">Roast: {c.roast}</span>
                    <span className="badge text-bg-light me-1">Acidity: {c.acidity}</span>
                    <span className="badge text-bg-light me-1">Body: {c.body}/5</span>
                    <span className="badge text-bg-light me-1">Bitterness: {c.bitterness}/10</span>
                  </div>
                </div>
                <div className="card-footer text-muted small">Best for: {c.bestFor}</div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default Home
