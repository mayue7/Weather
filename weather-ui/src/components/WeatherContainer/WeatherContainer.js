import React from 'react';
import WeatherForm from './WeatherForm';
import WeatherInfo from './WeatherInfo';
import useGetWeather from '../../hooks/useGetWeather';

const WeatherContainer = () => {
    const {weatherInfo, status, onSubmit } = useGetWeather()

    return (
        <div>
            <WeatherForm onSubmit={onSubmit} />
            {status === 'success' && <WeatherInfo weather={weatherInfo} />}
        </div>
    )
}

export default WeatherContainer;