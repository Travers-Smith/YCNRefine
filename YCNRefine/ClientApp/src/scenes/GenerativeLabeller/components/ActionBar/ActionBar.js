import React from "react";
import classes from "./ActionBar.module.css";

const ActionBar = () => {
    return (
        <div className={classes.actionBar}>
            <a
                href="/generative-labeller"
            >
                New
            </a>
        </div>
    )
};

export default ActionBar;