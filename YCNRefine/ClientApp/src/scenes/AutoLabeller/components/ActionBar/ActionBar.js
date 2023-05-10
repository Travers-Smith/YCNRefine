import React from "react";
import classes from "./ActionBar.module.css";

const ActionBar = () => {
    return (
        <div className={classes.actionBar}>
            <a
                href="/auto-labeller"
            >
                New
            </a>
        </div>
    )
};

export default ActionBar;