import { useQuery } from "react-query";

const useGetWeather = (city, country) => {
  const fetchWeather = async ({ city, country }) => {
    const response = await fetch(
      `/api/Weather/GetCurrentWeather?city=${city}&country=${country}`,
    );
    return response.json();
  };

  return useQuery(
    ["data", { city: "defaultCity", country: "defaultCountry" }],
    fetchWeather,
  );
};

export default useGetWeather;
