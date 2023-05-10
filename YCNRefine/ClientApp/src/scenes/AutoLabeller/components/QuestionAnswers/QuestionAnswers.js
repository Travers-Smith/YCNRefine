import React, { useState } from "react";
import classes from "./QuestionAnswers.module.css";
import QuestionAnswer from "./components/QuestionAnswer/QuestionAnswer";

const QuestionAnswers = ({ questionAnswers, setQuestionAnswers, originalSourceId, setOriginalSources }) => {
    const [editQuestion, setEditQuestion] = useState(null);

    console.log(questionAnswers)
    return (
        <div> 
            <h6>Questions/Answers</h6>
            <hr/>
            <div className={classes.questionAnswers}>
                {
                    questionAnswers.map((qa, index) => (
                        <QuestionAnswer
                            key={index}
                            data={qa}
                            editQuestion={editQuestion}
                            setEditQuestion={setEditQuestion}
                            originalSourceId={originalSourceId}
                            setQuestionAnswers={setQuestionAnswers}
                            index={index}
                            setOriginalSources={setOriginalSources}
                        />
                    ))
                }
            </div>
        </div>
    )  
};

export default QuestionAnswers;