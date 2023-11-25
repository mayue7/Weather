import React from "react";
import { render, screen } from "@testing-library/react";
import TextInput from "./index";

jest.mock("react-hook-form", () => ({
  ...jest.requireActual("react-hook-form"),
  useController: () => ({
    control: () => jest.fn(),
    field: { name: "testInput" },
    fieldState: { invalid: false },
  }),
}));

test("check TextInput component is rendered", () => {
  const label = "TextInput Label";
  const name = "testInput";
  const rules = { required: "This field is required" };
  const mockControl = {};

  render(
    <TextInput label={label} control={mockControl} name={name} rules={rules} />,
  );

  // Check if label is rendered correctly
  const labelElement = screen.getByText(label);
  expect(labelElement).toBeInTheDocument();

  // Check if input is rendered correctly
  const inputElement = screen.getByTestId(name);
  expect(inputElement).toBeInTheDocument();
});
