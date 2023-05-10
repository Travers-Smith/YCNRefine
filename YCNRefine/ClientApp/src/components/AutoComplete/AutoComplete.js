import React, { useState } from "react";
import { Autocomplete, TextField } from "@mui/material";

const AutoComplete = ({ 
  label,
  options, 
  value,
  setValue,
  optionLabelName,
  width,
  onChange
}) => {
    const [inputValue, setInputValue] = useState("");

    return (
      <Autocomplete
        isOptionEqualToValue={(option, value) => option.id === value.id || option.id}
        value={value}
        onChange={(_, newValue) => {
          setValue(newValue);
          if(onChange){
            onChange(newValue)
          }
        }}
        inputValue={inputValue}
        onInputChange={(_, newInputValue) => {
          setInputValue(newInputValue);
        }}
        getOptionLabel={option => option[optionLabelName]}
        id="controllable-states-demo"
        options={options}
        renderInput={(params) => <TextField {...params} label={label} />}
        sx={{ width: width }}
      />
    );
};

export default AutoComplete;