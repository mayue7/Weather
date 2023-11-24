import React from 'react';
import { useForm } from 'react-hook-form';

const WeatherForm = ({onSubmit}) =>{
    const {register, handleSubmit, formState: { errors }} = useForm();

    const submitForm =(data) => {
        onSubmit(data);
    }

    return (
        <form onSubmit={handleSubmit(submitForm)}>
            <label htmlFor='city'>City</label>
            <input type='text' name='city' ref={register("city", { required: true })} />
            {errors.city && <p>This field is required</p>}
            
            <label htmlFor='country'>Country</label>
            <input type='text' name='country' ref={register("country", { required: true })} />
            {errors.country && <p>This field is required</p>}
            
            <button type="submit">Get Weather</button>
        </form>
    )
}

export default WeatherForm;