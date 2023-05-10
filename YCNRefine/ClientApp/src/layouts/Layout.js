import React from "react";
import classes from "./Layout.module.css";
import Sidebar from "./components/Sidebar/Sidebar";
import { Outlet } from "react-router-dom";

const Layout = () => {
    return (
        <div className={classes.layout}>
            <Sidebar/>
            <div className={classes.body}>
                <Outlet/>
            </div>
        </div>
    )
};

export default Layout;