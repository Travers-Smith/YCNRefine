import React from "react";
import classes from "./Item.module.css";
import ChatBubbleIcon from '@mui/icons-material/ChatBubble';
import { Link } from "react-router-dom";
import Edit from '@mui/icons-material/Edit';
import { IconButton } from "@mui/material";

const Item = ({ data, itemUrl }) => {
    const {
        id,
        name
    } = data;

    return (
        <div className={classes.item}>
            <Link to={itemUrl + id} className={classes.itemLink}>
                <ChatBubbleIcon
                    fontSize="small"
                />
                <div>{name}</div>
            </Link>
            <div className={classes.rightSide}>
                <IconButton>
                    <Edit
                        fontSize="small"
                    />
                </IconButton>
            </div>
        
        </div>
    )
};

export default Item;