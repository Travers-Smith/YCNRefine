import React from "react";
import classes from "./QuestionAnswer.module.css";
import { IconButton } from "@mui/material";
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import CancelIcon from '@mui/icons-material/Cancel';
import useSafeDataFetch from "../../../../../../hooks/useSafeDataFetch";
import EditIcon from '@mui/icons-material/Edit';

const QuestionAnswer = ({ 
    data, 
    setQuestionAnswers, 
    originalSourceId,
    editQuestion,
    setEditQuestion,
    index
}) => {
    const {
        id,
        question,
        answer
    } = data;
    
    const [{ isLoading, }, safeFetch] = useSafeDataFetch();

    const addResponse = async correctAnswer => {
        const response = await safeFetch({
            data: {
                id: id,
                answer: answer,
                correctAnswer: correctAnswer,
                question: question,
                originalSourceId: originalSourceId
            },
            method: "PUT",
            url: "/question-answer/add-or-update"
        });

        if(!response.isError){
            setEditQuestion(null);
            setQuestionAnswers(qas => {
                if(correctAnswer){
                    return qas.map((qa, i) => {
                        if(i === index){
                            qa.id = response.data;
                            qa.correctAnswer = correctAnswer
                        };
        
                        return qa;
                    })
                } else {
                    return qas.filter((qa, i) => i !== index)
                }
            });
        }
    };

    return (
        <div className={classes.questionAnswer}>
            <div className={classes.question}>
                {question}
            </div>
            <div className={classes.question}>
                {answer}
            </div>
            <div className={classes.actionButtons}>
                {
                    (editQuestion === id || id === 0) ?
                        <>
                            <IconButton
                                onClick={() => addResponse(false)}
                            >
                                <CancelIcon
                                    htmlColor="red"
                                />
                            </IconButton>
                            <IconButton
                                onClick={() => addResponse(true)}
                            >
                                <CheckCircleIcon
                                    htmlColor="green"
                                />
                            </IconButton>
                        </>
                    :
                        <IconButton
                            onClick={() => setEditQuestion(id)}
                        >
                            <EditIcon/>
                        </IconButton> 
                    }
            </div>
        </div>
    )
};

export default QuestionAnswer;