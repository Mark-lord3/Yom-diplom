import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const ForgotPassword: React.FC = () => {
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await axios.post('https://localhost:7014/api/Account/forgot-password', { email: email }, {
          headers: {
              'Content-Type': 'application/json'
          }
      });
      setMessage("Посилання для скидання надіслано на вашу електронну адресу. Будь ласка, перевірте свою поштову скриньку.");
      // alert("")
      // navigate('/reset-password');
    } catch (error) {
      console.error('Error sending forgot password email', error);
      setMessage("Помилка надсилання електронного листа із забутим паролем. Будь ласка спробуйте ще раз.");
    }
  };

  return (
    <main>
      <div className="forgot-password-container">
        <h2>Забув пароль</h2>
        {message && <p>{message}</p>}
        <form onSubmit={handleSubmit}>
          <div className="input-group-forgot">
            <label>Електронна адреса:</label>
            <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
          </div>
          <button type="submit" className="reset-btn-forgot">Відправити код</button>
        </form>
      </div>
    </main>
  );
};

export default ForgotPassword;