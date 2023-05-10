import React from "react";
import classes from "./Sidebar.module.css";
import SidebarItem from "./components/SidebarItem/SidebarItem";
import Logout from "./components/Logout/Logout";
import ChatBubbleIcon from '@mui/icons-material/ChatBubble';
import LabelIcon from '@mui/icons-material/Label';
import QuizIcon from '@mui/icons-material/Quiz';

const Sidebar = () => {
    return (
        <div className={classes.sidebar}>
            <div className={classes.topSection}>
                <SidebarItem  
                    icon={<ChatBubbleIcon/>}
                    url="/chat-labeller" 
                >
                    Chat Labeller
                </SidebarItem>
                <SidebarItem
                    icon={<QuizIcon/>}
                    url="/auto-labeller"
                >
                    Auto Labeller
                </SidebarItem>
                <SidebarItem 
                    icon={<LabelIcon/>}
                    url="/generative-labeller" 
                >
                    Generative Labeller
                </SidebarItem>
            </div>
            <div className={classes.bottomSection}>
                <Logout/>
            </div>
        </div>
    );
};

export default Sidebar;