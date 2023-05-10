import React from "react";
import classes from "./DatasetTitle.module.css"
import { IconButton } from "@mui/material";
import EditIcon from '@mui/icons-material/Edit';

const DatasetTitle = ({ 
    setOpen, 
    selectedDataset, 
}) => {
    return (
        <div className={classes.subHeading}>
            <span className={classes.datasetName}>Dataset:</span> 
            <span>{selectedDataset?.name}</span>
            <IconButton
                onClick={() => setOpen(true)}
            >
                <EditIcon/>
            </IconButton> 
        </div>
    )
};

export default DatasetTitle;