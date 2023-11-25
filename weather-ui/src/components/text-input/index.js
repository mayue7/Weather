import React from "react";
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

  return (
    <>
      <label htmlFor={field.name}>{label}</label>
      <input
        type="text"
        name={field.name}
        onChange={field.onChange}
        onBlur={field.onBlur}
        value={field.value}
      />
      {invalid && <p className="errorMessage">{error.message}</p>}
    </>
  );
};

export default TextInput;
