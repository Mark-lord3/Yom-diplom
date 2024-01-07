import React, { useState, useEffect } from 'react';
import axios from 'axios';
import AdminSideBar from '../../components/layout/AdminSideBar';
import { useNavigate } from 'react-router-dom';
import '../../assets/css/AdminPages/adminCategories.css'

type Category = {
  id: number;
  title: string;
  photo: File | null;
};

type SubCategory = {
  id: number;
  categoryId: number;
  title: string;
  section: string;
};

const GetCategories: React.FC = () => {
  type ModalContent = {
    type: 'Категорія' | 'Під категорія' | '';
    id: number | null;
    data?: Category | SubCategory;
  };

  const [modalContent, setModalContent] = useState<ModalContent>({ type: '', id: null });

  const [categories, setCategories] = useState<Category[]>([]);
  const [subcategories, setSubcategories] = useState<SubCategory[]>([]);

  const [isModalOpen, setIsModalOpen] = useState(false);

  const [categoryPage, setCategoryPage] = useState<number>();
  const [subCategoryPage, setSubCategoryPage] = useState<number>();
  const [totalCategoryPages, setCategoryPages] = useState(1);
  const [totalSubCategoryPages, setSubCategoryPages] = useState(1);
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  const navigate = useNavigate();
  useEffect(() => {
    // Fetch data from the Category API endpoint
    axios.get('https://localhost:7014/api/Admin/Category/All?pageNumber=1')
      .then(response => {
        setCategories(response.data.categories);
        setCategoryPages(response.data.totalPages);
        console.log('====================================');
        console.log(response.data);
        console.log('====================================');
      })
      .catch(error => {
        console.error('Error fetching categories:', error);
      });
    setCategoryPage(1);
    // Fetch data from the SubCategory API endpoint
    axios.get('https://localhost:7014/api/Admin/SubCategory/All?pageNumber=1')
      .then(response => {
        setSubcategories(response.data.subCategories);
        setSubCategoryPages(response.data.totalPages);
        setSubCategoryPage(1);
      })
      .catch(error => {
        console.error('Error fetching subcategories:', error);
      });

  }, []);

  const getCategories = async (page: number) => {
    axios.get(`https://localhost:7014/api/Admin/Category/All?pageNumber=${page}`)
      .then(response => {
        setCategories(response.data.categories);
        console.log('====================================');
        console.log(response.data);
        console.log('====================================');
      })
      .catch(error => {
        console.error('Error fetching categories:', error);
      });
  }

  const getSubCategories = async (page: number) => {
    axios.get(`https://localhost:7014/api/Admin/Category/All?pageNumber=${page}`)
      .then(response => {
        setCategories(response.data);
        console.log('====================================');
        console.log(response.data);
        console.log('====================================');
      })
      .catch(error => {
        console.error('Error fetching categories:', error);
      });
  }

  const handleDeleteCategory = (categoryId: number) => {
    axios.delete(`https://localhost:7014/api/Admin/Category/Delete/${categoryId}`)
      .then(() => {
        console.log('Category deleted successfully.');
        setCategories(prevCategories => prevCategories.filter(cat => cat.id !== categoryId));
      })
      .catch(error => {
        console.error('Error deleting category:', error);
      });
  };
  const handleDeleteSubCategory = (subCategoryId: number) => {
    axios.delete(`https://localhost:7014/api/Admin/SubCategory/Delete`, {
      params: {
        subCategoryId: subCategoryId,
      }
    })
      .then(() => {
        console.log('Subcategory deleted successfully.');
        setSubcategories(prevSubcategories => prevSubcategories.filter(subcat => subcat.id !== subCategoryId));
      })
      .catch(error => {
        console.error('Error deleting subcategory:', error);
      });
  };
  const handlePopUp = (type: 'Категорія' | 'Під категорія', id: number, data: Category | SubCategory) => {
    setModalContent({ type, id, data });
    setIsModalOpen(true);
  };


  const handleUpdateForm = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const target = e.target as typeof e.target & {
      id: { value: string };
      title: { value: string };
      categoryId: { value: string };
      section: { value: string };
      photo: { value: null };
    };
    const categoryFormData = {
      id: parseInt(target.id.value, 10),
      title: target.title.value,
      photo: selectedFile
    };
    const updatedData = {
      id: target.id.value, // Assuming 'id' is a string. If it's a number, you may parse it as such.
      title: target.title.value,
      // Only add categoryId and section if it's a subcategory
      ...(modalContent.type === 'Під категорія' && {
        categoryId: Number(target.categoryId.value),
        section: target.section.value,
      }),
      ...(modalContent.type === 'Категорія' && {
        photo: target.photo.value as File | null
      }),
    };

    const url =
      modalContent.type === 'Категорія'
        ? 'https://localhost:7014/api/Admin/Category/Update'
        : 'https://localhost:7014/api/Admin/SubCategory/Update';

    try {
      if (modalContent.type === 'Категорія') {
        const config = {
          headers: {
            'Content-Type': 'multipart/form-data',
          },
        };
        console.log(categoryFormData);

        const response = await axios.put(url, categoryFormData, config);
        if (response.status === 200) {
          // Handle the updated category or subcategory data e.g., refresh the data
          closeModal();

          // You might want to update the state or re-fetch the categories/subcategories here
        } else {
          console.error("Error updating data");
        }
      }
      console.log('====================================');
      console.log(updatedData);
      console.log('====================================');
      const response = await axios.put(url, updatedData, {
        headers: {
          'Content-Type': 'application/json',
        },
      });
      if (response.status === 200) {
        // Handle the updated category or subcategory data e.g., refresh the data
        closeModal();

        // You might want to update the state or re-fetch the categories/subcategories here
      } else {
        console.error("Error updating data");
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setModalContent({ type: '', id: null });
  };

  const handleCategoryClickPage = async (pg: number) => {
    console.log(pg);
    if (subCategoryPage) {
      setCategoryPage(pg);
      getCategories(pg);
    }
  }
  const handleSubClickPage = async (pg: number) => {
    console.log(pg);
    if (subCategoryPage) {
      setSubCategoryPage(pg);
      getSubCategories(pg);
    }
  }
  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files.length > 0) {
      const selectedFile = e.target.files[0];
      setSelectedFile(selectedFile);
    }
  };
  return (
    <div className='admin-flex'>
      <AdminSideBar />
      <div className='admin-Allcategory'>

        {

          isModalOpen && (
            <div className='admin-category-update' >
              <h2>Оновити {modalContent.type}</h2>
              <form className='admin-category-update-form' onSubmit={handleUpdateForm}>
                <input type="hidden" name="id" value={modalContent.id || ''} />

                <div className='admin-category-update-value'>
                  <label>Назва:</label>
                  <input type="text" name="title" defaultValue={modalContent.data?.title || ''} required />
                </div>
                {modalContent.type === 'Категорія' && (
                  // <div className='admin-category-update-value'>
                  //   <div className="admin-custom-file-upload">
                  //     <label htmlFor="photo">Фото</label>
                  //     <input
                  //       type="file" id="photo" style={{ display: "none" }}
                  //       onChange={handleFileChange} required
                  //     />
                  //     <label htmlFor="file-upload" className="admin-custom-file-upload-label">
                  //       Загрузити файл
                  //     </label>
                  //   </div>
                  // </div>
                  <div className="custom-file-upload">
                    
                    <input
                      type="file"
                      id="photo"
                      onChange={handleFileChange} required
                      style={{ display: "none" }}
                    />
                    <label htmlFor="photo" className="custom-file-upload-label">
                    Загрузити файл
                    </label>
                  </div>
                )}
                {modalContent.type === 'Під категорія' && (
                  <>
                    <div className='admin-category-update-value'>
                      <label>Номер категорії:</label>
                      <input type="number" name="categoryId" defaultValue={modalContent.data && 'categoryId' in modalContent.data ? modalContent.data.categoryId : ''} required />
                    </div>
                    <div className='admin-category-update-value'>
                      <label>Секція:</label>
                      <input type="text" name="section" defaultValue={(modalContent.data as SubCategory)?.section || ''} required />
                    </div>
                  </>
                )}
                <div className='admin-category-buttons'>
                  <button className='update' type="submit">Оновити</button>
                  <button className='delete' onClick={closeModal}>Відмінити</button>
                </div>
              </form>
            </div>
          )
        }
        <div className='admin-category'>
          <h2>Категорії</h2>
          <table className="admin-category-table">
            <thead>
              <tr>
                <th>Номер</th>
                <th>Назва</th>
                <th>Дії</th>
              </tr>
            </thead>
            <tbody>
              {categories.map(category => (
                <tr key={category.id}>
                  <td style={{ color: "#1684EA" }}>{category.id}</td>
                  <td>{category.title}</td>
                  <td className='admin-category-buttons'>
                    <button className='update' onClick={() => handlePopUp('Категорія', category.id, category)}>Оновити</button>
                    <button className='delete' onClick={() => handleDeleteCategory(category.id)}>Видалити</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <div className='admin-pagination'>
            <button
              onClick={() => handleCategoryClickPage(categoryPage !== undefined ? categoryPage - 1 : 1)}
              disabled={categoryPage === 1}
            >
              Попередня
            </button>
            <span>
              Сторінка {categoryPage} з {totalCategoryPages}
            </span>
            <button
              onClick={() => handleCategoryClickPage(categoryPage !== undefined ? categoryPage + 1 : 1)}
              disabled={categoryPage === totalCategoryPages}
            >
              Наступна
            </button>
          </div>
        </div>
        <div className='admin-category'>
          <h2>Під категорії</h2>
          <table className="admin-category-table">
            <thead>
              <tr>
                <th>Номер</th>
                <th>Номер категорії</th>
                <th>Назва</th>
                <th>Секція</th>
                <th>Дії</th>
              </tr>
            </thead>
            <tbody>
              {subcategories.map(subcategory => (
                <tr key={subcategory.id}>
                  <td style={{ color: "#1684EA" }} >{subcategory.id}</td>
                  <td>{subcategory.categoryId}</td>
                  <td>{subcategory.title}</td>
                  <td>{subcategory.section}</td>
                  <td className='admin-category-buttons'>
                    <button className='update' onClick={() => handlePopUp('Під категорія', subcategory.id, subcategory)}>Оновити</button>

                    <button className='delete' onClick={() => handleDeleteSubCategory(subcategory.id)}>Видалити</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <div className='admin-pagination'>
            <button
              onClick={() => handleSubClickPage(subCategoryPage !== undefined ? subCategoryPage - 1 : 1)}
              disabled={subCategoryPage === 1}
            >
              Попередня
            </button>
            <span>
              Сторінка {subCategoryPage} з {totalSubCategoryPages}
            </span>
            <button
              onClick={() => handleSubClickPage(subCategoryPage !== undefined ? subCategoryPage + 1 : 1)}
              disabled={subCategoryPage === totalSubCategoryPages}
            >
              Наступна
            </button>
          </div>
        </div>
      </div>
    </div>

  );

}

export default GetCategories;