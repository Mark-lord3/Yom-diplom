import React, { useState } from 'react';
import axios from 'axios';
import { Navigate, useParams ,useNavigate } from 'react-router-dom';

interface ReportUserProps {}

const ReportUser: React.FC<ReportUserProps> = () => {
 
  const navigate=useNavigate();
  const params = new URLSearchParams(window.location.search);
  const userId= params.get('userId');
    

  const [description, setDescription] = useState<string>('');


  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();


    try {
      const response = await axios.post('https://localhost:7014/api/HelpReport/Create', {
        userId: userId,
        description: description,
      });
      

      if (response.status === 200) {
        console.log('Report submitted successfully.');
        navigate('/')
        // Handle any other logic after successful submission, e.g. showing a success message.
      } else {
        console.error('Failed to submit report.');
      }
    } catch (error) {
      console.error('Error submitting report:', error);
    }
  };

  return (
    <main>
    <div className="report-form-container">
      <form onSubmit={handleSubmit} className="report-form">
        <div className="input-container">
          <label htmlFor="description" className="description-label">Опис:</label>
          <textarea 
            id="description" 
            className="description-textarea"
            value={description} 
            onChange={e => setDescription(e.target.value)} 
            required 
          />
        </div>
        <button className="submit-button" type="submit">Надіслати звіт</button>
      </form>
    </div>
    </main>
  );
};

export default ReportUser;
