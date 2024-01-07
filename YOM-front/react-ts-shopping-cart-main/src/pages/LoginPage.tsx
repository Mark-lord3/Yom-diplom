import React, { useState } from 'react';

import 'react-tabs/style/react-tabs.css';
import axios from 'axios'; // Import axios
import "../assets/css/style.css"
import { useTranslation } from 'react-i18next';
import { setToken } from '../utilities/TokenUtility'; // Import the token utility
import zxcvbn from 'zxcvbn';
import invalid_login from "../assets/images/login-invalid.svg"

import '../assets/css/adaptivity.css'
import { useNavigate } from 'react-router-dom';
import { useUser } from '../utilities/UserContext';
const LoginPage: React.FC = () => {
  const [loginData, setLoginData] = useState({ email: '', password: '' });
  const [registrationData, setRegistrationData] = useState({ fullName: '', login: '', email: '', password: '', confirmPassword: '' });
  const [confirmPass, setConfirmPassword] = useState('');
  const [passwordStrength, setPasswordStrength] = useState(0);
  const [passwordsMatch, setPasswordsMatch] = useState(true);
  const navigate = useNavigate();
  const { userId, setUserId, userRole, setUserRole, refreshToken, setRefreshToken } = useUser();
  const [message, setMesage] = useState('');
  const [activeTab, setActiveTab] = useState(0);
  const [showMessage, setShowMessage] = useState(true);
  const [deviceType, setDeviceType] = useState<string>('');
  const { t } = useTranslation();
  const getDeviceType = (): string => {
    const width = window.innerWidth;

    if (width <= 768) {
      return 'Mobile';
    } else if (width <= 1024) {
      return 'Tablet';
    }
    return 'Desktop';
  };
  const connectUser = async (userId: string) => {
    const deviceType = getDeviceType(); // Get the device type
    try {
      const response = await axios.post('https://localhost:7014/api/UserConnection/Connect', {
        userId: userId,
        userDevice: deviceType,
      });
      // Handle the response from the connection endpoint
      console.log('User connected', response.data);
    } catch (error) {
      // Handle any errors here
      console.error('Error connecting user', error);
    }
  };
  const handleLoginSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await axios.post('https://localhost:7014/api/Account/Login', loginData);
      // Handle successful login response here
      console.log('Login successful', response.data);

      // Assuming your backend returns a token in the response data
      if (response.data) {
        // Save the token in session storage
        setToken(response.data);
        setUserId(response.data.userId);
        sessionStorage.setItem("userId", response.data.userId);

        setUserRole(response.data.role);

        setRefreshToken(response.data.refreshToken);
        connectUser(response.data.userId);

        navigate('/myads');
      }
    } catch (error) {
      setShowMessage(true);
      setMesage(t('login_page.error'));
      console.error('Login error', error);
    }
  };

  const handleRegistrationSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (registrationData.password !== confirmPass) {
        // Passwords do not match, show an error message or handle it accordingly.
        console.error('Passwords do not match');
        setPasswordsMatch(false); // Set passwordsMatch to false
        return;
      }

      // Reset passwordsMatch in case it was previously set to false
      setPasswordsMatch(true);

      const response = await axios.post('https://localhost:7014/api/Account/register', registrationData);
      // Handle successful registration response here
      console.log('Registration successful', response.data);

      // Assuming your backend returns a token in the response data
      if (response.data) {
        setToken(response.data);
        
        setUserId(response.data.userId);
        sessionStorage.setItem("userId", response.data.userId);

        setUserRole(response.data.role);

        setRefreshToken(response.data.refreshToken);
        navigate('/myads');
      }
    } catch (error: any) {
      if (error.response) {
        // The request was made and the server responded with a status code
        console.error('Response Data:', error.response.data);
        console.error('Response Status:', error.response.status);
        console.error('Response Headers:', error.response.headers);
      } else if (error.request) {
        // The request was made but no response was received
        console.error('Request made, but no response received:', error.request);
      } else {
        // Something else happened while setting up the request
        console.error('Error setting up the request:', error.message);
      }
    }

  };

  // Handle changes in the password field to update strength
  const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newPassword = e.target.value;
    setRegistrationData({ ...registrationData, password: newPassword });
    const result = zxcvbn(newPassword);
    setPasswordStrength(result.score); // 0 to 4, 0 being weak and 4 being strong
  };

  // Handle changes in other fields of the registration form
  const handleRegistrationFormChange = (field: string, value: string) => {
    setRegistrationData({ ...registrationData, [field]: value });
  };

  const handleTabClick = (index: number) => {
    setActiveTab(index);
  };
  const handleGoogleLoginFailure = (error: any) => {
    console.error('Google login error', error);
  };

  return (



    <main id='login-page-main'>
      <div className='loginPage'>
        <div className='login-register'>

          <div>
            <div className="login-top-bar">


              <div className="login-top-tabs">
                {[t('login_page.text.title2'), t('login_page.text.title1')].map(
                  (tabName, index) => (
                    <div
                      key={index}
                      className={`login-tab ${activeTab === index ? 'active' : ''}`}
                      onClick={() => handleTabClick(index)}
                    >
                      {tabName}
                    </div>
                  )
                )}
              </div>
            </div>
            <div className='login-form-panel'>
            <div className="myads-tab-panels">
              <div
                className={`login-tab-panel ${activeTab === 0 ? 'active' : ''}`}
              >
                {/* <div className="d-flex align-items-center justify-content-center vh-100"> */}
                <h1 className='loginPagetitle'>{t('login_page.text.title3')}</h1>
                <h3 className='loginPagedescription'>{t('login_page.text.description1')}</h3>

                {showMessage && message ? (
                  <div className='denger-button-login'>
                    <div className='denger-button-login-el1'>
                      <img src={invalid_login}></img>
                      {message}
                    </div>
                    <button onClick={() => setShowMessage(false)} style={{ marginLeft: '10px' }}>
                      <svg xmlns="http://www.w3.org/2000/svg" width="16" height="15" viewBox="0 0 16 15" fill="none">
                        <path d="M1 1L14.8125 14" stroke="black" stroke-opacity="0.66" stroke-width="2" stroke-linecap="round" />
                        <path d="M1 14L14.0855 0.999999" stroke="black" stroke-opacity="0.66" stroke-width="2" stroke-linecap="round" />
                      </svg>
                    </button>
                  </div>
                ) : null}
                <form onSubmit={handleLoginSubmit}>

                  <div className="login-input-section ">
                    <div className='loginregister-input-names'>{t('login_page.text.title4')}</div>
                    <input
                      type="email"
                      placeholder='Логін'
                      className="login-register-inputs"
                      value={loginData.email}
                      onChange={(e) => setLoginData({ ...loginData, email: e.target.value })}
                      style={showMessage && message ? { border: '1px solid red' } : {}}
                    />
                  </div>
                  <div className="login-input-section ">
                    <div className='loginregister-input-names'>{t('login_page.text.title5')}</div>
                    <input
                      type="password"
                      placeholder='Пароль'
                      className="login-register-inputs"
                      value={loginData.password}
                      onChange={(e) => setLoginData({ ...loginData, password: e.target.value })}
                      style={showMessage && message ? { border: '1px solid #F26071' } : {}}
                    />
                  </div>

                  <div>
                  <div className='reg-buttons-forgot'>
                      <a className='remove-style-from-link' href="#" onClick={() => navigate('/forgot-password')}>{t('login_page.text.description2')}</a>
                    </div>
                    <button type="submit" className="reg-buttons">{t('login_page.button.button_text1')}</button>
                   
                  </div>
                </form>

              </div>
              <div
                className={`login-tab-panel ${activeTab === 1 ? 'active' : ''}`}
              >
                <h1 className='loginPagetitle'>{t('create_account_page.text.title3')}</h1>
                <h1 className='loginPagedescription'>{t('create_account_page.text.description1')}</h1>

                <form onSubmit={handleRegistrationSubmit}>
                  <div className='firstAndlastName'>
                    <div className="login-input-section ">

                      <input
                        type="text"
                        placeholder={t('create_account_page.text.title4')}
                        className="login-register-inputs"
                        value={registrationData.fullName}
                        onChange={(e) => setRegistrationData({ ...registrationData, fullName: e.target.value })}
                      />
                    </div>

                    {/* <div className="login-input-section ">
                  <label  className="form-label">Last Name</label>
                  <input
                    type="text"
                    
                    className="login-register-inputs"
                    value={registrationData.lastname}
                    onChange={(e) => setRegistrationData({ ...registrationData, lastname: e.target.value })}
                  />
                </div> */}
                  </div>
                  {/* <div className="login-input-section ">
                  <label  className="form-label">User Name</label>
                  <input
                    type="text"
                    
                    className="login-register-inputs"
                    value={registrationData.userName}
                    onChange={(e) => setRegistrationData({ ...registrationData, userName: e.target.value })}
                  />
                </div> */}

                  <div className="login-input-section ">
                    <input
                      type="email"
                      placeholder={t('create_account_page.text.title6')}
                      className="login-register-inputs"
                      value={registrationData.email}
                      onChange={(e) => setRegistrationData({ ...registrationData, email: e.target.value })}
                    />
                  </div>
                  <div className="login-input-section ">
                    <input
                      type="text"
                      placeholder={t('create_account_page.text.title5')}
                      className="login-register-inputs"
                      value={registrationData.login}
                      onChange={(e) => setRegistrationData({ ...registrationData, login: e.target.value })}
                    />
                  </div>
                  <div className="login-input-section ">
                    <input
                      type="password"
                      placeholder={t('create_account_page.text.title7')}
                      className="login-register-inputs"
                      value={registrationData.password}
                      onChange={(e) => {
                        const newPassword = e.target.value;
                        setRegistrationData({ ...registrationData, password: newPassword });
                        const result = zxcvbn(newPassword);
                        setPasswordStrength(result.score); // 0 to 4, 0 being weak and 4 being strong
                      }}
                    />
                  </div>

                  <div className="login-input-section ">
                    {/* <label className="form-label">Confirm Password</label> */}
                    <input
                      type="password"
                      placeholder={t('create_account_page.text.title8')}
                      className="login-register-inputs"
                      value={confirmPass}
                      onChange={(e) => {
                        setConfirmPassword(e.target.value)
                        //  const newPassword = e.target.value;
                        const ConPassword = e.target.value;
                        setRegistrationData({ ...registrationData, confirmPassword: ConPassword });
                      }
                      }
                    />
                  </div>

                  {!passwordsMatch && (
                    <div className="text-danger">Паролі не збігаються!</div>
                  )}

                  {/* Password Strength */}
                  {registrationData.password && (
                    <div className="password-strength">
                      Надійність паролю:
                      <div className={`strength-${passwordStrength}`}>
                        {['Very Weak', 'Weak', 'Fair', 'Strong', 'Very Strong'][passwordStrength]}
                      </div>
                    </div>
                  )}

                  <button type="submit" className="reg-buttons">{t('create_account_page.button.button_text1')}</button>
                </form>
              </div>
            </div>
            </div>
          </div>

        </div>
      </div>
    </main>


  );
};

export default LoginPage;
