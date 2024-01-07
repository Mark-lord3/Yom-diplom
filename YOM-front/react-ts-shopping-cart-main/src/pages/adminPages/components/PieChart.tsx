
import React, { useState, useEffect, useRef } from 'react';
import { Doughnut } from "react-chartjs-2";
import { ArcElement, Chart } from 'chart.js'

interface PieChartProps {
    dataNumbers: number[];
}
const AdminPieChart: React.FC<PieChartProps> = ({ dataNumbers }) => {
    Chart.register(ArcElement);
    if (dataNumbers === null) {
        return <div>Loading...</div>;
    }
    Chart.register(ArcElement);
    const data = {
        datasets: [
            {
                label: "%",
                data: dataNumbers,
                backgroundColor: [
                    "#FFC220",
                    "#1684EA",
                    "#335A74",
                    "#F30000",
                ],
                borderColor: [
                    "transparent"
                ],
                cutout: [
                    80
                ]
            },
        ],
        scales: {
            xAxes: [{
                beginAtZero: true,
                ticks: {
                    autoSkip: false
                }
            }]
        }
    };
    return (
        <div>

            <Doughnut style={{ width: "100%", height: "100%" }} data={data} options={{
                plugins: {
                    legend: {
                        position: 'right',
                    },
                },
            }} />

        </div>
    );
};
export default AdminPieChart;