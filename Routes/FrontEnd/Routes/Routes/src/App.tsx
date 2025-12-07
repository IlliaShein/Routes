import './App.css';
import Button from '@mui/material/Button';
import Checkbox from '@mui/material/Checkbox';
import FormControlLabel from '@mui/material/FormControlLabel';
import { useState } from 'react';

function App() {
  const [vehicles, setVehicles] = useState(1);
  const [iterations, setIterations] = useState(100);
  const [priority, setPriority] = useState<'time' | 'distance'>('time');

  const uploadFile = () => {
    console.log("Processing file...");
    console.log("Vehicles:", vehicles);
    console.log("Iterations:", iterations);
    console.log("Priority (fitness):", priority);
  };

  return (
    <>
      <div className="file-upload">
        <label htmlFor="fileInput">Coordinates file:</label>
        <input type="file" id="fileInput" name="fileInput" />
      </div>

      <div className="input-container">
        <div className="input-group">
          <label htmlFor="vehicles">Vehicles amount:</label>
          <input
            type="number"
            id="vehicles"
            value={vehicles}
            min={1}
            onChange={(e) => setVehicles(Number(e.target.value))}
          />
        </div>

        <div className="input-group">
          <label htmlFor="iterations">Iterations:</label>
          <input
            type="number"
            id="iterations"
            value={iterations}
            min={1}
            onChange={(e) => setIterations(Number(e.target.value))}
          />
        </div>
      </div>

      <div className="priority-container">
        <FormControlLabel
          control={
            <Checkbox
              checked={priority === 'time'}
              onChange={() => setPriority('time')}
            />
          }
          label="Time"
        />
        <FormControlLabel
          control={
            <Checkbox
              checked={priority === 'distance'}
              onChange={() => setPriority('distance')}
            />
          }
          label="Distance"
        />
      </div>
      
      <div className="button-container">
        <Button
          variant="contained"
          color="primary"
          onClick={uploadFile}
          style={{ minWidth: '200px', padding: '10px 30px' }}
        >
          Process
        </Button>
      </div>

      <div className="iframe-wrapper">
        <iframe
          src="/maps/ThursdayDayRoutesMap.html"
          width="1200"
          height="800"
          style={{ border: "2px solid #ccc", borderRadius: "8px" }}
          title="Map"
        ></iframe>
      </div>
    </>
  );
}

export default App;
