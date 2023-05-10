import React, { useEffect, useState } from "react";
import classes from "./GenerativeLabeller.module.css";
import useSafeDataFetch from "../../hooks/useSafeDataFetch";
import DataButton from "../../utils/DataButton/DataButton";
import DatasetModal from "../../components/DatasetModal/DatasetModal";
import DatasetTitle from "../../components/DatasetTitle/DatasetTitle";
import DatasetDataSidebar from "../../components/DatasetDataSidebar/DatasetDataSidebar";
import ActionBar from "./components/ActionBar/ActionBar";
import { useParams } from "react-router-dom";

const GenerativeLabeller = () => {
    const [sample, setSample] = useState({
        context: "",
        input: "", 
        output: "",
        id: null
    });

    const [open, setOpen] = useState(true);

    const [{ isLoading: isSubmitting }, safeAddFetch] = useSafeDataFetch();
    const [{ isLoading: isDeleting }, safeDeleteFetch] = useSafeDataFetch();

    const safeFetch = useSafeDataFetch()[1];
    const [selectedDataset, setSelectedDataset] = useState({
        name: ""
    });

    const [samples, setSamples] = useState([]);

    const datasetId = selectedDataset?.id;

    const { sampleId } = useParams();

    const addSample = async () => {
        const response = await safeAddFetch({
            url: "/generative-sample/add", 
            data: {
                ...sample,
                datasetId: selectedDataset?.id
            },
            method: "POST"
        });
        
        if(!response.isError){
            setSample(cs => ({
                ...cs,
                id: response.data.id,
                name: response.data.name
            }));

            setSamples(samples => {
                const id = response.data.id
                if(!samples.some(s => s.id === id)){
                    return [
                        {
                            id: id,
                            name: response.data.name
                        },
                        ...samples
                    ]
                }

                return samples;
            })
        }
    }

    const removeSample = async () => {
        const response = await safeDeleteFetch({
            url: "/generative-sample/delete/" + sample.id, 
            method: "DELETE"
        });
        
        if(!response.isError){
            setSample({
                context: "",
                input: "", 
                output: "",
                id: null
            });
        }
    }

    const updateSample = async () => {
        await safeDeleteFetch({
            data: {
                ...sample,
                datasetId: selectedDataset?.id
            },
            url: "/generative-sample/update", 
            method: "PATCH"
        });
    }

    useEffect(() => {
        const fetchSample = async () => {
            const response = await safeFetch({
                url: "/generative-sample/get-sample-by-id/" + sampleId
            });

            if(!response.isError){
                setSample(response.data)
                setSelectedDataset(response.data.dataset);
            }
        };

        if(sampleId){
            fetchSample();
        } else {
            setSample({
                context: "",
                input: "", 
                output: "",
                id: null
            });
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [sampleId]);
    return (
        <div className={classes.generativeLabeller}>
            <DatasetModal
                open={open}
                setOpen={setOpen}
                handleClose={() => setOpen(false)}
                selectedDataset={selectedDataset}
                setSelectedDataset={setSelectedDataset}
            />
            <div className={classes.heading}>
                <h5>Generative Labeller</h5>
                <DatasetTitle
                    selectedDataset={selectedDataset}
                    setOpen={setOpen}
                />
            </div>
            <ActionBar/>
            <div className={classes.context}>
                <textarea
                    value={sample.context}
                    onChange={e => setSample(cs => ({
                        ...cs, 
                        context: e.target.value
                    }))}
                    placeholder="Context (optional)"
                />
            </div>
            <div className={classes.body}>
                <DatasetDataSidebar
                    datasetId={datasetId}
                    title="Samples"
                    dataUrl="/generative-sample/get-by-dataset/"
                    items={samples}
                    setItems={setSamples}
                    itemUrl="/generative-labeller/"
                />
                <div className={classes.sampleContainer}>
                    <textarea
                        className={classes.input}
                        value={sample.input}
                        onChange={e => setSample(cs => ({
                            ...cs, 
                            input: e.target.value
                        }))}
                        placeholder="Input"
                    />
                    <div className={classes.actionButtonContainer}>
                        <DataButton
                            disabled={(!sample.input?.length > 0 || !sample.output?.length > 0) || !selectedDataset.id}
                            onClick={sample.id ? updateSample : addSample}
                            isLoading={isSubmitting}
                        >
                            {sample.id ? "Update" : "Add"}
                        </DataButton>
                        {
                            sample.id &&
                                <DataButton
                                    onClick={removeSample}
                                    isLoading={isDeleting}
                                >
                                    Delete
                                </DataButton>
                        }
                    </div>
                    <textarea
                        className={classes.output}
                        value={sample.output}
                        onChange={e => setSample(cs => ({
                            ...cs, 
                            output: e.target.value
                        }))}
                        placeholder="Output"
                    />
                </div>
            </div>
        </div>
    );
}

export default GenerativeLabeller;