import React from 'react';

const WeatherInfo = ({weather}) => {
    return (
        <div>
            <h2>Weather Information</h2>
            <p>Description: {weather.description}</p>
        </div>
    )
}

export default WeatherInfo;
