import React, { useState, useEffect, useRef } from 'react';
import Chart from 'chart.js/auto';
import '../../assets/css/AdminPages/adminDashboard.css';
import AdminSideBar from '../../components/layout/AdminSideBar';
import axios from 'axios';
import AdminPieChart from './components/PieChart';
import BarChart from './components/BarChart';
import LineChart from './components/LineChart';


interface SessionChart {
  count: number;
  dateLogins: string[];
}

const Dashboard: React.FC = () => {
  const chartRef = useRef<HTMLCanvasElement | null>(null);
  const [userCount, setUserCount] = useState<number>(0);
  const [blockedUserCount, setBlockedUserCount] = useState<number>(0);
  const [profitMonth, setProfitMonth] = useState<number | null>(null);
  const [profitAllTime, setProfitAllTime] = useState<number | null>(null);
  const [profitBanner, setProfitBanner] = useState<number>(0);
  const [profitPromotion, setProfitPromotion] = useState<number | null>(null);
  const [profitCountOfPurchases, setCountOfPurchases] = useState<number | null>(null);
  const [sessionDetail, setSessionDetal] = useState<number[]>([]);


  const initialSessionChart: SessionChart = {
    count: 0,
    dateLogins: [],
  }
  const [sessionChart, setSessionChart] = useState<SessionChart>(initialSessionChart);
  const fetchSessionDetail = async () => {
    try {
      const response = await axios.get('https://localhost:7014/api/AdminUserConnection/SessionDetail');
      var data = Object.values(response.data) as number[]
      setSessionDetal(data);
    } catch (error) {
      console.error('Error fetching the banners:', error);
    }
  };

  const fetchSessionChart = async () => {
    try {
      const response = await axios.get('https://localhost:7014/api/AdminUserConnection/SessionChart');
      var data = response.data as SessionChart
      setSessionChart(data);
      console.log(data);
    } catch (error) {
      console.error('Error fetching the banners:', error);
    }
  };

  const [monthRegistration, setMonthRegistration] = useState<number>();
  const fetchmonthRegistration = async () => {
    try {
      const response = await axios.get('https://localhost:7014/api/AdminUserConnection/MonthRegistration');
      var data = response.data as number
      setMonthRegistration(data);
      console.log(data);
    } catch (error) {
      console.error('Error fetching the banners:', error);
    }
  };

  const [averageSessionTime, setAverageSessionTime] = useState<string>();
  const fetchAvgSessionTime = async () => {
    try {
      const response = await axios.get('https://localhost:7014/api/AdminUserConnection/AverageSessionTime');
      var data = response.data as string
      const durationParts = data.split(':');
      const hours = parseInt(durationParts[0], 10);
      const minutes = parseInt(durationParts[1], 10);
      const seconds = parseInt(durationParts[2], 10);

      const totalMinutes = hours * 60 + minutes;
      const totalHours = Math.floor(totalMinutes / 60);
      const remainingMinutes = totalMinutes % 60;

      const convertedString = `${totalHours} годин ${remainingMinutes} хвилин ${seconds} секунд`;
      setAverageSessionTime(convertedString);
      console.log(data);
    } catch (error) {
      console.error('Error fetching the banners:', error);
    }
  };
  const percentageBlockedUsers = (blockedUserCount) / (blockedUserCount + userCount) * 100;
  const percentageUsers = (userCount) / (blockedUserCount + userCount) * 100;
  useEffect(() => {
    // Simulating the fetch of data
    // setUserCount(dummyData.userCount);
    // setBlockedUserCount(dummyData.blockedUserCount);

    // Commented axios calls
    fetchSessionDetail();
    fetchSessionChart();
    fetchmonthRegistration();
    fetchAvgSessionTime();
    axios.all([
      axios.get('https://localhost:7014/api/Admin/User/UserCount'),
      axios.get('https://localhost:7014/api/Admin/User/BlockedUserCount')
    ]).then(axios.spread((userResponse, blockedUserResponse) => {
      setUserCount(userResponse.data);
      setBlockedUserCount(blockedUserResponse.data);

    })).catch(error => {
      console.error('Error fetching data:', error);
    });
    axios.all([
      axios.get('https://localhost:7014/api/AdminPayment/Profit/Month'),
      axios.get('https://localhost:7014/api/AdminPayment/Profit/AllTime'),
      axios.get('https://localhost:7014/api/AdminPayment/Profit/Month/Banner'),
      axios.get('https://localhost:7014/api/AdminPayment/Profit/Month/Promotion'),
      axios.get('https://localhost:7014/api/AdminPurchase/CountOfPurchases')
    ]).then(axios.spread((
      profitMonthResponse,
      profitAllTimeResponse,
      profitBannerResponse,
      profitPromotionResponse,
      CountOfPurchasesResponse
    ) => {
      setProfitMonth(profitMonthResponse.data.profit);
      setProfitAllTime(profitAllTimeResponse.data.profit);
      setProfitBanner(profitBannerResponse.data.profit ? profitBannerResponse.data.profit : 0);
      setProfitPromotion(profitPromotionResponse.data.profit);
      setCountOfPurchases(CountOfPurchasesResponse.data)
    })).catch(error => {
      console.error('Error fetching data:', error);
    });
  }, []);

  return (
    <div style={{ backgroundColor: "#ECEDEF" }} className='admin-flex'>
      <AdminSideBar />
      <div className='admin-dashboard'>
        <h2>Інформація про сайт</h2>

        <div className="admin-dashboard-details">
          <div className='admin-dashboard-card'>
            <label>Сесії по пристроям</label>
            <hr style={{ width: "100%" }} />
            <div className='admin-dashboard-card-items'>
              <div className='admin-dashboard-card-detail-left' >
                <BarChart userCount={userCount} blockedUserCount={blockedUserCount}  ></BarChart>
              </div>
              <div className='admin-dashboard-card-detail' >
                <div className='admin-dashboard-card-detail-right'>
                  <div style={{
                    width: "10px",
                    height: "10px",
                    borderRadius: '50%',
                    backgroundColor: "#335A74",
                    alignSelf: "center"
                  }}>
                  </div>
                  <div style={{ width: "100%", marginTop: "4px" }} >Користувачів<p style={{ fontWeight: 'bold' }}>{percentageUsers.toFixed(1)}%</p> </div>
                </div>
                <div className='admin-dashboard-card-detail-right'>
                  <div style={{
                    width: "10px",
                    height: "10px",
                    borderRadius: '50%',
                    backgroundColor: "#1684EA",
                    alignSelf: "center"
                  }}>
                  </div>
                  <div style={{ width: "100%", marginTop: "4px" }} >Заблоковані<p style={{ fontWeight: 'bold' }}>{percentageBlockedUsers.toFixed(1)}%</p> </div>
                </div>
              </div>
            </div>
          </div>

          <div className='admin-dashboard-card'>
            <label>Сесія сайту</label>
            <hr style={{ width: "100%" }} />
            <div className='admin-dashboard-card-items'>
              <LineChart dateLogins={sessionChart.dateLogins}  ></LineChart>
            </div>
          </div>

          <div className='admin-dashboard-card'>
            <label>Інформація о користувах</label>
            <hr style={{ width: "100%" }} />
            <div style={{ flexDirection: "column", paddingTop: "5%", gap: "5%" }} className='admin-dashboard-card-items'>
              <div className='admin-dashboard-card-info'>
                <label>Нові реєстраціЇ</label>
                <p>{monthRegistration}</p>
              </div>
              <div className='admin-dashboard-card-info'>
                <label>Середня тривалість</label>
                <p style={{fontSize: "28px"}}>{averageSessionTime}</p>
              </div>
            </div>
          </div>

          <div className="admin-dashboard-card">
            <label>Прибуток</label>
            <hr style={{ width: "100%" }} />
            <div style={{ flexWrap: "wrap", flexDirection: "row", marginLeft: "10%",marginTop: "8%",  gap: "18%" }} className='admin-dashboard-card-items'>
              <div style={{padding: "0"}} className='admin-dashboard-card-info'>
                <label>За весь час</label>
                <p>{profitAllTime}</p>
              </div>
              <div style={{padding: "0"}} className='admin-dashboard-card-info'>
                <label>За місяць</label>
                <p>{profitMonth}</p>
              </div>
              <div style={{padding: "0"}} className='admin-dashboard-card-info'>
                <label>За Банери</label>
                <p>{profitBanner}</p>
              </div>
              <div style={{padding: "0"}} className='admin-dashboard-card-info'>
                <label>За просування</label>
                <p>{profitPromotion}</p>
              </div>
            </div>
          </div>
          <div className="admin-dashboard-card">
            <label>Прибуток</label>
            <hr style={{ width: "100%" }} />
            <div style={{ flexWrap: "wrap", flexDirection: "row", marginLeft: "10%",marginTop: "8%",  gap: "15%" }} className='admin-dashboard-card-items'>
              <div style={{padding: "0"}} className='admin-dashboard-card-info'>
                <label>Кількість покупок</label>
                <p>{profitCountOfPurchases}</p>
              </div>
              <div style={{padding: "0"}} className='admin-dashboard-card-info'>
                <label>За місяць</label>
                <p>{profitCountOfPurchases}</p>
              </div>
              <div style={{padding: "0", alignSelf: "center"}} className='admin-dashboard-card-info'>
                <label>Куплено банерів</label>
                <p>{profitBanner/400}</p>
              </div>
            </div>
          </div>

          <div className='admin-dashboard-card'>
            <label>Сесії по пристроям</label>
            <hr style={{ width: "100%" }} />
            <div className='admin-dashboard-card-items'>
              <div className='admin-dashboard-card-detail-left' >
                <AdminPieChart dataNumbers={sessionDetail} ></AdminPieChart>
              </div>
              <div className='admin-dashboard-card-detail' >
                <div className='admin-dashboard-card-detail-right'>
                  <div style={{
                    width: "10px",
                    height: "10px",
                    borderRadius: '50%',
                    backgroundColor: "#335A74",
                    alignSelf: "center"
                  }}>
                  </div>
                  <div style={{ width: "100%", marginTop: "4px" }} >Моб. версія<p style={{ fontWeight: 'bold' }}>{sessionDetail[0]}%</p> </div>
                </div>
                <div className='admin-dashboard-card-detail-right'>
                  <div style={{
                    width: "10px",
                    height: "10px",
                    borderRadius: '50%',
                    backgroundColor: "#1684EA",
                    alignSelf: "center"
                  }}>
                  </div>
                  <div style={{ width: "100%", marginTop: "4px" }} >Повна версія<p style={{ fontWeight: 'bold' }}>{sessionDetail[1]}%</p> </div>
                </div>
                <div className='admin-dashboard-card-detail-right'>
                  <div style={{
                    width: "10px",
                    height: "10px",
                    borderRadius: '50%',
                    backgroundColor: "#FFC220",
                    alignSelf: "center"
                  }}>
                  </div>
                  <div style={{ width: "100%", marginTop: "4px" }} >Планшет<p style={{ fontWeight: 'bold' }} >{sessionDetail[2]}%</p> </div>
                </div>
                <div className='admin-dashboard-card-detail-right'>
                  <div style={{
                    width: "10px",
                    height: "10px",
                    borderRadius: '50%',
                    backgroundColor: "#F30000",
                    alignSelf: "center"
                  }}>
                  </div>
                  <div style={{ width: "100%", marginTop: "4px" }} >Невизначено<p style={{ fontWeight: 'bold' }}>{sessionDetail[3]}%</p> </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
