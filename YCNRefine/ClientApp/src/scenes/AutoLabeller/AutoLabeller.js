import React, { useEffect, useState } from "react";
import classes from "./AutoLabeller.module.css";
import DataInput from "./components/DataInput/DataInput";
import QuestionAnswers from "./components/QuestionAnswers/QuestionAnswers";
import DatasetTitle from "../../components/DatasetTitle/DatasetTitle";
import DatasetModal from "../../components/DatasetModal/DatasetModal";
import DatasetDataSidebar from "../../components/DatasetDataSidebar/DatasetDataSidebar";
import useSafeDataFetch from "../../hooks/useSafeDataFetch";
import { useParams } from "react-router-dom";
import ActionBar from "./components/ActionBar/ActionBar";

const AutoLabeller = () => {
    const safeFetch = useSafeDataFetch()[1];
    const [selectedDataset, setSelectedDataset] = useState({
        id: null,
        name: ""
    });
    
    const [open, setOpen] = useState(true);
    const [pastOriginalSources, setPastOriginalSources] = useState([]);

    const { originalSourceId } = useParams();

    const [originalSource, setOriginalSource] = useState({
        id: originalSourceId,
        sourceText: ""
    });
    
    const [questionAnswers, setQuestionAnswers] = useState([]);

    const datasetId = selectedDataset?.id;


    useEffect(() => {
        setOriginalSource(os => ({
            ...os,
            sourceText: ""
        }));
        
        setQuestionAnswers([]);

        const fetchData = async () => {
            const response = await safeFetch({
                url: "/question-answer/get-by-dataset/" + originalSourceId
            });

            if(!response.isError){
                setQuestionAnswers(response.data)
                setSelectedDataset(response.data.dataset)
            }
        };

        if(datasetId && originalSourceId){
            fetchData();
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [originalSourceId]);

    return (
        <div className={classes.autoLabeller}>
            <DatasetModal
                handleClose={() => setOpen(false)}
                open={open}
                selectedDataset={selectedDataset}
                setSelectedDataset={setSelectedDataset}
                setOpen={setOpen}
            />
            <div>
                <h5>Auto Labeller</h5>
                <DatasetTitle
                    selectedDataset={selectedDataset}
                    setOpen={setOpen}
                />
            </div>
            <ActionBar/>
            <div className={classes.body}>
                <DatasetDataSidebar
                    dataUrl="/original-source/get-by-dataset/"
                    datasetId={datasetId}
                    title="History"
                    itemUrl="/auto-labeller/"
                    items={pastOriginalSources}
                    setItems={setPastOriginalSources}
                />    
                <div className={classes.bodyContent}>
                    <DataInput
                        datasetId={datasetId}
                        hasQuestionAndAnswers={questionAnswers.length > 0}
                        setQuestionAnswers={setQuestionAnswers}
                        originalSource={originalSource}
                        setOriginalSource={setOriginalSource}
                        setPastOriginalSources={setPastOriginalSources}
                    />
                    {
                        questionAnswers?.length > 0 && 
                            <QuestionAnswers
                                originalSourceId={originalSource.id}
                                questionAnswers={questionAnswers}
                                setQuestionAnswers={setQuestionAnswers}
                            />
                    }
                </div>
            </div>
        </div>
    )
};

export default AutoLabeller;