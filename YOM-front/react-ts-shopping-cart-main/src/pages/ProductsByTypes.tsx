

import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import "../assets/css/style.css"
import data from '../utilities/UkrainianCity.json';
import Breadcrumbs from '../components/layout/Breadcrumbs';
import banner1 from '../assets/images/Rectangle 152565670.png'
import banner3 from '../assets/images/Rectangle 152565670 (1).png'
import banner2 from '../assets/images/banners/House-Banner.jpg'
import banner4 from '../assets/images/banners/OIP (3).jpg'
import banner5 from '../assets/images/Rectangle 152565675.png'
import banner1_bottom from '../assets/images/Rectangle 152565675.png'
import banner3_bottom from '../assets/images/banners/Group 1000004188.png'
import banner2_bottom from '../assets/images/banners/OIP (2).jpg'
import banner4_bottom from '../assets/images/banners/banner_1e42468b-3cb1-49b9-91ff-d873d662d108_1600x.webp'
import banner5_bottom from '../assets/images/Rectangle 152565675.png'
import next from '../assets/images/pagination_next.svg'
interface BreadcrumbsProps {
  category: string;
  subcategory: string;
}
interface AdData {
  id: number;
  title: string;
  description: string;
  price: number;
  dateCreated: string;
  dateModified: string;
  city: string;
  address: string;
  currency: string;
  adType: string;
  categoryId: number;
  subCategoryId: number;
  photos: string;
  userId: number;
  category: string;
}

interface ProductsByTypesProps { }

interface ProductsByTypesState {
  ads: AdData[];
  isLoading: boolean;
  location: string;
  minPrice: number;
  maxPrice: number;
  adType: string;
  condition: string;
  purchaseConditions: string[];
  pageNumber: number;
  totalPages: number;
  prevSearch: string; // Add this line
  category: string;
  sortField: string;
  catId: number;
  subcatId: number;
  numberOfElements: number;
}

export class ProductsByTypes extends Component<
  ProductsByTypesProps,
  ProductsByTypesState
> {
  constructor(props: ProductsByTypesProps) {
    super(props);
    this.state = {
      ads: [],
      isLoading: true,
      location: '',
      minPrice: 0,
      maxPrice: 0,
      adType: '',
      condition: '',
      purchaseConditions: [],
      pageNumber: 1,
      totalPages: 1,
      prevSearch: '',
      category: '',
      sortField: '',
      catId: 0,
      subcatId: 0,
      numberOfElements: 0,
    };

  }


  cities = data[0].regions[0].cities.map(city => city.name);

  // This handler is for <select> elements
  handleLocationChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    this.setState({ location: e.target.value }, () => {

    });
  };

  handleMinPriceChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    this.setState({ minPrice: Number(e.target.value) }, () => {

    });

  };

  handleMaxPriceChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    this.setState({ maxPrice: Number(e.target.value) }, () => {
      console.log(this.state.maxPrice);
    });

  };

  handleSortChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    this.setState({ sortField: e.target.value }, () => {

    });
  };

  handleConditionChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    this.setState({ condition: e.target.value }, () => {

    });
  };

  handlePurchaseConditionsChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    this.setState({ adType: e.target.value }, () => {

    });
  };
  componentDidMount() {
    this.applyFilters();
  }

  componentDidUpdate(prevProps: any, prevState: ProductsByTypesState) {
    const hasSearchChanged = this.state.prevSearch !== window.location.search;
    const hasFilterChanged = 
      prevState.location !== this.state.location ||
      prevState.sortField !== this.state.sortField ||
      prevState.minPrice !== this.state.minPrice ||
      prevState.maxPrice !== this.state.maxPrice ||
      prevState.condition !== this.state.condition ||
      prevState.adType !== this.state.adType;
  
    if (hasSearchChanged || hasFilterChanged) {
      this.applyFilters();
    }
  }
  applyFilters = async () => {

    const location = this.state.location;
    const minPrice = this.state.minPrice;
    const maxPrice = this.state.maxPrice;
    const condition = this.state.condition;
    const adtype = this.state.adType;
    const pageNumber = this.state.pageNumber;
    console.log('====================================');
    console.log(location);
    console.log('====================================');

    const params = new URLSearchParams(window.location.search);
    const categoryId = params.get('categoryId');
    this.setState({ catId: Number(categoryId) });
    const categoryId2 = this.state.catId;
    const location_from_url = params.get('location');
    console.log('====================================');
    console.log(location_from_url);
    console.log('====================================');

    const subCategoryId = params.get('subCategoryId');

    this.setState({ subcatId: Number(subCategoryId) });
    const subCategoryId2 = this.state.subcatId;
    // const pageNumber = params.get('PageNumber') || '1'; // Default to page 1 if not specified

    let endpoint = `https://localhost:7014/api/Ad/AllAd/ByQuery?`;

    const sortField = this.state.sortField;

    let sortDir = 'asc';  // You can later add functionality to toggle this between 'asc' and 'desc' if needed

    let i = 0;
    //https://localhost:7014/api/Ad/AllAd/ByQuery?Sort.Dir=asc&Sort.Field=Price&Filter.Filters[0].Field=CategoryId&Filter.Filters[0].Operator=eq&Filter.Filters[0].Value=1


    if (categoryId || categoryId2) {

      endpoint += `Filter.Filters[${i}].Field=CategoryId&Filter.Filters[${i}].Operator=eq&Filter.Filters[${i}].Value=${categoryId}`;

      i++;
    }
    if (subCategoryId || subCategoryId2) {
      endpoint += `&Filter.Filters[${i}].Field=SubCategoryId&Filter.Filters[${i}].Operator=eq&Filter.Filters[${i}].Value=${subCategoryId}`;
      i++;
    }

    // location filter
    if (location_from_url) {
      endpoint += `&Filter.Filters[${i}].Field=city&Filter.Filters[${i}].Operator=eq&Filter.Filters[${i}].Value=${location_from_url}`;

      i++;
    }
    if (location) {
      endpoint += `&Filter.Filters[${i}].Field=city&Filter.Filters[${i}].Operator=eq&Filter.Filters[${i}].Value=${location}`;

      i++;
    }
    if (sortField) {
      if (sortField === "price") {
        endpoint += `&Sort.Dir=${sortDir}&Sort.Field=Price`;
      } else if (sortField === "date") {
        // You need to provide the appropriate field name from your API for date
        endpoint += `&Sort.Dir=${sortDir}&Sort.Field=DateCreated`;
      } else if (sortField === "popularity") {
        // You need to provide the appropriate field name from your API for popularity
        endpoint += `&Sort.Dir=${sortDir}&Sort.Field=AdPopularity`;
      }
    }

    // price range filters
    if (minPrice != 0) {

      endpoint += `&Filter.Filters[${i}].Field=Price&Filter.Filters[${i}].Operator=gte&Filter.Filters[${i}].Value=${minPrice}`;
      i++;
    }
    if (maxPrice) {

      endpoint += `&Filter.Filters[${i}].Field=Price&Filter.Filters[${i}].Operator=lte&Filter.Filters[${i}].Value=${maxPrice}`;
      i++;
    }

    // condition filter
    if (condition) {

      endpoint += `&Filter.Filters[${i}].Field=ProductState&Filter.Filters[${i}].Operator=eq&Filter.Filters[${i}].Value=${condition}`;
      i++;
    }

    // purchase conditions filter
    if (adtype) {
      endpoint += `&Filter.Filters[${i}].Field=AdType&Filter.Filters[${i}].Operator=eq&Filter.Filters[${i}].Value=${adtype}`;
      i++;
    }






    endpoint += `&Filter.Filters[${i}].Field=AdState&Filter.Filters[${i}].Operator=eq&Filter.Filters[${i}].Value=0&pageSize=5&PageNumber=${pageNumber}`;
    try {
      const response = await axios.get(endpoint);


      const data: AdData[] = response.data;
      this.setState({ numberOfElements: data.length });


      this.setState({ ads: data, isLoading: false });

      this.setState({ subcatId: 0 });
      this.setState({ prevSearch: window.location.search });
      console.log('====================================');
      console.log(this.state.prevSearch);
      console.log('====================================');
      // After successfully fetching ads, log the popularity of the category
      if (categoryId) {
        this.takeCategoryName(categoryId);
        await this.logCategoryPopularity(categoryId);
      }

    } catch (error) {
      console.error('Failed to fetch data:', error);
      this.setState({ isLoading: false });
    }

  };

  setPage = (pageNum: number) => {
    this.setState({ pageNumber: pageNum }, () => {
      this.applyFilters(); // Refetch the data with the new page number
    });
  };


  takeCategoryName = (categoryId: string) => {
    axios.get(`https://localhost:7014/api/Admin/Category/ById/${categoryId}`)
      .then(response => {
        // Setting the state with the category title

        this.setState({ category: response.data.title });
      })
      .catch(error => console.error('Error fetching categories:', error));
  }


  async logCategoryPopularity(categoryId: string) {
    if(this.state.prevSearch !== window.location.search)
    try {
      const response = await axios.post(`https://localhost:7014/api/Category/AddClicks?сategoryId=${categoryId}`)
      console.log('====================================');
      console.log(response.data);
      console.log('====================================');
      if (response.status !== 200) {
        throw new Error('Failed to log category popularity');
      }
    } catch (error) {
      console.error(error);
    }
  }
  handlePurchaseConditionsToggle = (value: string) => {
    const { purchaseConditions } = this.state;
    const index = purchaseConditions.indexOf(value);

    if (index > -1) {
      purchaseConditions.splice(index, 1);
    } else {
      purchaseConditions.push(value);
    }

    this.setState({ purchaseConditions });
  };

  render() {
    const { ads, isLoading, pageNumber, totalPages, minPrice, maxPrice, location, condition, adType, purchaseConditions, category, numberOfElements } = this.state;

    if (isLoading) {
      return <div>Loading...</div>;
    }
    const params = new URLSearchParams(window.location.search);
    const categoryId = params.get('categoryId');
    const subCategoryId = params.get('subCategoryId');
    return (
      <main>


        <div className='productsByTypesPage'>
          {/* <Breadcrumbs category={categoryId} subcategory={subCategoryId}/> */}
          <section>
            <div>
              <h1>{category}</h1>
            </div>

            <div className='productsByTypesPage-banner'>

              {category == 'Одежа' ? (
                <img src={banner1} alt="Banner 1" />
              ) : category == 'Домівка' ? (
                <img src={banner2} alt="Banner 2" />
                ) : (category == 'Eлектроніка' ||category == 'Телефон') ? (
                  <img src={banner3} alt="Banner 3" />
                ): category == 'Авто' ? (
                  <img src={banner4} alt="Banner 4" />
                ) : <img src={banner1} alt="Banner 1" />}
            </div>
            <div className='sort-elment-pagination'>
              <div className='sort-elment'>
                <select id="sort" onChange={this.handleSortChange}>
                  <option value="">Оберіть Сортування </option>
                  <option value="price">Сортування за : Вартість</option>
                  <option value="date">Сортування за : Датою</option>
                  <option value="popularity">Сортування за : Популярністю</option>
                </select>
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
                    onClick={() => this.setPage(page)}
                    className={pageNumber === page ? 'active-page' : ''}
                  >
                    {page}
                  </button>
                ))}

                <button
                  onClick={() => this.setPage(pageNumber + 1)}
                  disabled={numberOfElements < 5}
                  className='pagination-next-btn'
                >
                  <img src={next}></img>
                </button>
              </div>
            </div>
            <div className='productsByTypesPage-content'>


              <div className='productsByTypes-form'>



                <button className='productsByTypes-subscribe'>💙 Підписатися на запит</button>

                <form>
                  <div className='productsByTypes-location'>
                    Місцезнаходження:
                  </div>

                  <select
                    id="citySelect"
                    name='location'
                    value={this.state.location}
                    onChange={this.handleLocationChange}
                    className='productsByTypes-location-selection'
                  >
                    {this.cities.map((city, index) => (
                      <option key={index} value={city}>{city}</option>  // Set value to current city
                    ))}
                  </select>


                  <div className='productsByTypes-price'>
                    Ціна, грн:
                  </div>
                  <div className='productsByTypes-price-inputs'>
                    <input type='number' value={minPrice ? minPrice.toString() : ''} placeholder='125' onChange={this.handleMinPriceChange} className='productsByTypes-price-input' />
                    <input type='number' value={maxPrice} placeholder='20 000' onChange={this.handleMaxPriceChange} className='productsByTypes-price-input' />
                  </div>

                  <div className='productsByTypes-state'>
                    Стан:
                  </div>
                  <div className='productsByTypes-state-inputs'>
                    {/* <input className='productsByTypes-state-input' type="radio" value="New" checked={condition === "New"} onChange={this.handleConditionChange} /> Новий
                          <input className='productsByTypes-state-input' type="radio" value="Used" checked={condition === "Used"} onChange={this.handleConditionChange} /> Вживаний
                          <input className='productsByTypes-state-input' type="radio" value="AlmostNew" checked={condition === "AlmostNew"} onChange={this.handleConditionChange} /> Відмінний
                          <input className='productsByTypes-state-input' type="radio" value="Bad" checked={condition === "Bad"} onChange={this.handleConditionChange} /> Bad
                           */}
                    <input className='productsByTypes-state-input' type="radio" value="0" checked={condition === "0"} onChange={this.handleConditionChange} /> Новий
                    <input className='productsByTypes-state-input' type="radio" value="1" checked={condition === "1"} onChange={this.handleConditionChange} /> Вживаний
                    <input className='productsByTypes-state-input' type="radio" value="2" checked={condition === "2"} onChange={this.handleConditionChange} /> Відмінний
                    <input className='productsByTypes-state-input' type="radio" value="3" checked={condition === "3"} onChange={this.handleConditionChange} /> Bad
                  </div>


                  <div className='productsByTypes-condition'>
                    Умови покупки:
                  </div>
                  <div className='productsByTypes-condition-inputs'>
                    <input className='productsByTypes-condition-input' type="radio" value="0" checked={adType === "0"} onChange={this.handlePurchaseConditionsChange} /> Безкоштовно
                    <input className='productsByTypes-condition-input' type="radio" value="1" checked={adType === "1"} onChange={this.handlePurchaseConditionsChange} /> Торг
                    <input className='productsByTypes-condition-input' type="radio" value="2" checked={adType === "2"} onChange={this.handlePurchaseConditionsChange} /> Обмін
                  </div>


                </form>
              </div>
              <div className='productsByTypes-ads-section'>
                {/* Ad listings */}
                {ads.map((ad) => (
                  <div key={ad.id}>
                    <div className='productsByTypes-ads-blocks'>

                    {ad?.photos && ad.photos.length > 0 && (
                        <div className='productsByTypes-ads-photo'>
                          <img src={ad.photos[0]} alt="First Image" />
                        </div>
                      )}
                     

                      <div className='productsByTypes-ads-description'>

                        <Link className='remove-style-from-link' to={`/products/bycategory/product/${ad.id}`} key={ad.id}>

                          <h3 className='productsByTypes-ads-description-title'>{ad.title}</h3>
                        </Link>
                        <p className='productsByTypes-ads-description-price'>{ad.price}</p>


                      </div>
                    </div>

                    {/* <Link to={`/products/byUser/${ad.userId}`} key={ad.userId}>
                        <div>
                            <h3>Go to products by user</h3>
                            
                            
                        </div>
                      </Link> */}
                  </div>

                ))}

                {/* <div className='productsByTypes-ads-blocks'>
                      <div className='productsByTypes-ads-photo'>

                      </div>
                      <div className='productsByTypes-ads-description'>

                      </div>
                    </div>
                    <div className='productsByTypes-ads-blocks'>
                      <div className='productsByTypes-ads-photo'>

                      </div>
                      <div className='productsByTypes-ads-description'>
                      <Link className='remove-style-from-link' to={`/products/bycategory/product/1`} >
                            
                            <h3 className='productsByTypes-ads-description-title'>akenfalkej,fmsnef</h3>
                            
                            <p className='productsByTypes-ads-description-price'>price</p>
                            </Link>
                      </div>
                    </div>
                    <div className='productsByTypes-ads-blocks'>
                      <div className='productsByTypes-ads-photo'>

                      </div>
                      <div className='productsByTypes-ads-description'>

                      </div>
                    </div>
                    <div className='productsByTypes-ads-blocks'>
                      <div className='productsByTypes-ads-photo'>

                      </div>
                      <div className='productsByTypes-ads-description'>

                      </div>
                    </div>
                    <div className='productsByTypes-ads-blocks'>
                      <div className='productsByTypes-ads-photo'>

                      </div>
                      <div className='productsByTypes-ads-description'>

                      </div>
                    </div>
                    <div className='productsByTypes-ads-blocks'>
                      <div className='productsByTypes-ads-photo'>

                      </div>
                      <div className='productsByTypes-ads-description'>

                      </div>
                    </div>
                    <div className='productsByTypes-ads-blocks'>
                      <div className='productsByTypes-ads-photo'>

                      </div>
                      <div className='productsByTypes-ads-description'>

                      </div>
                    </div> */}

              </div>

            </div>
            <div className='productsByTypesPage-bottombanner-section'>
              <div className='productsByTypesPage-bottombanner'>
                {category == 'Одежа' ? (
                  <img src={banner1_bottom} alt="Banner 1" />
                ) : category == 'Домівка' ? (
                  <img src={banner2_bottom} alt="Banner 2" />
                ) : (category == 'Eлектроніка' ||category == 'Телефон')  ? (
                  <img src={banner3_bottom} alt="Banner 3" />
                ): category == 'Авто' ? (
                  <img src={banner4_bottom} alt="Banner 4" />
                ) : category == 'Їжа для тварин'  ? (
                  <img src={banner5_bottom} alt="Banner 5" />
                ) : <img src={banner1_bottom} alt="Banner 1" />}
              </div>
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
                  onClick={() => this.setPage(page)}
                  className={pageNumber === page ? 'active-page' : ''}
                >
                  {page}
                </button>
              ))}

              <button
                onClick={() => this.setPage(pageNumber + 1)}
                disabled={numberOfElements < 5}
                className='pagination-next-btn'
              >
                <img src={next}></img>
              </button>
            </div>

          </section>
        </div>
      </main>
    );
  }
}


export default ProductsByTypes;