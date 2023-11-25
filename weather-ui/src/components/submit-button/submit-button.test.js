import React from "react";
import { render, screen } from "@testing-library/react";
import SubmitButton from "./index";

test("check SubmitButton is rendered with the correct label", () => {
  const label = "Submit label";

  render(<SubmitButton label={label} />);

  const submitButton = screen.getByRole("button", { name: label });
  expect(submitButton).toBeInTheDocument();
});
