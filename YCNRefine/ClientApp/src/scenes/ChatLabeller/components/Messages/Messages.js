import React, { useState } from "react";
import classes from "./Messages.module.css";
import Message from "./components/Message/Message";
import DataButton from "../../../../utils/DataButton/DataButton";
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import useSafeDataFetch from "../../../../hooks/useSafeDataFetch";
import { Button } from "@mui/material";

const Messages = ({ chat, setChat, datasetId, setPastChats }) => {
    const [lastSaved, setLastSaved] = useState(null);
    const [{ isLoading }, safeFetch] = useSafeDataFetch();

    const saveMessages = async () => {
        const response = await safeFetch({
            url: "/chat/save",
            method: "POST",
            data: {
                datasetId: datasetId,
                id: chat?.id,
                messages: chat.messages
            }
        });

        if(!response.isError){
            const currentDate = new Date();

            const date = currentDate.toLocaleString("en-UK", { year: "numeric", "month": "long", day: "numeric" });

            const currentDateMinutes = currentDate.getMinutes();

            setLastSaved(`${currentDate.getHours()}:${currentDateMinutes > 9 ? "" : "0"}${currentDateMinutes} at ${date}`);

            setPastChats(pastChats => {
                const id = response.data.id;

                if(!pastChats.some(pc => pc.id === id)){
                    return [response.data, ...pastChats]
                }

                return pastChats
                
            });

            setChat(chat => ({
                ...chat,
                id: response.data.id,
                name: response.data.name
            }))
        }
    };

    return  (
        <div>
            <div className={classes.messages}>
                {
                    chat.messages?.map((message, index) => (
                        <Message
                            data={message}
                            key={index}
                            index={index}
                            setChat={setChat}
                        />
                    ))
                }
            </div>
            <div className={classes.addMessageContainer}>
                <div className={classes.lastSaved}>
                    {lastSaved ?? "Not saved"}
                </div>
                <Button
                    color="info"
                    className={classes.addMessage}
                    onClick={() => setChat(chat => ({
                        ...chat,
                        messages: [
                            ...chat.messages,
                            {
                                isSystem: !chat.messages[chat.messages.length - 1].isSystem,
                                text: ""
                            }
                        ]
                    }))}
                >
                    <AddCircleOutlineIcon/>
                    Add Message
                </Button>
            </div>
            <DataButton
                disabled={!datasetId}
                isLoading={isLoading}
                onClick={saveMessages}
            >
                Save
            </DataButton>
        </div>
    )
};

export default Messages;