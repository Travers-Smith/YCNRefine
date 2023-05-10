import React from "react";
import classes from "./SidebarItem.module.css";
import { Link } from "react-router-dom";

const SidebarItem = ({ children, icon, url }) => {
    return (
        <Link 
            className={classes.sidebarItem} 
            to={url}
        >
            {icon}
            {children}
        </Link>
    );
};

export default SidebarItem;