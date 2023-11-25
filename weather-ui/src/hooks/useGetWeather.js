import { useQuery } from "react-query";

const useGetWeather = (city, country) => {
  const requestOptions = {
    method: "GET",
    headers: { "x-api-key": "weather-api-key-2" },
  };

  const fetchWeather = async ({ city, country }) => {
    const response = await fetch(
      `http://localhost:5087/api/Weather/GetCurrentWeather?city=${city}&country=${country}`,
      requestOptions,
    );
    return response.json();
  };

  return useQuery(
    ["data", { city, country }],
    () => fetchWeather({ city, country }),
    {
      enabled: !!(city && country),
    },
  );
};

export default useGetWeather;
