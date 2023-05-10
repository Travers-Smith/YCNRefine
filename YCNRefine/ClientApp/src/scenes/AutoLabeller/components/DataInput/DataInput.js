import React from "react";
import classes from "./DataInput.module.css";
import DataButton from "../../../../utils/DataButton/DataButton";
import useSafeDataFetch from "../../../../hooks/useSafeDataFetch";

const DataInput = ({ 
    datasetId,
    hasQuestionAndAnswers, 
    setQuestionAnswers, 
    originalSource, 
    setOriginalSource,
    setPastOriginalSources
}) => {
    const [{ isLoading }, safeFetch] = useSafeDataFetch();

    const extractQuestionsAnswers = async () => {
        const response = await safeFetch({
            data: {
                datasetId: datasetId,
                originalSourceId: originalSource?.id,
                sourceText: originalSource.sourceText
            },
            method: "POST",
            url: "/question-answer-generator/generate-from-free-text"
        });

        if(!response.isError){
            setPastOriginalSources(pastOriginalSources => {
                const originalSourceId = response.data.originalSource.id;
                if(!pastOriginalSources.some(pos => pos.id === originalSourceId)){
                    return [
                        {
                            id: originalSourceId,
                            name: response.data.originalSource.name
                        },
                        ...pastOriginalSources
                    ];
                }

                return pastOriginalSources

            })
            setQuestionAnswers(questionsAnswers => [...questionsAnswers, ...response.data.questionAnswers]);
            setOriginalSource(os => ({ 
                ...os, 
                id: response.data.originalSource.id
            }));
        }
    };

    return (
        <div className={hasQuestionAndAnswers ? classes.partialScreenDataInput : classes.fullScreenDataInput}>
            <textarea
                className={classes.textarea}
                onChange={e => setOriginalSource(os => ({
                    ...os,
                    sourceText: e.target.value
                }))}
                value={originalSource.sourceText}
            />
            <div className={classes.buttonContainer}>
                <DataButton
                    disabled={!datasetId}
                    isLoading={isLoading}
                    onClick={extractQuestionsAnswers}
                >
                    Extract
                </DataButton>
            </div>
        </div>
    )
};

export default DataInput;