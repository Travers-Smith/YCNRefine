import React from "react";
import classes from "./Message.module.css";
import { IconButton, TextField } from "@mui/material";
import RemoveCircleOutlineIcon from "@mui/icons-material/RemoveCircleOutline";

const Message = ({ data, setChat, index }) => {
    const {
        isSystem,
        text
    } = data;

    const updateMessageText = e => {
        setChat(chat => {
            const newChat = { ...chat };
            
            newChat.messages[index].text = e.target.value;
            
            return newChat;
        })
    };
    
    return (
        <div className={classes.messageContainer}>
            <div className={classes.heading}>
                {
                    isSystem ?
                        "System"
                    : 
                        "User"
                }
            </div>
            <TextField 
                multiline
                id="outlined-basic" 
                label="Message"
                variant="outlined"
                value={text}
                onChange={updateMessageText}
            />
            <IconButton
                onClick={() => setChat(chat => ({
                    ...chat,
                    messages: [
                        ...chat.messages.filter((message, messageIndex) => index !== messageIndex)
                    ]
                }))}
            >
                <RemoveCircleOutlineIcon/>
            </IconButton>
        </div>
    )
};

export default Message;