import React, { useEffect, useState } from "react";
import { 
    Button, 
    CircularProgress, 
    Dialog,
    DialogActions, 
    DialogContent, 
    DialogTitle, 
    FormControlLabel,
    Switch,
    TextField
} from "@mui/material";
import AutoComplete from "../AutoComplete/AutoComplete";
import useSafeDataFetch from "../../hooks/useSafeDataFetch";

const DatasetModal = ({ 
    open, 
    setOpen, 
    selectedDataset, 
    setSelectedDataset, 
    handleClose 
}) => {
    const [openAutoComplete, setOpenAutoComplete] = useState(false);
    const [existingDataset, setExistingDataset] = useState(true);
    const [datasets, setDatasets] = useState([]);
    const [newDataset, setNewDataset] = useState("");
    const [{ isLoading }, safeFetch] = useSafeDataFetch();
    const [{ isLoading: postLoading }, safePostFetch] = useSafeDataFetch();

    const addDataset = async () => {
        const response = await safePostFetch({ 
            url: "/dataset/add", 
            data: {
                name: newDataset
            },
            method: "POST"
        });

        if(!response.isError){
            setSelectedDataset(response.data);
            setExistingDataset(true);
            setOpen(false);
        }
    }

    useEffect(() => {
        const fetchDatasets = async () => {
            const response = await safeFetch({
                url: "/dataset/get-datasets"
            });

            if(!response.isError){
                setDatasets(response.data);
            }
        };

        fetchDatasets();
    }, [open]);
    
    return (
        <Dialog maxWidth="lg" open={open} onClose={handleClose}>
            <DialogTitle>Datasets</DialogTitle>
            <DialogContent>
                <FormControlLabel 
                    control={
                        <Switch 
                            defaultChecked
                            value={existingDataset} 
                            onChange={e => setExistingDataset(e.target.checked)}
                        />
                    } 
                    label="Existing dataset" 
                />
                <br/>
                <br/>
                {
                    existingDataset ?
                            <AutoComplete
                                open={openAutoComplete}
                                setOpen={setOpenAutoComplete}
                                options={datasets}
                                setOptions={setDatasets}
                                isLoading={isLoading}
                                label="Dataset"
                                value={selectedDataset}
                                setValue={setSelectedDataset}
                                optionLabelName="name"
                                width={500}
                                onChange={() => setOpen(false)}
                            />
                        :
                            <TextField
                                label="New dataset name"
                                style={{
                                    width: 500
                                }}
                                value={newDataset}
                                onChange={e => setNewDataset(e.target.value)}
                            />
                }
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose}>Close</Button>
                {
                    !existingDataset &&
                        <Button 
                            onClick={addDataset}
                        >
                            {
                                postLoading ?
                                    <CircularProgress/>
                                :
                                    "Submit"
                            }
                        </Button>
                }
            </DialogActions>
      </Dialog>
    )
};

export default DatasetModal;