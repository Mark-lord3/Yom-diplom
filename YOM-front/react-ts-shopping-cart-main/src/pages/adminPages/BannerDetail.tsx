// BannerDetail.tsx

import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom';

interface BannerDetail {
  id: number;
  companyName: string;
  description: string;
  email: string;
  phoneNumber: string;
  header: string;
  photoPaths: string;
  bannerState: string;
  linkToCompany: string;
  clicksCount: number;
  bannerPage: "Main" | "Userсare" | "Promotion";
  bannerSize: "Large" | "Medium" | "Small" | "Long";
  bannerAdvertisementPlan: "Standard" | "Premium" | "Professional";
}

interface BannerDetailUpdate {
  id: number;
  companyName: string;
  description: string;
  email: string;
  phoneNumber: string;
  bannerState: string;
  header: string;
  photo: File | null;
  linkToCompany: string;

  bannerPage: "Main" | "Userсare" | "Promotion";
  bannerSize: "Large" | "Medium" | "Small" | "Long";

  bannerAdvertisementPlan: "Standard" | "Premium" | "Professional";
}

const BannerDetail: React.FC = () => {
  const [bannerDetail, setBannerDetail] = useState<BannerDetail>();
  const { id } = useParams<{ id: string }>();
  const [bannerId, setBannerId] = useState<number>(0);

  const [formData, setFormData] = useState<BannerDetailUpdate>({
    id: bannerId,
    companyName: '',
    description: '',
    email: '',
    phoneNumber: '',
    bannerState: '',
    header: '',
    photo: null,
    linkToCompany: '',
    bannerPage: "Promotion",
    bannerSize: "Large",
    bannerAdvertisementPlan: 'Standard',
  });

  useEffect(() => {
    const fetchBannerDetail = async () => {
      try {
        const response = await axios.get(`https://localhost:7014/api/AdminBanner/DetailInfo/${id}`);
        setBannerDetail(response.data);

        setFormData(response.data);

        setBannerId(response.data.id);
      } catch (error) {
        console.error('Error fetching the banner details:', error);
      }
    };

    fetchBannerDetail();
  }, [id]);
  const [formData, setFormData] = useState<BannerDetailUpdate>({
    id: bannerId,
    companyName: "",
    description: "",
    email: "",
    phoneNumber: "",
    bannerState: "Active",
    header: "",
    photo: null, // Assuming a File object for the photo
    linkToCompany: "",
    bannerAdvertisementPlan: "Premium",
  });
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (formData) {
      setFormData({
        ...formData,
        [e.target.name]: e.target.value
      });
    }
  };


  const handlePhotoChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length > 0) {
      // If there's a file selected
      setFormData({
        ...formData,
        photo: e.target.files[0]
      });
    } else {
      // If there's no file selected or the file is deleted
      setFormData({
        ...formData,
        photo: null
      });
    }
  };

  const [showSuccess, setShowSuccess] = useState<boolean>(false);

  const handleSubmit = async () => {
    try {
      const config = {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      };
      const response = await axios.post(`https://localhost:7014/api/AdminBanner/Update/WithPhoto`, formData, config);
      if (response.status === 200) {
        console.log("Banner details updated successfully");
        setShowSuccess(true);
        setTimeout(() => {
          setShowSuccess(false);
        }, 5000);
      } else {
        console.error("Error updating the banner:", response.data);
      }
    } catch (error) {
      console.error("Error while trying to update the banner:", error);
    }
  };

  return (
    <div className='BannerDetail-container'>
      <div className="BannerDetail-card">
        <h2>Подробиці банера</h2>
        {bannerDetail ? (
          <form onSubmit={e => {
            e.preventDefault();
            handleSubmit();
          }}>
            <div className="BannerDetail-field">
              <label>Компанія:</label>
              <input id='company' type="text" name="company" value={bannerDetail.companyName} onChange={handleInputChange} />
            </div>
            <div className="BannerDetail-field">
              <img style={{
                width: bannerDetail.bannerSize === "Large" ? "480px" :
                  bannerDetail.bannerSize === "Medium" ? "580px" :
                    bannerDetail.bannerSize === "Small" ? "280px" :
                      bannerDetail.bannerSize === "Long" ? "1260px" :
                        "auto", // За замовчуванням - 'auto', якщо розмір не впізнаний
                height: bannerDetail.bannerSize === "Large" ? "500px" :
                  bannerDetail.bannerSize === "Medium" ? "340px" :
                    bannerDetail.bannerSize === "Small" ? "340px" :
                      bannerDetail.bannerSize === "Long" ? "170px" :
                        "auto", // За замовчуванням - 'auto', якщо розмір не впізнаний
              }} src={bannerDetail.photoPaths}></img>

              <div className="BannerDetail-field">
                <div className="admin-custom-file-upload">
                  <label htmlFor="photo">Фото</label>
                  <input
                    type="file" id="photo" style={{ display: "none" }}
                    onChange={handlePhotoChange} required
                  />
                  <label htmlFor="file-upload" className="admin-custom-file-upload-label">
                    Загрузити файл
                  </label>
                </div>
              </div>
            </div>
            <div className="BannerDetail-field">
              <label>Посилання на компанію:</label>
              <input type="text" name="linkToCompany" value={formData.linkToCompany} defaultValue={bannerDetail.linkToCompany} onChange={handleInputChange} />
            </div>
            <div className="BannerDetail-field">
              <label>Опис:</label>
              <input id='description' type="text" name="description" value={formData.description} defaultValue={bannerDetail.description} onChange={handleInputChange} />
            </div>
            <div className="BannerDetail-field">
              <label>Електронна адреса:</label>
              <input id='email' type="text" name="email" value={formData.email} defaultValue={bannerDetail.email} onChange={handleInputChange} />
            </div>
            <div className="BannerDetail-field">
              <label>Номер телефону</label>
              <input id='phoneNumber' type="text" name="phoneNumber" defaultValue={bannerDetail.phoneNumber} value={formData.phoneNumber} onChange={handleInputChange} />
            </div>
            <div className="BannerDetail-field">
              <label>Назва</label>
              <input id='header' type="text" name="header" value={formData.header} defaultValue={bannerDetail.header} onChange={handleInputChange} />
            </div>
            <div className="BannerDetail-field">
              <label>План рекламної кампанії:</label>
              <select id='bannerAdvertisementPlan' name="bannerAdvertisementPlan" value={formData.bannerAdvertisementPlan} defaultValue={bannerDetail.bannerAdvertisementPlan} onChange={e => handleInputChange(e as any)}>
                <option value="Standard">Стандартний</option>
                <option value="Premium">Преміум</option>
                <option value="Professional">Професійний</option>
              </select>
            </div>
            <div className="BannerDetail-field">
              <label>Сторінка рекламної кампанії:</label>
              <select id='bannerPage' name="bannerPage" value={formData.bannerPage} defaultValue={bannerDetail.bannerPage} onChange={e => handleInputChange(e as any)}>
                <option value={"Main"}>Головна</option>
                <option value={"Userсare"}>Сторінка користувача</option>
                <option value={"Promotion"}> Просування</option>
              </select>
            </div>
            <div className="BannerDetail-field">
              <label>Розмір банера:</label>
              <select id='bannerSize' name="bannerSize" value={formData.bannerSize} defaultValue={bannerDetail.bannerSize} onChange={e => handleInputChange(e as any)}>
                <option value={"Large"}>Великий</option>
                <option value={"Medium"}>Середній</option>
                <option value={"Small"}>Малий</option>
                <option value={"Long"}>Довгий</option>
              </select>
            </div>
            {showSuccess && (
              <span style={{ color: 'green' }}>Оновлено!</span>
            )}

            <button className="BannerDetail-btn" type="submit">Оновити банер</button>
          </form>
        ) : (
          <p>Загрузка...</p>
        )}
      </div>
    </div>
  );






};

export default BannerDetail;
