import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import App from "./App"; // Make sure to provide the correct path to your App component

jest.mock("./hooks/useGetWeather", () => ({
  useGetWeather: () => ({
    data: { description: "cloudy" },
    isSuccess: true,
  }),
}));

test("check App component is rendered", () => {
  render(<App />);

  // Check inputs are rendered
  const cityInput = screen.getByTestId("city");
  const countryInput = screen.getByTestId("country");
  expect(cityInput).toBeInTheDocument();
  expect(countryInput).toBeInTheDocument();

  // Check 'Get Weather' button is rendered
  const getWeatherButton = screen.getByRole("button", { name: "Get Weather" });
  expect(getWeatherButton).toBeInTheDocument();
});

test("check user input and form submission are handled", async () => {
  render(<App />);

  const cityInput = screen.getByTestId("city");
  const countryInput = screen.getByTestId("country");

  userEvent.type(cityInput, "London");
  userEvent.type(countryInput, "US");

  expect(cityInput).toHaveValue("London");
  expect(countryInput).toHaveValue("US");

  const getWeatherButton = screen.getByRole("button", { name: "Get Weather" });
  fireEvent.click(getWeatherButton);

  await waitFor(() => {
    const weatherDescription = screen.getByText("cloudy");

    // Check weather description is displayed correctly
    expect(weatherDescription).toBeInTheDocument();
  });
});
