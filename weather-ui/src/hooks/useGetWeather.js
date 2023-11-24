import {
    useQuery,
    useQueryClient,
  } from 'react-query'

const useGetWeather = () => {
    const queryClient = useQueryClient();

    const fetchWeather = async({city, country}) => {
        const response = await fetch(`/api/Weather/GetCurrentWeather?city=${city}&country=${country}`)
        return response.json()
    }
    
    const {data: weatherInfo, status} = useQuery(['weatherInfo', { city: 'defaultCity', country: 'defaultCountry'}], fetchWeather)

    const onSubmit = (formData) => {
        queryClient.refetchQueries('weatherInfo', {active: true})
    }

    return { weatherInfo, status, onSubmit };
}

export default useGetWeather;