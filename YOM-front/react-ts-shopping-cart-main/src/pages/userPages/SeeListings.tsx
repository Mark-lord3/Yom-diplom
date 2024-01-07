import React, { useEffect, useState } from 'react';
import CustomNavbar from '../../components/layout/CustomNavbar';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { getToken } from '../../utilities/TokenUtility';
import { useUser } from '../../utilities/UserContext';
import { Link } from 'react-router-dom';
import next from '../../assets/images/pagination_next.svg'
const SeeListings: React.FC = () => {
  const navigate = useNavigate();
  const { userId } = useUser();
  
  const [lastViewedAds, setLastViewedAds] = useState<any[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [pageNumber, setpageNumber] = useState<number>(1); // Initialize products state as an empty array
  const [numberOfElements, setnumberOfElements] = useState<number>(4);

  useEffect(() => {
    const checkAuthenticationStatus = () => {
      const isAuthenticated = !!getToken();
      if (!isAuthenticated) {
        navigate('/login');
      }
    };
    const filterUniqueAds = (ads: any[]) => {
      const uniqueAdIds = new Set();
      return ads.filter(ad => {
        if (!uniqueAdIds.has(ad.adId)) {
          uniqueAdIds.add(ad.adId);
          return true;
        }
        return false;
      });
    };
    const sortByNewest = (ads: any[]) => {
      return ads.sort((a, b) => new Date(b.lastSeenAt).getTime() - new Date(a.lastSeenAt).getTime());
    };
    const fetchLastViewedAds = async (page: number) => {
      setIsLoading(true);
      try {
        const response = await axios.get(`https://localhost:7014/api/LastViewed?userId=${userId}&PageNumber=${pageNumber}&PageSize=6`);
        const data = response.data;
        setnumberOfElements(data.length);
        const uniqueAds = filterUniqueAds(data);
        setLastViewedAds(sortByNewest(uniqueAds));
      } catch (error) {
        console.error("Error fetching LastViewed ads:", error);
      } finally {
        setIsLoading(false);
      }
    };

    checkAuthenticationStatus();
    fetchLastViewedAds(pageNumber);
  }, [navigate, userId, pageNumber]);

  const renderAds = () => lastViewedAds.map((ad: any) => (
    <div key={ad.id} className='lastseen-product-view'>
      <h3>{ad.title}</h3>
      <Link to={`/products/bycategory/product/${ad.id}`}>Click</Link>
      
    </div>
  ));

  
  return (
    <main>
      <div className='LastSeenPage'>
        <div className='myads-menu'>
          <CustomNavbar />
        </div>
        <section>
          <div className='lastseen-title'>
            <p>Переглянуті оголошення</p>
          </div>
          <div className='lastseen-grid'>
            {isLoading ? (
              <p>Loading...</p>
            ) : (
              // renderAds()
              lastViewedAds.map(ad => (
                <div key={ad.adId} className='lastseen-product-view'>
                  <div>
                    <div className='lastseen-product-view-image'>
                      <img src={ad.photoPaths}></img>
                    </div>
                    <div className='lastseen-product-description'>
                    <Link className='remove-style-from-link lastseen-product-title' to={`/products/bycategory/product/${ad.adId}`}>{ad.title}</Link>
                    </div>
                    <div className='lastseen-product-price'>{ad.price} грн</div>
                  </div>
                </div>
              ))
              
            )}
          </div>
        </section>
      </div>
      <div className='pagination'>
        {/* <button
                          onClick={() => this.setPage(pageNumber - 1)}
                          disabled={pageNumber === 1}
                      >
                          Previous
                      </button> */}

        {pageNumber > 3 && <span>...</span>}

        {Array.from({ length: Math.min(3, pageNumber) }, (_, index) => pageNumber - index).reverse().map(page => (
          <button
            key={page}
            onClick={() => setpageNumber(page)}
            className={pageNumber === page ? 'active-page' : ''}
          >
            {page}
          </button>
        ))}

        <button
          onClick={() => setpageNumber(pageNumber + 1)}
          disabled={numberOfElements < 5}
          className='pagination-next-btn'
        >
          <img src={next}></img>
        </button>
      </div>
    </main>
  );
}

export default SeeListings;
