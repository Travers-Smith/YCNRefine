import React from "react";
import classes from "./Item.module.css";
import ChatBubbleIcon from '@mui/icons-material/ChatBubble';
import { Link } from "react-router-dom";

const Item = ({ data, itemUrl }) => {
    const {
        id,
        name
    } = data;

    return (
        <Link to={itemUrl + id} className={classes.itemLink}>
            <ChatBubbleIcon
                fontSize="small"
            />
            <div>{name}</div>
        </Link>
    )
};

export default Item;