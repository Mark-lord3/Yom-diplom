import React, { useState, useEffect, useRef } from 'react';
import { Bar } from "react-chartjs-2";
import { Chart, LineController, LineElement, PointElement, LinearScale, BarElement, BarController, CategoryScale, Decimation, Filler, Legend, Title, Tooltip } from 'chart.js';


export interface BarChartProps {
    userCount: number;
    blockedUserCount: number;
}
const BarChart: React.FC<BarChartProps> = ({ userCount, blockedUserCount }) => {
    Chart.register(LineController, LineElement, PointElement, LinearScale, BarElement, BarController, CategoryScale, Decimation, Filler, Legend, Title, Tooltip);
    const data = {
        labels: ["Користувачі", "Заблоковані"],
        datasets: [
            {
                label: "Користувачі",
                data: [userCount, blockedUserCount],
                backgroundColor: [
                    "#FFC220",
                    "#1684EA"
                ],
                borderColor: [
                    "transparent"
                ],
                cutout: [
                    50
                ]
            },
        ],
    };
    return (
        <div>
            <Bar style={{ width: "100%", height: "100%" }} data={data} />
        </div>
    );
};
export default BarChart;