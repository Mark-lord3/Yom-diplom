import React, { useState, useEffect, useRef } from 'react';
import { Line } from "react-chartjs-2";
import { Chart, LineController, LineElement, PointElement, LinearScale, BarElement, BarController, CategoryScale, Decimation, Filler, Legend, Title, Tooltip } from 'chart.js';


interface LineChartProps {
    dateLogins: string[];
}

interface DateLogin {
    x: string;
    y: number;
}
const LineChart: React.FC<LineChartProps> = ({ dateLogins }) => {
    Chart.register(
        CategoryScale,
        LinearScale,
        PointElement,
        LineElement,
        Title,
        Tooltip,
        Legend
    );

    const convertedData: DateLogin[] = dateLogins.map((dateString, index) => ({
        x: dateString,
        y: index + 0
    }));

    const data = {
        datasets: [
            {
                label: "Cесії",
                data: convertedData,
                fill: false,
                tension: 0.1,
                backgroundColor: [
                    "#1684EA"
                ],
                borderColor: [
                    "transparent"
                ],
            },
        ],
    };
    return (
        <div>
            <Line style={{ width: "100%", height: "100%" }} data={data} />
        </div>
    );
};
export default LineChart;