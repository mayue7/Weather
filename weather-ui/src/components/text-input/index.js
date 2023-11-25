import React, { useState } from "react";
import { useController } from "react-hook-form";
import "./styles.css";

const TextInput = ({ label, control, name, rules }) => {
  const {
    field,
    fieldState: { invalid, error },
  } = useController({
    name,
    control,
    rules,
  });
  const [inputValue, setInputValue] = useState(field.value);

  return (
    <>
      <label htmlFor={field.name}>{label}</label>
      <input
        type="text"
        name={field.name}
        onChange={(e) => {
          setInputValue(e.target.value);
          field.onChange(e.target.value);
        }}
        onBlur={field.onBlur}
        value={inputValue}
        data-testid={field.name}
      />
      {invalid && <p className="errorMessage">{error.message}</p>}
    </>
  );
};

export default TextInput;
