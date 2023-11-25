import "./App.css";
import React from "react";
import useGetWeather from "./hooks/useGetWeather";
import { useForm } from "react-hook-form";
import TextInput from "./components/text-input";
import SubmitButton from "./components/submit-button";

function App(props) {
  const { data, status, onSubmit } = useGetWeather(props.city, props.country);

  const { control, handleSubmit } = useForm();

  const submitForm = (data) => {
    console.log(data);
    onSubmit(data);
  };
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
      {status === "success" && (
        <>
          <h2>Weather Information</h2>
          <p>Description: {data.weather.description}</p>
        </>
      )}
    </div>
  );
}

export default App;
