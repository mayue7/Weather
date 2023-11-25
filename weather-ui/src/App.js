import "./App.css";
import React, { useState } from "react";
import useGetWeather from "./hooks/useGetWeather";
import { useForm } from "react-hook-form";
import TextInput from "./components/text-input";
import SubmitButton from "./components/submit-button";

function App() {
  const [city, setCity] = useState();
  const [country, setCountry] = useState();

  const { data: weatherData, status, refetch } = useGetWeather(city, country);

  const { control, handleSubmit, values } = useForm({
    defaultValues: {
      city: "",
      country: "",
    },
  });

  const submitForm = (data) => {
    setCity(data.city);
    setCountry(data.country);
    refetch();
  };

  console.log(city, country, values);
  return (
    <div>
      <form onSubmit={handleSubmit(submitForm)}>
        <TextInput
          label="City"
          control={control}
          name="city"
          rules={{ required: "City field is required." }}
        />
        <TextInput
          label="Country"
          control={control}
          name="country"
          rules={{ required: "Country field is required." }}
        />
        <SubmitButton label="Get Weather" />
      </form>
      {status === "success" && weatherData && weatherData.weather && (
        <>
          <h2>Weather Information</h2>
          <p>Description: {weatherData.weather[0].description}</p>
        </>
      )}
    </div>
  );
}

export default App;
